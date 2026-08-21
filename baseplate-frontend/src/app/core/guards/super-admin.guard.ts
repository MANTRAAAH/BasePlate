import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';

export const superAdminGuard: CanActivateFn = (route, state) => {
  const router = inject(Router);

  // Peschiamo il ruolo dal localStorage
  const ruolo = localStorage.getItem('ruolo');

  // Se è Admin, lo facciamo passare
  if (ruolo === 'Admin') {
    return true;
  }

  // Altrimenti, redirect immediato al login o alla home
  router.navigate(['/']);
  return false;
};
