import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

export interface CreateTenantRequest {
  nomeRistorante: string;
  partitaIva: string;
  emailManager: string;
}

@Injectable({
  providedIn: 'root'
})
export class SuperAdminService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/api/master/tenants`;

  // L'header magico per il God Mode
  private get godModeHeaders(): HttpHeaders {
    return new HttpHeaders({
      'X-Tenant-Id': '00000000-0000-0000-0000-000000000000'
      // Il JWT verrà iniettato in automatico dal tuo Interceptor (se lo hai già configurato)
    });
  }

  provisionTenant(request: CreateTenantRequest): Observable<any> {
    return this.http.post(`${this.apiUrl}/provision`, request, {
      headers: this.godModeHeaders
    });
  }

  // Da implementare poi sul backend
  // getTenants(): Observable<any[]> { ... }
}
