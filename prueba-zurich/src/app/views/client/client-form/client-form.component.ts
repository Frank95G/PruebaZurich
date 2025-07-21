import { Component, EventEmitter, Input, Output, inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule, AbstractControl, FormControl } from '@angular/forms';
import { ClienteService } from '../../../services/client.service';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule, Location } from '@angular/common';
import { Observable, take } from 'rxjs';

import {
  CardComponent,
  CardHeaderComponent,
  CardBodyComponent,
  FormControlDirective,
  FormDirective,
  ButtonDirective,
  RowComponent,
  ColComponent,
  ToastBodyComponent,
  ToastComponent,
  ToasterComponent,  
} from '@coreui/angular';
import { Client } from '../../../models/client.model';

@Component({
  selector: 'app-client-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    CardComponent, CardHeaderComponent, CardBodyComponent,
    FormControlDirective, FormDirective,
    ButtonDirective, RowComponent, ColComponent,
    ToastComponent,
    ToasterComponent,
    ToastBodyComponent,
  ],
  templateUrl: './client-form.component.html'
})
export class ClientFormComponent implements OnInit {
  private fb = inject(FormBuilder);
  private clientService = inject(ClienteService);
  private router = inject(Router);
  private route = inject(ActivatedRoute);
  private location = inject(Location); 

  @Input() clientId?: number;
  @Output() saved = new EventEmitter<boolean>();

  clientForm: FormGroup;
  loading = false;
  isEditMode = false;
  errorMessage: string | null = null;  
  showToast = false;
  toastMessage = '';
  toastColor = '';

  constructor() {
    this.clientForm = new FormGroup({
      identificacion: new FormControl('', [
        Validators.required,
        Validators.minLength(10),
        Validators.maxLength(10),
        Validators.pattern(/^[0-9]*$/)
      ]),
      nombre: new FormControl('', [
        Validators.required,
        Validators.pattern(/^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ\s]+$/)
      ]),
      email: new FormControl('', [
        Validators.required, 
        Validators.email,
        Validators.pattern(/^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$/)
      ]),
      telefono: new FormControl('', [
        Validators.required,
        Validators.pattern(/^\+?[0-9\s\-]+$/) // Permite +, números, espacios y guiones
      ]),
      direccion: new FormControl('', [Validators.required])
    });
  }

  ngOnInit(): void {
    this.clientId = Number(this.route.snapshot.paramMap.get('id'));

    if (this.clientId) {
      this.isEditMode = true;
      this.loadClientData();
    }
    // Validación para nombre al pegar
    this.clientForm.get('nombre')?.valueChanges.subscribe(value => {
      if (value) {
        const filteredValue = value.replace(/[^a-zA-ZáéíóúÁÉÍÓÚñÑüÜ\s]/g, '');
        if (value !== filteredValue) {
          this.clientForm.get('nombre')?.setValue(filteredValue, { emitEvent: false });
        }
      }
    });

    // Validación para teléfono al pegar
    this.clientForm.get('telefono')?.valueChanges.subscribe(value => {
      if (value) {
        const filteredValue = value.replace(/[^0-9+\-\s]/g, '');
        if (value !== filteredValue) {
          this.clientForm.get('telefono')?.setValue(filteredValue, { emitEvent: false });
        }
      }
    });

    //Validación de mail al pegar
    this.clientForm.get('email')?.valueChanges.subscribe(() => {
      this.clientForm.get('email')?.markAsTouched();
    });
  }

  loadClientData(): void {
    if (!this.clientId) return;

    this.loading = true;
    this.clientService.getClientById(this.clientId).pipe(take(1)).subscribe({
      next: (client: Client) => {
        this.clientForm.patchValue({
          identificacion: client.identificacion,
          nombre: client.nombre,
          email: client.email,
          telefono: client.telefono,
          direccion: client.direccion
        });
      },
      error: (err) => {
        console.error('Error loading client:', err);
      },
      complete: () => {
        this.loading = false;
      }
    });
  }

  onSubmit(): void {
    if (this.clientForm.invalid) return;

    this.loading = true;
    const formData = this.clientForm.value;

    const operation$: Observable<any> = this.isEditMode && this.clientId
      ? this.clientService.updateClient(this.clientId, formData)
      : this.clientService.createClient(formData);

      operation$.pipe(take(1)).subscribe({
      next: (result) => {        
        this.saved.emit(true);
        
        if (!this.isEditMode) {
          this.showSuccessToast('Cliente creado exitosamente! Redirigiendo...');
          setTimeout(() => this.router.navigate(['/client/view', result.clienteId]), 2000);
        }
        else{
          this.showSuccessToast('Cliente actualizado exitosamente!');          
        }
        this.loading = false;
      },
      error: (err) => {
        this.showErrorToast('Error: ' + err.error);
      }
    });
    this.loading = false;
  }
  
  onCancel(): void {
  if (this.clientForm.dirty) {
      // Opcional: Mostrar confirmación si hay cambios no guardados
      if (confirm('¿Estás seguro de cancelar? Los cambios no guardados se perderán.')) {
        this.location.back();
      }
    } else {
      this.location.back();
    }
  }
  
  private showErrorToast(message: string) {
    this.toastMessage = message;
    this.toastColor = 'danger';
    this.showToast = true;
    setTimeout(() => this.showToast = false, 5000);
  }

  private showSuccessToast(message: string) {
    this.toastMessage = message;
    this.toastColor = 'success';
    this.showToast = true;
    setTimeout(() => this.showToast = false, 5000);
  }
  onNameKeyPress(event: KeyboardEvent) {
    const allowedChars = /[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ\s]/;
    const inputChar = event.key;
    
    if (!allowedChars.test(inputChar)) {
      event.preventDefault();
    }
  }

  onPhoneKeyPress(event: KeyboardEvent) {
    const allowedChars = /[0-9+\-\s]/;
    const inputChar = event.key;
    
    if (!allowedChars.test(inputChar)) {
      event.preventDefault();
    }
  }
  onNumberKeyPress(event: KeyboardEvent): void {
    const allowedKeys = [
      '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
      'Backspace', 'Delete', 'Tab', 'ArrowLeft', 'ArrowRight'
    ];

    if (!allowedKeys.includes(event.key)) {
      event.preventDefault();
    }

    const input = event.target as HTMLInputElement;
    if (input.value.length >= 10 && event.key !== 'Backspace' && event.key !== 'Delete') {
      event.preventDefault();
    }
  }
}