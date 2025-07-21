export interface AuthStateModel {
  token: string | null;
  user: User | null;
  loading: boolean;
  error: string | null;
}

export interface User {
  id: string;
  nombreUsuario: string;
  email: string;
  rol: string;
}

export interface RegisterPayload {
  nombreUsuario: string;
  email: string;
  password: string;
  confirmPassword: string;
}