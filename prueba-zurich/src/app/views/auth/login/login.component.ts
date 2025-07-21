import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { IconDirective } from '@coreui/icons-angular';
import {
  ButtonDirective,
  CardBodyComponent,
  CardComponent,
  CardGroupComponent,
  ColComponent,
  ContainerComponent,
  FormControlDirective,
  FormDirective,
  InputGroupComponent,
  InputGroupTextDirective,
  RowComponent,
  ToastBodyComponent,
  ToastComponent,
  ToasterComponent,
  ToastHeaderComponent
} from '@coreui/angular';
import { AuthService } from '../../../services/auth.service'; 
import { Store } from '@ngxs/store';
import { Login } from '../../../store/auth.state';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    RouterLink,
    ContainerComponent,
    RowComponent,
    ColComponent,
    CardGroupComponent,
    CardComponent,
    CardBodyComponent,
    FormDirective,
    InputGroupComponent,
    InputGroupTextDirective,
    IconDirective,
    FormControlDirective,
    ButtonDirective,
    ToastComponent,
    ToasterComponent,
    ToastHeaderComponent,
    ToastBodyComponent
  ]
})
export class LoginComponent {


  credentials = {
    username: '',
    password: ''
  };
  loading = false;
  errorMessage = '';
  showToast = false;
  toastMessage = '';
  toastColor = 'danger';

  private store = inject(Store);
  private authService = inject(AuthService);
  private router = inject(Router);

  onSubmit() {
    // Validación básica
    if (!this.credentials.username || !this.credentials.password) {
      this.showErrorToast('Por favor ingrese username y contraseña');
      return;
    }
    this.loading = true;
    this.errorMessage = '';

    this.authService.login(this.credentials).subscribe({
      next: (response) => {
        this.store.dispatch(new Login({
          token: response.token,
          usuario: response.usuario
        }));
        this.router.navigate(['/policy/list']);
      },
      error: (error) => {
        this.showErrorToast(error.error?.message || 'Error en el login. Por favor intente nuevamente.');
      },
      complete: () => {
        this.loading = false;
      }
    });
    this.loading = false;
  }

  private showErrorToast(message: string) {
    this.toastMessage = message;
    this.toastColor = 'danger';
    this.showToast = true;
    setTimeout(() => this.showToast = false, 5000);
  }
}