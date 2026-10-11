import { Injectable, inject, signal } from '@angular/core';
import { Router } from '@angular/router';

export interface ImpersonatedTenant {
  id: string;
  nome: string;
}

@Injectable({
  providedIn: 'root'
})
export class ImpersonationService {
  private router = inject(Router);

  private readonly KEY_ACTIVE = 'Impersonation-Active';
  private readonly KEY_TENANT_ID = 'X-Tenant-Id';
  private readonly KEY_TENANT_NAME = 'Tenant-Name';

  // Signals reattivi accessibili da tutta l'applicazione
  isImpersonating = signal<boolean>(this.checkIfActive());
  currentTenantId = signal<string | null>(localStorage.getItem(this.KEY_TENANT_ID));
  currentTenantName = signal<string | null>(localStorage.getItem(this.KEY_TENANT_NAME));

  private checkIfActive(): boolean {
    return localStorage.getItem(this.KEY_ACTIVE) === 'true';
  }

  startImpersonation(tenant: { id: string; nome: string }): void {
    localStorage.setItem(this.KEY_ACTIVE, 'true');
    localStorage.setItem(this.KEY_TENANT_ID, tenant.id);
    localStorage.setItem(this.KEY_TENANT_NAME, tenant.nome);

    this.isImpersonating.set(true);
    this.currentTenantId.set(tenant.id);
    this.currentTenantName.set(tenant.nome);

    // Naviga direttamente all'interfaccia di amministrazione del ristorante
    this.router.navigate(['/admin']);
  }

  stopImpersonation(): void {
    localStorage.removeItem(this.KEY_ACTIVE);
    localStorage.removeItem(this.KEY_TENANT_ID);
    localStorage.removeItem(this.KEY_TENANT_NAME);

    this.isImpersonating.set(false);
    this.currentTenantId.set(null);
    this.currentTenantName.set(null);

    // Ritorna immediatamente alla console God Mode
    this.router.navigate(['/god-mode/lista-clienti']);
  }
}

