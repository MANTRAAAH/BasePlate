import { Component, inject, EventEmitter, Output, OnInit, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ProdottiService } from '../../../core/services/prodotti.service';
import { ElementoBase, LookupDati } from '../../../core/models/prodotto.model';

@Component({
  selector: 'app-crea-prodotto',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  template: `
    <div class="bg-white p-8 rounded-xl shadow-sm border border-gray-200">
      <div class="flex justify-between items-center mb-6">
        <h2 class="text-2xl font-bold text-gray-800">{{ prodottoId ? 'Modifica Pizza' : 'Aggiungi Nuova Pizza' }}</h2>
        <button type="button" (click)="annulla.emit()" class="text-gray-400 hover:text-gray-600">
          <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor" class="w-6 h-6"><path stroke-linecap="round" stroke-linejoin="round" d="M6 18L18 6M6 6l12 12" /></svg>
        </button>
      </div>

      <form [formGroup]="prodottoForm" (ngSubmit)="salvaProdotto()" class="space-y-6">

        <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
          <div>
            <label class="block text-sm font-medium text-gray-700 mb-1">Nome Prodotto *</label>
            <input type="text" formControlName="nome"
                   class="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-amber-500 focus:border-amber-500"
                   placeholder="Es. Diavola">
          </div>

          <div>
            <label class="block text-sm font-medium text-gray-700 mb-1">Prezzo (€) *</label>
            <input type="number" step="0.50" formControlName="prezzo"
                   class="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-amber-500 focus:border-amber-500"
                   placeholder="0.00">
          </div>
        </div>

        <div>
          <label class="block text-sm font-medium text-gray-700 mb-1">Descrizione</label>
          <textarea formControlName="descrizione" rows="2"
                    class="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-amber-500 focus:border-amber-500"
                    placeholder="Breve descrizione del piatto..."></textarea>
        </div>

        <div>
          <label class="block text-sm font-medium text-gray-700 mb-1">URL Immagine</label>
          <input type="text" formControlName="immagineUrl"
                 class="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-amber-500 focus:border-amber-500"
                 placeholder="https://...">
        </div>

        <div class="grid grid-cols-1 md:grid-cols-3 gap-6 bg-gray-50 p-4 rounded-lg border border-gray-100">

          <div>
            <label class="block text-sm font-semibold text-gray-700 mb-2">Categoria</label>
            <select formControlName="categoriaId" class="w-full border-gray-300 rounded-md py-2 px-3 text-sm">
              <option *ngFor="let cat of categorie" [value]="cat.id">{{ cat.nome }}</option>
            </select>
          </div>

          <div>
            <label class="block text-sm font-semibold text-gray-700 mb-2">Ingredienti Base</label>
            <div class="space-y-2">
              <label *ngFor="let ing of ingredienti" class="flex items-center gap-2 text-sm text-gray-600">
                <input type="checkbox"
                       [value]="ing.id"
                       [checked]="prodottoForm.get('ingredientiIds')?.value?.includes(ing.id)"
                       (change)="toggleArray('ingredientiIds', ing.id, $event)"
                       class="rounded text-amber-500 focus:ring-amber-500">
                {{ ing.nome }}
              </label>
            </div>
          </div>

          <div>
            <label class="block text-sm font-semibold text-gray-700 mb-2">Allergeni</label>
            <div class="space-y-2">
              <label *ngFor="let all of allergeni" class="flex items-center gap-2 text-sm text-red-600">
                <input type="checkbox"
                       [value]="all.id"
                       [checked]="prodottoForm.get('allergeniIds')?.value?.includes(all.id)"
                       (change)="toggleArray('allergeniIds', all.id, $event)"
                       class="rounded border-red-300 text-red-500 focus:ring-red-500">
                {{ all.nome }}
              </label>
            </div>
          </div>

        </div>

        <div class="flex justify-end gap-3 pt-4 border-t border-gray-100">
          <button type="button" (click)="annulla.emit()" class="px-5 py-2 text-sm font-medium text-gray-700 bg-white border border-gray-300 rounded-lg hover:bg-gray-50">
            Annulla
          </button>
          <button type="submit" [disabled]="prodottoForm.invalid" class="px-5 py-2 text-sm font-medium text-white bg-amber-500 rounded-lg hover:bg-amber-600 disabled:opacity-50 disabled:cursor-not-allowed">
            Salva Prodotto
          </button>
        </div>

      </form>
    </div>
  `
})
export class CreaProdottoComponent implements OnInit {
  @Input() prodottoId: number | null = null;
  @Output() annulla = new EventEmitter<void>();
  @Output() salvato = new EventEmitter<void>();

  private fb = inject(FormBuilder);
  private prodottiService = inject(ProdottiService);

  prodottoForm: FormGroup = this.fb.group({
    nome: ['', Validators.required],
    descrizione: [''],
    prezzo: [0, [Validators.required, Validators.min(0)]],
    categoriaId: [null, Validators.required],
    immagineUrl: [''],
    ingredientiIds: [[] as number[]], // Inizializzazione corretta array per TS Strict
    allergeniIds: [[] as number[]]
  });

  categorie: ElementoBase[] = [];
  ingredienti: ElementoBase[] = [];
  allergeni: ElementoBase[] = [];

  ngOnInit() {
    // 1. Carichiamo sempre i dati base (Categorie, Ingredienti, ecc.)
    this.prodottiService.getLookupDati().subscribe({
      next: (dati: LookupDati) => {
        this.categorie = dati.categorie;
        this.ingredienti = dati.ingredienti;
        this.allergeni = dati.allergeni;

        // 2. Se è una MODIFICA, andiamo a scaricare i dati della pizza!
        if (this.prodottoId) {
          this.caricaProdottoDaModificare(this.prodottoId);
        } else if (this.categorie.length > 0) {
          // Se è una creazione nuova, seleziona solo la prima categoria
          this.prodottoForm.patchValue({ categoriaId: this.categorie[0].id });
        }
      }
    });
  }

  // Scarica la pizza e "spalma" i dati nel form!
  caricaProdottoDaModificare(id: number) {
    this.prodottiService.getProdotto(id).subscribe({
      next: (pizza) => {
        this.prodottoForm.patchValue({
          nome: pizza.nome,
          descrizione: pizza.descrizione,
          prezzo: pizza.prezzo,
          categoriaId: pizza.categoriaId,
          immagineUrl: pizza.immagineUrl,
          ingredientiIds: pizza.ingredientiIds,
          allergeniIds: pizza.allergeniIds
        });
      },
      error: (err) => alert("Errore nel caricamento del prodotto")
    });
  }

  toggleArray(controlName: string, id: number, event: Event) {
    const isChecked = (event.target as HTMLInputElement).checked;
    const currentArray = this.prodottoForm.get(controlName)?.value as number[];

    if (isChecked) {
      this.prodottoForm.patchValue({ [controlName]: [...currentArray, id] });
    } else {
      this.prodottoForm.patchValue({ [controlName]: currentArray.filter(x => x !== id) });
    }
  }

  salvaProdotto() {
    if (this.prodottoForm.valid) {
      // BIVIO: Modifica o Creazione?
      if (this.prodottoId) {
        // MODIFICA
        this.prodottiService.aggiornaProdotto(this.prodottoId, this.prodottoForm.value).subscribe({
          next: () => {
            alert('Prodotto aggiornato!');
            this.salvato.emit();
          }
        });
      } else {
        // CREAZIONE
        this.prodottiService.creaProdotto(this.prodottoForm.value).subscribe({
          next: () => {
            alert('Prodotto creato!');
            this.salvato.emit();
          }
        });
      }
    }
  }
}
