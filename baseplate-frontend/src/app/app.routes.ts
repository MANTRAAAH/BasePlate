import { Routes } from '@angular/router';

export const routes: Routes = [
  // 1. ROTTA PUBBLICA (Il menù per i clienti)
  {
    path: 'menu',
    loadComponent: () => import('./features/lista-prodotti/lista-prodotti.component').then(m => m.ListaProdottiComponent)
  },

  // 2. ROTTA DI ACCESSO STAFF
  {
    path: 'login',
    loadComponent: () => import('./features/auth/login/login.component').then(m => m.LoginComponent)
  },

  // 3. ROTTA GESTIONALE (Per il Ristoratore)
  // In futuro qui metteremo il "Guard" che bloccherà l'accesso a chi non ha il Token JWT Admin
  {
    path: 'admin',
    loadComponent: () => import('./features/admin/dashboard-admin/dashboard-admin.component').then(m => m.DashboardAdminComponent)
  },

  // 4. ROTTA SALA (Per i Camerieri)
  // Anche qui ci sarà un Guard legato al ruolo "Cameriere"
  {
    path: 'sala',
    loadComponent: () => import('./features/sala/dashboard-sala/dashboard-sala.component').then(m => m.DashboardSalaComponent)
  },

  // REGOLA DI DEFAULT: Se qualcuno digita solo localhost:4200, mandalo al menu
  { path: '', redirectTo: 'menu', pathMatch: 'full' },

  // REGOLA FALLBACK: Se qualcuno digita un URL che non esiste, rimandalo al menu
  { path: '**', redirectTo: 'menu' }
];
