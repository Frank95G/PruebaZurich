import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Store } from '@ngxs/store';
import { firstValueFrom, Observable } from 'rxjs';

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
  ToastBodyComponent,
  ToastComponent,
  ToasterComponent,  
} from '@coreui/angular';

// Componentes y servicios locales
import { PolicyService } from '../../../services/policy.service';
import { Policy, POLICY_TYPES, PolicyListResponse } from '../../../models/policy.model';
import { CancelPolicyModalComponent } from '../../../shared/cancel-policy-modal/cancel-policy-modal.component';
import { AuthState } from '../../../store/auth.state';
import { RequestCancelPolicyModalComponent } from '../../../shared/request-cancel-policy-modal/request-cancel-policy-modal.component';

@Component({
  selector: 'app-policy-list',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    CRow, CCol, CCard, CCardHeader, CCardBody,
    TableDirective, SpinnerComponent,
    PaginationModule,
    ButtonDirective,
    ToastComponent,
    ToasterComponent,
    ToastBodyComponent,
  ],
  templateUrl: './policy-list.component.html'
})
export class PolicyListComponent implements OnInit {
  private policyService = inject(PolicyService);
  public router = inject(Router);
  private modalService = inject(NgbModal);
  private store = inject(Store);
  protected usuario = this.store.select(AuthState.usuario);

  // Estado del componente
  policies: Policy[] = [];
  policyTypes = POLICY_TYPES;
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
    NumeroPoliza: '',  
    ClienteId: null as number | null,
    TipoPolizaId: null as number | null,
    Estado: '',
    FechaDesde: null as Date | null,
    FechaHasta: null as Date | null
  };

  userRol: string = '';
  userClienteIId: number | null = null;

  async ngOnInit(): Promise<void> {
    await this.verifyAuthentication();
    await this.loadPolicies();
  }

  private async verifyAuthentication(): Promise<void> {
    const token = await firstValueFrom(this.store.select(AuthState.token));
    if (!token) {
      this.router.navigate(['/login']);
      throw new Error('Usuario no autenticado');
    }
    else{
      this.usuario.subscribe(usuario => {
        this.userRol = usuario?.rol || '';
        this.userClienteIId = usuario?.clienteId || null;
      });
    }
  }

  async loadPolicies(): Promise<void> {
    
    var policies$ = await this.policyService.getPolicies(
      this.currentPage,
      this.pageSize,
      {
        ClienteId: this.filters.ClienteId,
        TipoPolizaId: this.filters.TipoPolizaId,
        Estado: this.filters.Estado,
        FechaDesde: this.filters.FechaDesde,
        FechaHasta: this.filters.FechaHasta,
        NumeroPoliza: this.filters.NumeroPoliza
      }
    );

    if (this.userRol === 'Cliente' && this.userClienteIId) {
      this.filters.ClienteId = this.userClienteIId;
    }


    try {
      policies$ = await this.policyService.getPolicies(
        this.currentPage, 
        this.pageSize, 
        this.filters
      );
      
      policies$.subscribe({
        next: (response) => this.handleSuccessResponse(response),
        error: (err) => this.handleErrorResponse(err)
      });
    } catch (error) {
      this.handleErrorResponse(error);
    }
  }

  private handleSuccessResponse(response: PolicyListResponse): void {
    this.policies = response.items;
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
    await this.loadPolicies();
  }

  async applyFilter(): Promise<void> {
    this.currentPage = 1;
    await this.loadPolicies();
  }

  async clearFilters(): Promise<void> {
    this.filters = {
      NumeroPoliza: '',
      ClienteId: null,
      TipoPolizaId: null,
      Estado: '',
      FechaDesde: null,
      FechaHasta: null
    };
    await this.loadPolicies();
  }

  viewPolicy(policyId: number): void {
    this.router.navigate(['/policy/view', policyId]);
  }

  editPolicy(policyId: number): void {
    this.router.navigate(['/policy/form', policyId]);
  }

  async cancelPolicy(policyId: number): Promise<void> {
    if (this.userRol === 'Cliente') {        
      const modalRef = this.modalService.open(RequestCancelPolicyModalComponent, { centered: true});     
      try {
        const confirmed = await modalRef.result;
        if (confirmed) { // Si el usuario confirmó
          this.loading = true;
          // Creamos el objeto con solo el campo a actualizar
          const updateData = {
            polizaId: policyId,
            estado: 'Solicitud de Cancelación'
          };
          const operation$: Observable<any> = this.policyService.reqCancelPolicy(policyId);
          operation$.subscribe({
            next: () => {
              this.loading = false;
              this.showSuccessToast('Solicitud de cancelación enviada correctamente');
              this.loadPolicies(); // Recargar la lista de pólizas
            },
            error: (err) => {
              this.loading = false;
              this.showErrorToast('Error al cancelar póliza: ' + err.error);
            }
          });
        }
      } catch { }
    }
    else{
      const modalRef = this.modalService.open(CancelPolicyModalComponent, { centered: true}); 
        try {
          const motivo = await modalRef.result;
          if (motivo) {
            this.loading = true;
            const result$ = await this.policyService.cancelPolicy(policyId, motivo);
            
            result$.subscribe({
              next: () => this.handleCancelSuccess(),
              error: (err) => this.handleCancelError(err)
            });
          }
        } catch { }
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

  private handleCancelSuccess(): void {
    this.showSuccessToast('Poliza cancelada exitosamente');
    this.loadPolicies();
  }

  private handleCancelError(error: any): void {
    this.loading = false;
    console.error('Error al cancelar póliza:', error);
    
    if (error.status === 401) {
      this.error = 'Sesión expirada. Redirigiendo...';
      setTimeout(() => this.router.navigate(['/login']), 2000);
    } else {
      this.error = error.error?.message || 'Error al cancelar la póliza';
    }
  }

  formatDate(dateString: string): string {
    if (!dateString) return '';
    const date = new Date(dateString);
    return date.toLocaleDateString('es-ES'); // Formato para español
  }
  
  getPolicyTypeName(id: number): string {
    const type = this.policyTypes.find(t => t.tipoPolizaId === id);
    return type ? type.nombre : 'Desconocido';
  }
}