import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

export const authGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  // Se l'utente ha il token, fallo passare (restituisce true)
  if (authService.hasToken()) {
    return true;
  }

  // Altrimenti, caccialo via verso la pagina di login e bloccalo (restituisce false)
  router.navigate(['/login']);
  return false;
};
