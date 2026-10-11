import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { SuperAdminService } from '../../../core/services/super-admin.service';

@Component({
  selector: 'app-tenant-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './tenant-form.component.html',
  styleUrl: './tenant-form.component.scss'
})
export class TenantFormComponent {
  private fb = inject(FormBuilder);
  private superAdminService = inject(SuperAdminService);
  private router = inject(Router);

  // Stato dello Step corrente (1, 2, 3) o 4 (Completato con successo)
  currentStep = signal<number>(1);
  isSubmitting = signal<boolean>(false);
  errorMessage = signal<string | null>(null);
  provisionResult = signal<any | null>(null);

  // Palette colori primari predefiniti per il brand
  colorPresets = [
    { name: 'Ambra Gold', hex: '#F59E0B', bgClass: 'bg-amber-500' },
    { name: 'Smeraldo', hex: '#10B981', bgClass: 'bg-emerald-500' },
    { name: 'Oceano', hex: '#3B82F6', bgClass: 'bg-blue-500' },
    { name: 'Viola Royal', hex: '#8B5CF6', bgClass: 'bg-purple-500' },
    { name: 'Rubino', hex: '#EF4444', bgClass: 'bg-red-500' }
  ];

  // Temi layout vetrina
  layoutThemes = [
    { id: 'tema-liquid-glass', name: 'Liquid Glass (Dark)', desc: 'Interfaccia scura, sfocature moderne e contrasti vibranti' },
    { id: 'tema-minimal-light', name: 'Minimal Modern', desc: 'Stile pulito ad alta leggibilità ideale per menu diurni' }
  ];

  // Form reattivo completo con tutti i campi del wizard
  tenantForm: FormGroup = this.fb.group({
    // Step 1: Identità & Brand
    nomeRistorante: ['', [Validators.required, Validators.minLength(3)]],
    partitaIva: ['', [Validators.required, Validators.pattern(/^[0-9]{11}$/)]],
    dominioPersonalizzato: [''],
    colorePrimario: ['#F59E0B', Validators.required],
    temaLayout: ['tema-liquid-glass', Validators.required],

    // Step 2: Accesso Manager (Root User)
    nomeManager: ['', [Validators.required, Validators.minLength(2)]],
    emailManager: ['', [Validators.required, Validators.email]],
    passwordManager: ['Admin123!', [Validators.required, Validators.minLength(6)]],

    // Step 3: Moduli & Impostazioni
    isCoverChargeEnabled: [true],
    coverChargePrice: [2.00, [Validators.min(0)]],
    timezone: ['Europe/Rome', Validators.required]
  });

  // Validazioni per Step
  isStep1Valid(): boolean {
    const nome = this.tenantForm.get('nomeRistorante');
    const piva = this.tenantForm.get('partitaIva');
    return !!(nome?.valid && piva?.valid);
  }

  isStep2Valid(): boolean {
    const nomeM = this.tenantForm.get('nomeManager');
    const emailM = this.tenantForm.get('emailManager');
    const passM = this.tenantForm.get('passwordManager');
    return !!(nomeM?.valid && emailM?.valid && passM?.valid);
  }

  nextStep(): void {
    this.errorMessage.set(null);
    if (this.currentStep() === 1) {
      if (!this.isStep1Valid()) {
        this.tenantForm.get('nomeRistorante')?.markAsTouched();
        this.tenantForm.get('partitaIva')?.markAsTouched();
        return;
      }
      this.currentStep.set(2);
    } else if (this.currentStep() === 2) {
      if (!this.isStep2Valid()) {
        this.tenantForm.get('nomeManager')?.markAsTouched();
        this.tenantForm.get('emailManager')?.markAsTouched();
        this.tenantForm.get('passwordManager')?.markAsTouched();
        return;
      }
      this.currentStep.set(3);
    }
  }

  prevStep(): void {
    this.errorMessage.set(null);
    if (this.currentStep() > 1) {
      this.currentStep.update(s => s - 1);
    }
  }

  goToStep(step: number): void {
    if (step === 1) {
      this.currentStep.set(1);
    } else if (step === 2 && this.isStep1Valid()) {
      this.currentStep.set(2);
    } else if (step === 3 && this.isStep1Valid() && this.isStep2Valid()) {
      this.currentStep.set(3);
    }
  }

  selectColor(hex: string): void {
    this.tenantForm.patchValue({ colorePrimario: hex });
  }

  selectTheme(themeId: string): void {
    this.tenantForm.patchValue({ temaLayout: themeId });
  }

  onSubmit(): void {
    if (this.tenantForm.invalid) {
      this.tenantForm.markAllAsTouched();
      return;
    }

    this.isSubmitting.set(true);
    this.errorMessage.set(null);

    this.superAdminService.provisionTenant(this.tenantForm.value).subscribe({
      next: (response) => {
        this.isSubmitting.set(false);
        this.provisionResult.set(response);
        this.currentStep.set(4); // Step di successo!
      },
      error: (err) => {
        this.isSubmitting.set(false);
        const backendMsg = err.error?.message || err.error?.error || 'Errore durante la creazione del tenant.';
        this.errorMessage.set(backendMsg);
      }
    });
  }

  vaiAllaLista(): void {
    this.router.navigate(['/god-mode/lista-clienti']);
  }

  creaAltro(): void {
    this.tenantForm.reset({
      colorePrimario: '#F59E0B',
      temaLayout: 'tema-liquid-glass',
      passwordManager: 'Admin123!',
      isCoverChargeEnabled: true,
      coverChargePrice: 2.00,
      timezone: 'Europe/Rome'
    });
    this.provisionResult.set(null);
    this.currentStep.set(1);
  }
}
