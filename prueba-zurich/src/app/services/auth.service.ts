import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';

interface LoginResponse {
  token: string;
  expiresIn: number;
  usuario: {
    usuarioId: string;
    email: string;
    nombreUsuario: string;
    rol: string;
  };
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly apiUrl = `${environment.apiUrl}/Auth`;

  constructor(private http: HttpClient) {}

  login(credentials: { username: string; password: string }): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${this.apiUrl}/login`, credentials);
  }
  register(userData: { nombreUsuario: string, email: string, password: string }) {
    return this.http.post(`${this.apiUrl}/register`, userData);
  }
}