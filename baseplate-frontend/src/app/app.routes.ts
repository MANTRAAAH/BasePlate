import { TenantListComponent } from './features/super-admin/tenant-list/tenant-list.component';
import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard'; // 👈 Importiamo il buttafuori
import { adminGuard } from './core/guards/admin.guard';
import { superAdminGuard } from './core/guards/super-admin.guard';

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
  {
    path: 'admin',
    canActivate: [adminGuard], // 👈 IL BUTTAFUORI ENTRA IN AZIONE QUI! Blocca chi non è loggato.
    loadComponent: () => import('./features/admin/dashboard-admin/dashboard-admin.component').then(m => m.DashboardAdminComponent)
  },
  {
    path: 'god-mode',
    canActivate:[superAdminGuard],
    loadComponent: () => import('./features/super-admin/dashboard/dashboard.component').then(c => c.DashboardComponent),
    children: [
      {
        path: 'lista-clienti',
        loadComponent: () => import('./features/super-admin/tenant-list/tenant-list.component').then(c => c.TenantListComponent)
      },
      {
        path: 'nuovo-cliente',
        loadComponent: () => import('./features/super-admin/tenant-form/tenant-form.component').then(c => c.TenantFormComponent)
      },
      // Quando visiti /god-mode, ti porta subito sulla lista
      { path: '', redirectTo: 'lista-clienti', pathMatch: 'full' }
    ]
  },

  // 4. ROTTA SALA (Per i Camerieri)
  {
    path: 'sala',
    canActivate: [authGuard], // 👈 Mettiamolo preventivamente anche in sala, visto che non è pubblica.
    loadComponent: () => import('./features/sala/dashboard-sala/dashboard-sala.component').then(m => m.DashboardSalaComponent)
  },

  // REGOLA DI DEFAULT: Se qualcuno digita solo localhost:4200, mandalo al menu
  { path: '', redirectTo: 'menu', pathMatch: 'full' },

  // REGOLA FALLBACK: Se qualcuno digita un URL che non esiste, rimandalo al menu
  { path: '**', redirectTo: 'menu' }
];
