import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { map, Observable, switchMap } from 'rxjs';
import { environment } from '../../environments/environment';
import { PolicyListResponse, CancelPolicyRequest, Policy, PolicyType, CreatePolicyRequest, UpdatePolicyRequest } from '../models/policy.model';
import { AuthState } from '../store/auth.state';
import { Store } from '@ngxs/store';

@Injectable({
  providedIn: 'root'
})
export class PolicyService {
  private apiUrl = `${environment.apiUrl}/Polizas`;
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

  getPolicies(
    page: number = 1,
    pageSize: number = 10,
    filters: {
      ClienteId?: number | null;
      TipoPolizaId?: number | null;
      Estado?: string | null;
      FechaDesde?: Date | null;
      FechaHasta?: Date | null;
      NumeroPoliza?: string | null;
    } = {}
  ): Observable<PolicyListResponse> {
    return this.getHeaders().pipe(
      switchMap(headers => {
        let params = new HttpParams()
          .set('PageNumber', page.toString())
          .set('PageSize', pageSize.toString());

        if (filters.ClienteId) params = params.set('ClienteId', filters.ClienteId.toString());
        if (filters.TipoPolizaId) params = params.set('TipoPolizaId', filters.TipoPolizaId.toString());
        if (filters.Estado) params = params.set('Estado', filters.Estado);
        if (filters.FechaDesde) params = params.set('FechaDesde', filters.FechaDesde.toISOString());
        if (filters.FechaHasta) params = params.set('FechaHasta', filters.FechaHasta.toISOString());
        if (filters.NumeroPoliza) params = params.set('NumeroPoliza', filters.NumeroPoliza);

        return this.http.get<PolicyListResponse>(this.apiUrl, { params, headers });
      })
    );
  }

  cancelPolicy(policyId: number, motivo: string): Observable<void> {
    return this.getHeaders().pipe(
      switchMap(headers => {
        const request: CancelPolicyRequest = { motivo };
        return this.http.post<void>(`${this.apiUrl}/${policyId}/cancelar`, request, { headers });
      })
    );
  }

  createPolicy(policyData: CreatePolicyRequest): Observable<Policy> {
    return this.getHeaders().pipe(
      switchMap(headers => {
        return this.http.post<Policy>(this.apiUrl, policyData, { headers });
      })
    );
  }

  getPolicyById(policyId: number): Observable<Policy> {
    return this.getHeaders().pipe(
      switchMap(headers => {
        return this.http.get<Policy>(`${this.apiUrl}/${policyId}`, { headers });
      })
    );
  }

  updatePolicy(policyId: number, policyData: UpdatePolicyRequest): Observable<Policy> {
    return this.getHeaders().pipe(
      switchMap(headers => {
        return this.http.put<Policy>(`${this.apiUrl}/${policyId}`, policyData, { headers });
      })
    );
  }

  reqCancelPolicy(policyId: number): Observable<void> {
    return this.getHeaders().pipe(
      switchMap(headers => {
        return this.http.post<void>(`${this.apiUrl}/${policyId}/solcancelar`, null, { headers });
      })
    );
  } 

}