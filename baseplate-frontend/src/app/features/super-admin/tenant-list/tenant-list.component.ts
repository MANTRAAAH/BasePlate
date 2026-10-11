import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { ReactiveFormsModule, FormGroup, FormControl, Validators } from '@angular/forms';
import { SuperAdminService } from '../../../core/services/super-admin.service';
import { ImpersonationService } from '../../../core/services/impersonation.service';

@Component({
  selector: 'app-tenant-list',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './tenant-list.component.html',
  styleUrl: './tenant-list.component.scss'
})
export class TenantListComponent implements OnInit {
  private superAdminService = inject(SuperAdminService);
  private impersonationService = inject(ImpersonationService);
  private router = inject(Router);

  tenants: any[] = [];
  isLoading = true;

  // Variabili per la modale
  isModalOpen = false;
  editingTenantId: string | null = null;
  editForm = new FormGroup({
    nomeRistorante: new FormControl('', [Validators.required]),
    partitaIva: new FormControl('', [Validators.required, Validators.maxLength(11)]),
    isActive: new FormControl(true)
  });
  isDeleteModalOpen = false;
  tenantToDelete: any = null;
  confermaNomeCtrl = new FormControl('');


  get clientiAttivi(): number {
    return this.tenants.filter(t => t.isActive).length;
  }

  ngOnInit() {
    this.caricaClienti();
  }

  caricaClienti() {
    this.superAdminService.getTenants().subscribe({
      next: (data) => {
        this.tenants = data;
        this.isLoading = false;
      },
      error: (err) => {
        console.error('Errore nel recupero tenants:', err);
        this.isLoading = false;
      }
    });
  }

  // 👑 IL METODO MAGICO PER L'IMPERSONATION CON IL BANNER REATTIVO
  entraComeCliente(tenant: any) {
    this.impersonationService.startImpersonation(tenant);
  }
// ⚙️ RIEMPIAMO IL METODO APRI MODIFICA
  apriModifica(tenant: any) {
    this.editingTenantId = tenant.id;

    // Riempiamo il form con i dati attuali del ristorante
    this.editForm.patchValue({
      nomeRistorante: tenant.nome,
      partitaIva: tenant.partitaIva,
      isActive: tenant.isActive
    });

    this.isModalOpen = true; // Mostra la modale
  }

  // ❌ CHIUDI MODALE
  chiudiModale() {
    this.isModalOpen = false;
    this.editingTenantId = null;
    this.editForm.reset();
  }
  // 🗑️ APRI MODALE ELIMINAZIONE
  apriElimina(tenant: any) {
    this.tenantToDelete = tenant;
    this.confermaNomeCtrl.reset();
    this.isDeleteModalOpen = true;
  }

  // ❌ CHIUDI MODALE ELIMINAZIONE
  chiudiElimina() {
    this.isDeleteModalOpen = false;
    this.tenantToDelete = null;
  }

  // 🔥 ESEGUI L'ELIMINAZIONE EFFETTIVA
  confermaEliminazione() {
    // Ultimo check di sicurezza: i nomi devono coincidere esattamente
    if (this.confermaNomeCtrl.value !== this.tenantToDelete.nome) {
      return;
    }

    this.superAdminService.deleteTenant(this.tenantToDelete.id).subscribe({
      next: () => {
        this.chiudiElimina();
        this.caricaClienti(); // Ricarica la tabella, il ristorante sarà sparito
      },
      error: (err) => console.error('Errore durante eliminazione:', err)
    });
  }

  // 💾 SALVA LE MODIFICHE
  salvaModifiche() {
    if (this.editForm.invalid || !this.editingTenantId) return;

    const formData = this.editForm.value;

    this.superAdminService.updateTenant(this.editingTenantId, formData).subscribe({
      next: () => {
        this.chiudiModale();
        this.caricaClienti(); // Ricarica la tabella in automatico per vedere i nuovi dati!
      },
      error: (err) => console.error('Errore aggiornamento:', err)
    });
  }
}
