import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ElementoBase } from '../models/prodotto.model';

@Injectable({
  providedIn: 'root'
})
export class ImpostazioniService {
  private http = inject(HttpClient);
  private baseUrl = 'http://localhost:5147/api';

  // "endpoint" sarà la stringa 'categorie', 'allergeni' o 'ingredienti'
  getTutti(endpoint: string): Observable<ElementoBase[]> {
    return this.http.get<ElementoBase[]>(`${this.baseUrl}/${endpoint}`);
  }

  crea(endpoint: string, nome: string): Observable<ElementoBase> {
    return this.http.post<ElementoBase>(`${this.baseUrl}/${endpoint}`, { nome });
  }

  modifica(endpoint: string, id: number, nome: string): Observable<ElementoBase> {
    return this.http.put<ElementoBase>(`${this.baseUrl}/${endpoint}/${id}`, { nome });
  }

  elimina(endpoint: string, id: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/${endpoint}/${id}`);
  }
}
