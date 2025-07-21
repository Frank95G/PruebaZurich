export interface Client {
  clienteId: number;
  usuarioId: number;
  identificacion: string;
  nombre: string;
  email: string;
  telefono: string;
  direccion: string;
  fechaRegistro: string;
}

export interface ClientListResponse {
  totalItems: number;
  pageNumber: number;
  pageSize: number;
  totalPages: number;
  items: Client[];
}

export interface CreateClientRequest {
  identificacion: string;
  nombre: string;
  email: string;
  telefono: string;
  direccion: string;
}

export interface UpdateClientRequest extends Partial<CreateClientRequest> {
  clientId: number;
}