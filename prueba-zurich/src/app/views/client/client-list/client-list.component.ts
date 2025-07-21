import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { Store } from '@ngxs/store';
import { firstValueFrom } from 'rxjs';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

// CoreUI Components
import {
  RowComponent as CRow,
  ColComponent as CCol,
  CardComponent as CCard,
  CardHeaderComponent as CCardHeader,
  CardBodyComponent as CCardBody,
  TableDirective,
  SpinnerComponent,
  PaginationModule,
  ButtonDirective,    
    ToastComponent,
    ToasterComponent,
    ToastBodyComponent,
} from '@coreui/angular';

// Componentes y servicios locales
import { ClienteService } from '../../../services/client.service';
import { Client, ClientListResponse } from '../../../models/client.model';
import { AuthState } from '../../../store/auth.state';
import { DeleteClientModalComponent } from '../../../shared/delete-client-modal/delete-client-modal.component';

@Component({
  selector: 'app-client-list',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    CRow, CCol, CCard, CCardHeader, CCardBody,
    TableDirective, SpinnerComponent,
    PaginationModule,
    ButtonDirective,    
    ToastBodyComponent,
    ToastComponent,
    ToasterComponent,  
  ],
  templateUrl: './client-list.component.html'
})
export class ClientListComponent implements OnInit {
  private clientService = inject(ClienteService);
  public router = inject(Router);
  private store = inject(Store);
  private modalService = inject(NgbModal);

  // Estado del componente
  clients: Client[] = [];
  loading = false;
  error = '';
  showToast = false;
  toastMessage = '';
  toastColor = '';
  
  // Paginación
  currentPage = 1;
  pageSize = 10;
  totalItems = 0;
  totalPages = 0;
  
  // Filtros
  filters = {
    Identificacion: '',  
    Nombre: '',
    Email: '',
    Telefono: '',
    Direccion: ''
  };

  async ngOnInit(): Promise<void> {
    await this.verifyAuthentication();
    await this.loadClients();
  }

  private async verifyAuthentication(): Promise<void> {
    const token = await firstValueFrom(this.store.select(AuthState.token));
    if (!token) {
      this.router.navigate(['/login']);
      throw new Error('Usuario no autenticado');
    }
  }

  async loadClients(): Promise<void> {
    
    try {
      var clients$ = await this.clientService.getClients(
      this.currentPage,
      this.pageSize,
        {
          Identificacion: this.filters.Identificacion,
          Nombre: this.filters.Nombre,
          Email: this.filters.Email,
          Telefono: this.filters.Telefono,
          Direccion: this.filters.Direccion
        }
      );

      clients$.subscribe({
        next: (response) => this.handleSuccessResponse(response),
        error: (err) => this.handleErrorResponse(err)
      });
    } catch (error) {
      this.handleErrorResponse(error);
    }
  }

  private handleSuccessResponse(response: ClientListResponse): void {
    this.clients = response.items;
    this.totalItems = response.totalItems;
    this.totalPages = response.totalPages;
    this.loading = false;
  }

  private handleErrorResponse(error: any): void {
    this.loading = false;
    console.error('Error:', error);
    
    if (error.status === 401) {
      this.error = 'Sesión expirada. Redirigiendo...';
      setTimeout(() => this.router.navigate(['/login']), 2000);
    } else {
      this.error = 'Error al cargar las pólizas. Intente nuevamente.';
    }
  }

  async onPageChange(page: number): Promise<void> {
    this.currentPage = page;
    await this.loadClients();
  }

  async applyFilter(): Promise<void> {
    this.currentPage = 1;
    await this.loadClients();
  }

  async clearFilters(): Promise<void> {
    this.filters = {
      Identificacion: '',
      Nombre: '',
      Email: '',
      Telefono: '',
      Direccion: ''
    };
    await this.loadClients();
  }

  viewClient(clientId: number): void {
    this.router.navigate(['/client/view', clientId]);
  }

  editClient(clientId: number): void {
    this.router.navigate(['/client/form', clientId]);
  }

  async deleteClient(clientId: number): Promise<void> {
    const modalRef = this.modalService.open(DeleteClientModalComponent, { centered: true}); 

    try {
      const confirmed = await modalRef.result;
      if (confirmed) { // Si el usuario confirmó
        this.loading = true;
        this.clientService.deleteClient(clientId).subscribe({
          next: () => {
            this.loading = false;
            this.showSuccessToast('Cliente eliminado correctamente');
            this.loadClients(); // Recargar la lista de clientes
          },
          error: (err) => {
            this.loading = false;
            this.showErrorToast('Error al eliminar cliente: ' + err.error);
          }
        });
      }
    } catch { }
}

  formatDate(dateString: string): string {
    if (!dateString) return '';
    const date = new Date(dateString);
    return date.toLocaleDateString('es-ES'); // Formato para español
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