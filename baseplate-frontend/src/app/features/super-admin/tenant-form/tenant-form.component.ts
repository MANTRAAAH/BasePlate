import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { SuperAdminService } from '../../../core/services/super-admin.service';

@Component({
  selector: 'app-tenant-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './tenant-form.component.html',
  styleUrl: './tenant-form.component.scss' // 👈 QUESTA E' LA RIGA MAGICA
})
export class TenantFormComponent {
  private fb = inject(FormBuilder);
  private superAdminService = inject(SuperAdminService);

  // Definizione del form con regole di validazione
  tenantForm: FormGroup = this.fb.group({
    nomeRistorante: ['', [Validators.required, Validators.minLength(3)]],
    partitaIva: ['', [Validators.required, Validators.pattern(/^[0-9]{11}$/)]], // Solo 11 numeri
    emailManager: ['', [Validators.required, Validators.email]]
  });

  isSubmitting = false;

  onSubmit() {
    // Blocca l'invio se il form non è valido
    if (this.tenantForm.invalid) {
      this.tenantForm.markAllAsTouched();
      return;
    }

    this.isSubmitting = true;

    // Chiama il servizio che abbiamo abbozzato prima
    this.superAdminService.provisionTenant(this.tenantForm.value).subscribe({
      next: (response) => {
        // In produzione useremo un Toast/Snackbar, per ora un alert va benissimo
        alert(`🚀 Provisioning completato!\nNome: ${response.nome}\nTenantId: ${response.tenantId}`);
        this.tenantForm.reset();
        this.isSubmitting = false;
      },
      error: (err) => {
        console.error('Errore dal backend:', err);
        alert('Errore durante la creazione. Controlla la console.');
        this.isSubmitting = false;
      }
    });
  }
}
