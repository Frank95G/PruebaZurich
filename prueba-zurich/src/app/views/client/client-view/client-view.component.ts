import { Component, OnInit, inject } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { ClienteService } from '../../../services/client.service';
import { Client } from '../../../models/client.model';
import { firstValueFrom } from 'rxjs';

// CoreUI Components
import {
  CardComponent,
  CardHeaderComponent,
  CardBodyComponent,
  ButtonDirective,
  RowComponent,
  ColComponent,
    ToastBodyComponent,
    ToastComponent,
    ToasterComponent,  
} from '@coreui/angular';
import { DeleteClientModalComponent } from '../../../shared/delete-client-modal/delete-client-modal.component';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-client-view',
  standalone: true,
  imports: [
    CommonModule,
    CardComponent, CardHeaderComponent, CardBodyComponent,
    ButtonDirective, RowComponent, ColComponent,
    ToastBodyComponent,
    ToastComponent,
    ToasterComponent,  
  ],
  templateUrl: './client-view.component.html'
})
export class ClientViewComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private clientService = inject(ClienteService);
  private modalService = inject(NgbModal);

  clientId!: number;
  clientData!: Client;
  loading = false;
  errorMessage: string | null = null;
  
  showToast = false;
  toastMessage = '';
  toastColor = '';

  async ngOnInit(): Promise<void> {
    this.clientId = Number(this.route.snapshot.paramMap.get('id'));

    if (isNaN(this.clientId)) {
      this.errorMessage = 'ID de cliente inválido';
      return;
    }

    await this.loadClientData();
  }

  async loadClientData(): Promise<void> {
    this.loading = true;
    this.errorMessage = null;
    
    try {
      const client$ = this.clientService.getClientById(this.clientId);
      this.clientData = await firstValueFrom(client$);
    } catch (error) {
      console.error('Error al cargar el cliente:', error);
      this.errorMessage = 'Error al cargar los datos del cliente';
    } finally {
      this.loading = false;
    }
  }

  formatDate(dateString: string): string {
    if (!dateString) return '';
    const date = new Date(dateString);
    return date.toLocaleDateString('es-ES');
  }

  goToEdit(): void {
    if (this.clientId) {
      this.router.navigate(['/client/form', this.clientId]);
    } else {
      this.router.navigate(['/client/form']);
    }
  }

  goBack(): void {
    this.router.navigate(['/client/list']);
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
            this.router.navigate(['/client/list']); // Redirigir a la lista de clientes
          },
          error: (err) => {
            this.loading = false;
            this.showErrorToast('Error al eliminar cliente: ' + err.error);
          }
        });
      }
    } catch { }
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