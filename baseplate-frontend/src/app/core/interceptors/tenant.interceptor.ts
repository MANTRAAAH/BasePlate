import { HttpInterceptorFn } from '@angular/common/http';

export const tenantInterceptor: HttpInterceptorFn = (req, next) => {
  // 1. Peschiamo l'ID del ristorante selezionato dal Local Storage
  const tenantId = localStorage.getItem('X-Tenant-Id');

  // 2. Escludiamo le rotte della God Mode (che usano il Guid.Empty forzato nel servizio)
  const isGodModeApi = req.url.includes('/api/master/');

  // 3. Se abbiamo un ID e non siamo in God Mode, iniettiamo l'header
  if (tenantId && !isGodModeApi) {
    const clonedReq = req.clone({
      headers: req.headers.set('X-Tenant-Id', tenantId)
    });
    return next(clonedReq);
  }

  // Altrimenti, fai passare la richiesta liscia così com'è
  return next(req);
};
