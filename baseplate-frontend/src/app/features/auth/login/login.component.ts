import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  template: `
    <div class="min-h-screen bg-slate-900 flex flex-col justify-center py-12 sm:px-6 lg:px-8 font-sans">
      <div class="sm:mx-auto sm:w-full sm:max-w-md text-center">
        <span class="text-white font-black text-4xl tracking-wider">Base<span class="text-amber-500">Plate</span></span>
        <h2 class="mt-6 text-center text-3xl font-extrabold text-white">
          Accesso Gestionale
        </h2>
      </div>

      <div class="mt-8 sm:mx-auto sm:w-full sm:max-w-md">
        <div class="bg-slate-800 py-8 px-4 shadow-xl sm:rounded-xl sm:px-10 border border-slate-700">

          <!-- Messaggio di errore -->
          <div *ngIf="errore()" class="mb-4 bg-red-500/10 border border-red-500 text-red-500 px-4 py-3 rounded-lg text-sm text-center font-medium">
            {{ errore() }}
          </div>

          <form [formGroup]="loginForm" (ngSubmit)="onSubmit()" class="space-y-6">
            <div>
              <label class="block text-sm font-medium text-slate-300">Email</label>
              <div class="mt-1">
                <input formControlName="email" type="email" class="appearance-none block w-full px-3 py-2 border border-slate-600 rounded-lg shadow-sm placeholder-slate-400 bg-slate-900 text-white focus:outline-none focus:ring-amber-500 focus:border-amber-500 sm:text-sm">
              </div>
            </div>

            <div>
              <label class="block text-sm font-medium text-slate-300">Password</label>
              <div class="mt-1">
                <input formControlName="password" type="password" class="appearance-none block w-full px-3 py-2 border border-slate-600 rounded-lg shadow-sm placeholder-slate-400 bg-slate-900 text-white focus:outline-none focus:ring-amber-500 focus:border-amber-500 sm:text-sm">
              </div>
            </div>

            <div>
              <button type="submit" [disabled]="loginForm.invalid || inCaricamento()" class="w-full flex justify-center py-2.5 px-4 border border-transparent rounded-lg shadow-sm text-sm font-bold text-white bg-amber-500 hover:bg-amber-600 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-amber-500 focus:ring-offset-slate-900 disabled:opacity-50 disabled:cursor-not-allowed transition-colors">
                <span *ngIf="!inCaricamento()">Entra</span>
                <span *ngIf="inCaricamento()">Accesso in corso...</span>
              </button>
            </div>
          </form>

          <div class="mt-6 text-center text-sm text-slate-400">
            <a href="/menu" class="hover:text-amber-500 transition-colors">Torna al menù pubblico</a>
          </div>

        </div>
      </div>
    </div>
  `
})
export class LoginComponent {
  private fb = inject(FormBuilder);
  private authService = inject(AuthService);
  private router = inject(Router);

  loginForm = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', Validators.required]
  });

  errore = signal<string | null>(null);
  inCaricamento = signal<boolean>(false);

  onSubmit() {
    if (this.loginForm.valid) {
      this.inCaricamento.set(true);
      this.errore.set(null);

      const { email, password } = this.loginForm.value;

      this.authService.login(email!, password!).subscribe({
        next: () => {
          // Login riuscito! Vai alla dashboard
          this.router.navigate(['/admin']);
        },
        error: (err) => {
          this.inCaricamento.set(false);
          // Se il backend ci manda un messaggio, lo stampiamo. Altrimenti errore generico.
          this.errore.set(err.error?.message || 'Errore di connessione al server.');
        }
      });
    }
  }
}
