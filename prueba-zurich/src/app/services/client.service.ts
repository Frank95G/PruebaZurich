import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { map, Observable, switchMap } from 'rxjs';
import { environment } from '../../environments/environment';
import { Client, ClientListResponse, CreateClientRequest, UpdateClientRequest } from '../models/client.model';
import { AuthState } from '../store/auth.state';
import { Store } from '@ngxs/store';

@Injectable({
  providedIn: 'root'
})
export class ClienteService {
  private apiUrl = `${environment.apiUrl}/Clientes`;
  private http = inject(HttpClient);
  private store = inject(Store);

  private getHeaders(): Observable<HttpHeaders> {
    return this.store.select(AuthState.token).pipe(
      map(token => {
        if (!token) {
          throw new Error('Authentication token not available');
        }
        return new HttpHeaders({
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${token}`
        });
      })
    );
  }

  getClients(
    page: number = 1,
    pageSize: number = 10,
    filters: {
      Identificacion?: string | null;
      Nombre?: string | null;
      Email?: string | null;
      Telefono?: string | null;
      Direccion?: string | null;
    } = {}
  ): Observable<ClientListResponse> {
    return this.getHeaders().pipe(
      switchMap(headers => {
        let params = new HttpParams()
          .set('PageNumber', page.toString())
          .set('PageSize', pageSize.toString());

        if (filters.Identificacion) params = params.set('Identificacion', filters.Identificacion);
        if (filters.Nombre) params = params.set('Nombre', filters.Nombre);
        if (filters.Email) params = params.set('Email', filters.Email);
        if (filters.Telefono) params = params.set('Telefono', filters.Telefono);
        if (filters.Direccion) params = params.set('Direccion', filters.Direccion);

        return this.http.get<ClientListResponse>(this.apiUrl, { params, headers });
      })
    );
  }

  createClient(clientData: CreateClientRequest): Observable<Client> {
    return this.getHeaders().pipe(
      switchMap(headers => {
        return this.http.post<Client>(this.apiUrl, clientData, { headers });
      })
    );
  }

  getClientById(clientId: number): Observable<Client> {
    return this.getHeaders().pipe(
      switchMap(headers => {
        return this.http.get<Client>(`${this.apiUrl}/${clientId}`, { headers });
      })
    );
  }

  updateClient(clientId: number, clientData: UpdateClientRequest): Observable<Client> {
    return this.getHeaders().pipe(
      switchMap(headers => {
        return this.http.put<Client>(`${this.apiUrl}/${clientId}`, clientData, { headers });
      })
    );
  }

  deleteClient(clientId: number): Observable<void> {
    return this.getHeaders().pipe(
      switchMap(headers => {
        return this.http.delete<void>(`${this.apiUrl}/${clientId}`, { headers });
      })
    );
  }

}