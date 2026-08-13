import { Injectable, inject, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { tap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private http = inject(HttpClient);
  private baseUrl = 'http://localhost:5147/api/auth';

  // Usiamo un Signal per far sapere a tutta l'app se sei loggato o no
  isLoggedIn = signal<boolean>(this.hasToken());

login(email: string, password: string) {
    return this.http.post<any>(`${this.baseUrl}/login`, { email, password }).pipe(
      tap(response => {
        localStorage.setItem('token', response.token);
        localStorage.setItem('nomeUtente', response.nome);
        localStorage.setItem('ruolo', response.ruolo); // 👈 ORA SALVIAMO IL RUOLO!
        this.isLoggedIn.set(true);
      })
    );
  }

  logout() {
    localStorage.removeItem('token');
    localStorage.removeItem('nomeUtente');
    localStorage.removeItem('ruolo'); // 👈 Puliamo tutto al logout
    this.isLoggedIn.set(false);
  }

  // Aggiungi questo metodo sotto getToken()
  getRuolo(): string | null {
    return localStorage.getItem('ruolo');
  }

  hasToken(): boolean {
    // Controlla se esiste un token salvato
    return !!localStorage.getItem('token');
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }
}
