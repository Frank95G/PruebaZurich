import { Component, EventEmitter, Input, Output, inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule, FormControl, ValidatorFn, AbstractControl, ValidationErrors } from '@angular/forms';
import { PolicyService } from '../../../services/policy.service';
import { ClienteService } from '../../../services/client.service';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule, Location } from '@angular/common';


// CoreUI Components
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
  InputGroupComponent,
  InputGroupTextDirective,
} from '@coreui/angular';
import { Policy, POLICY_TYPES } from '../../../models/policy.model';
import { Client } from '../../../models/client.model';
import { debounceTime, distinctUntilChanged, map, Observable, of, switchMap, take } from 'rxjs';

@Component({
  selector: 'app-policy-form',
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
    InputGroupTextDirective,
    InputGroupComponent
  ],
  templateUrl: './policy-form.component.html'
})
export class PolicyFormComponent implements OnInit {

  private policyService = inject(PolicyService);
  private clientService = inject(ClienteService);
  private router = inject(Router);
  private route = inject(ActivatedRoute);
  private location = inject(Location); 

  @Input() policyId?: number;
  @Input() clientId?: number;
  @Output() saved = new EventEmitter<boolean>();

  policyForm: FormGroup;
  loading = false;
  isEditMode = false;
  policyTypes = POLICY_TYPES;
  errorMessage: string | null = null;  
  showToast = false;
  toastMessage = '';
  toastColor = '';
  today = new Date().toISOString().split('T')[0];
  
  clients: Pick<Client, 'clienteId' | 'nombre'>[] = []; // Solo almacena ID y nombre
  filteredClients: Pick<Client, 'clienteId' | 'nombre'>[] = [];
  searchControl = new FormControl('');

  constructor() {
    this.policyForm = new FormGroup({
      clienteId: new FormControl('', [
        Validators.required,
        Validators.min(1),        
        Validators.pattern(/^[1-9]\d*$/)
      ]),
      tipoPolizaId: new FormControl('', [
        Validators.required,
        Validators.min(1),
        Validators.pattern(/^[1-9]\d*$/)
      ]),
      fechaInicio: new FormControl('', [
        Validators.required,
        this.futureDateValidator()
      ]),
      fechaExpiracion: new FormControl('', [
        Validators.required,
        this.futureDateValidator()
      ]),
      montoAsegurado: new FormControl('', [
        Validators.required,
        Validators.min(0.01)
      ])
      
    }, { validators: this.dateValidator });
  }

  ngOnInit(): void {
    
    this.policyId = Number(this.route.snapshot.paramMap.get('id'));
    
    this.setupAutocomplete();
    
    if (this.policyId) {
      this.isEditMode = true;
      this.loadPolicyData();
    } else if (this.clientId) {
      this.policyForm.patchValue({ clienteId: this.clientId });
    }
  }

  loadPolicyData(): void {
    if (!this.policyId) return;

    this.loading = true;
    this.policyService.getPolicyById(this.policyId).pipe(take(1)).subscribe({
      next: (policy: Policy) => {
        this.policyForm.patchValue({
          clienteId: policy.clienteId,
          tipoPolizaId: policy.tipoPolizaId,
          fechaInicio: policy.fechaInicio.split('T')[0],
          fechaExpiracion: policy.fechaExpiracion.split('T')[0],
          montoAsegurado: policy.montoAsegurado
        });
      },
      error: (err) => {
        console.error('Error loading policy:', err);
      },
      complete: () => {
        this.loading = false;
      }
    });    
    this.loading = false;
  }

  futureDateValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const selectedDate = new Date(control.value);
      const today = new Date();
      today.setHours(0, 0, 0, 0);
      
      return selectedDate < today ? { pastDate: true } : null;
    };
  }

  // Validador de fechas (mejorado)
  dateValidator: ValidatorFn = (group: AbstractControl): ValidationErrors | null => {
    const formGroup = group as FormGroup;
    const start = formGroup.get('fechaInicio')?.value;
    const end = formGroup.get('fechaExpiracion')?.value;

    if (!start || !end) return null;

    return new Date(start) >= new Date(end) ? { invalidDates: true } : null;
  };

  onSubmit(): void {
    if (this.policyForm.invalid) return;

    this.loading = true;
    const formData = this.policyForm.value;
    formData.fechaInicio = new Date(formData.fechaInicio).toISOString();
    formData.fechaExpiracion = new Date(formData.fechaExpiracion).toISOString();

    const operation$: Observable<any> = this.isEditMode && this.policyId
      ? this.policyService.updatePolicy(this.policyId, formData)
      : this.policyService.createPolicy(formData);

      operation$.pipe(take(1)).subscribe({
      next: (result) => {        
        this.saved.emit(true);
        
        if (!this.isEditMode) {          
          this.showSuccessToast('Póliza emitida exitosamente! Redirigiendo...');
          setTimeout(() => this.router.navigate(['/policy/view', result.polizaId]), 2000);
        }
        else{
          this.showSuccessToast('Póliza actualizada exitosamente!');          
        }
        this.loading = false;
      },
      error: (err) => {
        this.showErrorToast('Error: ' + err.error);
      },
      complete: () => {
        this.loading = false;
      }
    });
    
  }
  onCancel(): void {
  if (this.policyForm.dirty) {
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
  
 private setupAutocomplete(): void {
    this.searchControl.valueChanges
      .pipe(
        debounceTime(300),
        distinctUntilChanged(),
        switchMap(query => this.filterClients(query ?? ''))
      )
      .subscribe(filtered => this.filteredClients = filtered);
  }

  private filterClients(query: string): Observable<Pick<Client, 'clienteId' | 'nombre'>[]> {
    if (!query || query.length < 2) {
      return of([]);
    }
    return this.clientService.getClients(1, 5, { Nombre: query })
      .pipe(
        map(response => response.items.map(client => ({
          clienteId: client.clienteId,
          nombre: client.nombre
        })))
      );
  }

  selectClient(client: Pick<Client, 'clienteId' | 'nombre'>): void {
    const clienteIdControl = this.policyForm.get('clienteId');
    if (clienteIdControl) {
      clienteIdControl.setValue(client.clienteId);
    }
    this.searchControl.setValue(client.nombre);
    this.filteredClients = [];
  }
}