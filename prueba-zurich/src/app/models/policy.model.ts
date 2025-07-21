export interface Policy {
  polizaId: number;
  numeroPoliza: string;
  clienteId: number;
  clienteNombre: string;
  tipoPolizaId: number;
  tipoPoliza: string;
  fechaInicio: string;
  fechaExpiracion: string;
  montoAsegurado: number;
  estado: string;
  fechaEmision: string;
}

export interface PolicyListResponse {
  totalItems: number;
  pageNumber: number;
  pageSize: number;
  totalPages: number;
  items: Policy[];
}
export interface CancelPolicyRequest {
  motivo: string;
}

export interface CreatePolicyRequest {
  clienteId: number;
  tipoPolizaId: number;
  fechaInicio: string;
  fechaExpiracion: string;
  montoAsegurado: number;
}

export interface UpdatePolicyRequest extends Partial<CreatePolicyRequest> {
  polizaId: number;
  estado: string;
}

export interface PolicyType {
  tipoPolizaId: number;
  nombre: string;
  descripcion?: string;
}

export const POLICY_TYPES: PolicyType[] = [
  { tipoPolizaId: 1, nombre: 'Vida' },
  { tipoPolizaId: 2, nombre: 'Automóvil' },
  { tipoPolizaId: 3, nombre: 'Salud' },
  { tipoPolizaId: 4, nombre: 'Hogar' }
];