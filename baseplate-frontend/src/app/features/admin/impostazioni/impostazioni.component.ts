import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ImpostazioniService } from '../../../core/services/impostazioni.service';
import { ElementoBase } from '../../../core/models/prodotto.model';

@Component({
  selector: 'app-impostazioni',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="grid grid-cols-1 lg:grid-cols-3 gap-6">

      <!-- COLONNA 1: CATEGORIE -->
      <div class="bg-white rounded-xl shadow-sm border border-gray-200 p-6 flex flex-col h-[70vh]">
        <h2 class="text-xl font-bold text-gray-800 mb-4 border-b pb-2">Categorie Pizze</h2>

        <!-- Input per aggiungere -->
        <div class="flex gap-2 mb-4">
          <input #inputCat type="text" placeholder="Es. Pizze Speciali" (keyup.enter)="aggiungi('categorie', inputCat.value); inputCat.value=''" class="flex-1 px-3 py-2 border border-gray-300 rounded-lg text-sm focus:ring-amber-500 focus:border-amber-500">
          <button (click)="aggiungi('categorie', inputCat.value); inputCat.value=''" class="bg-amber-500 hover:bg-amber-600 text-white px-3 py-2 rounded-lg font-bold text-sm transition-colors">+</button>
        </div>

        <!-- Lista (scrollabile se troppi) -->
        <div class="flex-1 overflow-y-auto space-y-2 pr-2">
          <div *ngFor="let item of categorie()" class="flex justify-between items-center bg-gray-50 p-3 rounded-lg border border-gray-100 group">
            <span class="font-medium text-gray-700">{{ item.nome }}</span>
            <div class="flex gap-2 opacity-0 group-hover:opacity-100 transition-opacity">
              <button (click)="modifica('categorie', item)" class="text-blue-500 hover:text-blue-700"><svg class="w-4 h-4" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="2" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" d="M16.862 4.487l1.687-1.688a1.875 1.875 0 112.652 2.652L6.832 19.82a4.5 4.5 0 01-1.897 1.13l-2.685.8.8-2.685a4.5 4.5 0 011.13-1.897L16.863 4.487zm0 0L19.5 7.125" /></svg></button>
              <button (click)="elimina('categorie', item.id)" class="text-red-500 hover:text-red-700"><svg class="w-4 h-4" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="2" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" d="M6 18L18 6M6 6l12 12" /></svg></button>
            </div>
          </div>
        </div>
      </div>

      <!-- COLONNA 2: INGREDIENTI -->
      <div class="bg-white rounded-xl shadow-sm border border-gray-200 p-6 flex flex-col h-[70vh]">
        <h2 class="text-xl font-bold text-gray-800 mb-4 border-b pb-2">Ingredienti</h2>

        <div class="flex gap-2 mb-4">
          <input #inputIng type="text" placeholder="Es. Salame Piccante" (keyup.enter)="aggiungi('ingredienti', inputIng.value); inputIng.value=''" class="flex-1 px-3 py-2 border border-gray-300 rounded-lg text-sm focus:ring-amber-500 focus:border-amber-500">
          <button (click)="aggiungi('ingredienti', inputIng.value); inputIng.value=''" class="bg-amber-500 hover:bg-amber-600 text-white px-3 py-2 rounded-lg font-bold text-sm transition-colors">+</button>
        </div>

        <div class="flex-1 overflow-y-auto space-y-2 pr-2">
          <div *ngFor="let item of ingredienti()" class="flex justify-between items-center bg-gray-50 p-3 rounded-lg border border-gray-100 group">
            <span class="font-medium text-gray-700">{{ item.nome }}</span>
            <div class="flex gap-2 opacity-0 group-hover:opacity-100 transition-opacity">
              <button (click)="modifica('ingredienti', item)" class="text-blue-500 hover:text-blue-700"><svg class="w-4 h-4" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="2" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" d="M16.862 4.487l1.687-1.688a1.875 1.875 0 112.652 2.652L6.832 19.82a4.5 4.5 0 01-1.897 1.13l-2.685.8.8-2.685a4.5 4.5 0 011.13-1.897L16.863 4.487zm0 0L19.5 7.125" /></svg></button>
              <button (click)="elimina('ingredienti', item.id)" class="text-red-500 hover:text-red-700"><svg class="w-4 h-4" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="2" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" d="M6 18L18 6M6 6l12 12" /></svg></button>
            </div>
          </div>
        </div>
      </div>

      <!-- COLONNA 3: ALLERGENI -->
      <div class="bg-white rounded-xl shadow-sm border border-gray-200 p-6 flex flex-col h-[70vh]">
        <h2 class="text-xl font-bold text-red-600 mb-4 border-b pb-2">Allergeni</h2>

        <div class="flex gap-2 mb-4">
          <input #inputAll type="text" placeholder="Es. Glutine" (keyup.enter)="aggiungi('allergeni', inputAll.value); inputAll.value=''" class="flex-1 px-3 py-2 border border-gray-300 rounded-lg text-sm focus:ring-amber-500 focus:border-amber-500">
          <button (click)="aggiungi('allergeni', inputAll.value); inputAll.value=''" class="bg-amber-500 hover:bg-amber-600 text-white px-3 py-2 rounded-lg font-bold text-sm transition-colors">+</button>
        </div>

        <div class="flex-1 overflow-y-auto space-y-2 pr-2">
          <div *ngFor="let item of allergeni()" class="flex justify-between items-center bg-red-50 p-3 rounded-lg border border-red-100 group">
            <span class="font-medium text-red-700">{{ item.nome }}</span>
            <div class="flex gap-2 opacity-0 group-hover:opacity-100 transition-opacity">
              <button (click)="modifica('allergeni', item)" class="text-blue-500 hover:text-blue-700"><svg class="w-4 h-4" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="2" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" d="M16.862 4.487l1.687-1.688a1.875 1.875 0 112.652 2.652L6.832 19.82a4.5 4.5 0 01-1.897 1.13l-2.685.8.8-2.685a4.5 4.5 0 011.13-1.897L16.863 4.487zm0 0L19.5 7.125" /></svg></button>
              <button (click)="elimina('allergeni', item.id)" class="text-red-500 hover:text-red-700"><svg class="w-4 h-4" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="2" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" d="M6 18L18 6M6 6l12 12" /></svg></button>
            </div>
          </div>
        </div>
      </div>

    </div>
  `
})
export class ImpostazioniComponent implements OnInit {
  private service = inject(ImpostazioniService);

  categorie = signal<ElementoBase[]>([]);
  ingredienti = signal<ElementoBase[]>([]);
  allergeni = signal<ElementoBase[]>([]);

  ngOnInit() {
    this.caricaTutti();
  }

  caricaTutti() {
    this.service.getTutti('categorie').subscribe(d => this.categorie.set(d));
    this.service.getTutti('ingredienti').subscribe(d => this.ingredienti.set(d));
    this.service.getTutti('allergeni').subscribe(d => this.allergeni.set(d));
  }

  aggiungi(endpoint: string, nome: string) {
    if (!nome.trim()) return;
    this.service.crea(endpoint, nome).subscribe(() => this.caricaTutti());
  }

  modifica(endpoint: string, item: ElementoBase) {
    const nuovoNome = prompt(`Modifica nome (${endpoint}):`, item.nome);
    if (nuovoNome && nuovoNome.trim() !== item.nome) {
      this.service.modifica(endpoint, item.id, nuovoNome).subscribe(() => this.caricaTutti());
    }
  }

  elimina(endpoint: string, id: number) {
    if (confirm(`Sicuro di voler eliminare questo elemento?`)) {
      this.service.elimina(endpoint, id).subscribe(() => this.caricaTutti());
    }
  }
}
