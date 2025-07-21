import { Component, OnInit, inject } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { PolicyService } from '../../../services/policy.service';
import { Policy, POLICY_TYPES } from '../../../models/policy.model';
import { firstValueFrom } from 'rxjs';

// CoreUI Components
import {
  CardComponent,
  CardHeaderComponent,
  CardBodyComponent,
  ButtonDirective,
  RowComponent,
  ColComponent
} from '@coreui/angular';

@Component({
  selector: 'app-policy-view',
  standalone: true,
  imports: [
    CommonModule,
    CardComponent, CardHeaderComponent, CardBodyComponent,
    ButtonDirective, RowComponent, ColComponent
  ],
  templateUrl: './policy-view.component.html'
})
export class PolicyViewComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private policyService = inject(PolicyService);

  policyId!: number;
  policyData!: Policy;
  loading = false;
  errorMessage: string | null = null;  
  policyTypes = POLICY_TYPES;

  async ngOnInit(): Promise<void> {
    this.policyId = Number(this.route.snapshot.paramMap.get('id'));
    
    if (isNaN(this.policyId)) {
      this.errorMessage = 'ID de póliza inválido';
      return;
    }

    await this.loadPolicyData();
  }

  async loadPolicyData(): Promise<void> {
    this.loading = true;
    this.errorMessage = null;
    
    try {
      const policy$ = this.policyService.getPolicyById(this.policyId);
      this.policyData = await firstValueFrom(policy$);
    } catch (error) {
      console.error('Error al cargar la póliza:', error);
      this.errorMessage = 'Error al cargar los datos de la póliza';
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
    if (this.policyId) {
      this.router.navigate(['/policy/form', this.policyId]);
    } else {
      this.router.navigate(['/policy/form']);
    }
  }

  goBack(): void {
    this.router.navigate(['/policy/list']);
  }
  
  getPolicyTypeName(id: number): string {
    const type = this.policyTypes.find(t => t.tipoPolizaId === id);
    return type ? type.nombre : 'Desconocido';
  }
}