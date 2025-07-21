import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink } from '@angular/router';
import { FormsModule, ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { IconDirective } from '@coreui/icons-angular';
import {
  ButtonDirective,
  CardBodyComponent,
  CardComponent,
  ColComponent,
  ContainerComponent,
  FormControlDirective,
  FormDirective,
  InputGroupComponent,
  InputGroupTextDirective,
  RowComponent,
  FormFeedbackComponent,
  ToastBodyComponent,
  ToastComponent,
  ToasterComponent,
} from '@coreui/angular';
import { AuthService } from '../../../services/auth.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    RouterLink,
    ContainerComponent,
    RowComponent,
    ColComponent,
    CardComponent,
    CardBodyComponent,
    FormDirective,
    InputGroupComponent,
    InputGroupTextDirective,
    IconDirective,
    FormControlDirective,
    ButtonDirective,
    FormFeedbackComponent,
    ToastComponent,
    ToasterComponent,
    ToastBodyComponent
  ]
})
export class RegisterComponent {
  registerForm = this.fb.group({
    nombreUsuario: ['', [Validators.required, Validators.minLength(3)]],
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(6)]],
    confirmPassword: ['', Validators.required]
  }, { 
    validator: this.passwordMatchValidator 
  });

  loading = false;
  showToast = false;
  toastMessage = '';
  toastColor = '';

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router
  ) {}

  passwordMatchValidator(formGroup: any) {
    const password = formGroup.get('password').value;
    const confirmPassword = formGroup.get('confirmPassword').value;
    return password === confirmPassword ? null : { mismatch: true };
  }

  onSubmit() {
    if (this.registerForm.invalid) {
      this.showErrorToast('Por favor complete todos los campos correctamente');
      return;
    }

    this.loading = true;

    this.authService.register(this.registerForm.value).subscribe({
      next: () => {
        this.loading = false;
        this.showSuccessToast('Registro exitoso! Redirigiendo...');
        setTimeout(() => this.router.navigate(['/login']), 2000);
      },
      error: (error) => {
        this.loading = false;
        this.showErrorToast(error.error?.message || 'Error en el registro. Por favor intente nuevamente.');
      }
    });
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
}