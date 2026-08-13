import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

export const adminGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  // 1. Controlliamo se è loggato e se il ruolo è esattamente "Admin"
  if (authService.hasToken() && authService.getRuolo() === 'Admin') {
    return true; // Prego, capo!
  }

  // 2. Se è un cameriere (o non loggato) che prova a fare il furbo, mandalo al menu
  router.navigate(['/menu']);
  return false;
};
