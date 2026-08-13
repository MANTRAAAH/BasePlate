// prodotti.service.ts
import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ProdottoReadDto, CreaProdottoDto } from '../models/prodotto.model';

@Injectable({
  providedIn: 'root'
})
export class ProdottiService {
  private http = inject(HttpClient); // Usiamo il nuovo inject() di Angular

  // Sostituisci con la porta del tuo backend .NET
  private apiUrl = 'http://localhost:5147/api/prodotti';

  // Recupera tutti i prodotti
  getProdotti(): Observable<ProdottoReadDto[]> {
    return this.http.get<ProdottoReadDto[]>(this.apiUrl);
  }

  // Crea un nuovo prodotto
  creaProdotto(prodotto: CreaProdottoDto): Observable<any> {
    return this.http.post(this.apiUrl, prodotto);
  }

  // Aggiungi questo metodo sotto getProdotti() e creaProdotto()
  getLookupDati(): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/lookup`);
  }
  getProdotto(id: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/${id}`);
  }

  // MODIFICA un prodotto
  aggiornaProdotto(id: number, prodotto: any): Observable<any> {
    return this.http.put(`${this.apiUrl}/${id}`, prodotto);
  }

  // ELIMINA un prodotto
  eliminaProdotto(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
}
