import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CreaProdottoComponent } from '../crea-prodotto/crea-prodotto.component';
import { ProdottiService } from '../../../core/services/prodotti.service';
import { ProdottoReadDto } from '../../../core/models/prodotto.model';

@Component({
  selector: 'app-dashboard-admin',
  standalone: true,
  imports: [CommonModule, CreaProdottoComponent],
  template: `
    <div class="flex h-screen bg-gray-50 font-sans text-gray-900 overflow-hidden">

      <!-- SIDEBAR -->
      <aside class="w-64 bg-slate-900 text-slate-300 transition-all duration-300 hidden md:flex flex-col">
         <div class="h-16 flex items-center px-6 bg-slate-950 border-b border-slate-800">
          <span class="text-white font-black text-xl tracking-wider">Base<span class="text-amber-500">Plate</span></span>
        </div>

        <nav class="flex-1 px-4 py-6 space-y-2 overflow-y-auto">
          <a href="#" class="flex items-center gap-3 px-4 py-3 bg-amber-500/10 text-amber-500 rounded-lg transition-colors">
            <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="2" stroke="currentColor" class="w-5 h-5"><path stroke-linecap="round" stroke-linejoin="round" d="M12 6v12m-3-2.818l.879.659c1.171.879 3.07.879 4.242 0 1.172-.879 1.172-2.303 0-3.182C13.536 12.219 12.768 12 12 12c-.725 0-1.45-.22-2.003-.659-1.106-.879-1.106-2.303 0-3.182s2.9-.879 4.006 0l.415.33M21 12a9 9 0 11-18 0 9 9 0 0118 0z" /></svg>
            <span class="font-semibold">Il mio Menù</span>
          </a>
          <!-- Altre voci menu omesse per brevità... -->
        </nav>
      </aside>

      <!-- MAIN CONTENT -->
      <div class="flex-1 flex flex-col h-screen overflow-hidden">

        <header class="h-16 bg-white border-b border-gray-200 flex items-center justify-between px-6 shadow-sm z-10">
          <h1 class="text-xl font-bold text-gray-800">Gestione Menù</h1>

          <button *ngIf="!mostraForm()" (click)="apriForm()" class="bg-amber-500 hover:bg-amber-600 text-white px-4 py-2 rounded-lg font-semibold text-sm shadow-sm transition-colors flex items-center gap-2">
            Nuovo Prodotto
          </button>
        </header>

        <main class="flex-1 overflow-x-hidden overflow-y-auto bg-gray-50 p-6">

          <!-- FORM DI CREAZIONE (Nascosto di default) -->
<app-crea-prodotto
  *ngIf="mostraForm()"
  [prodottoId]="prodottoInModifica()"
  (annulla)="chiudiForm()"
  (salvato)="prodottoSalvato()">
</app-crea-prodotto>

          <!-- TABELLA PRODOTTI (Visibile di default) -->
          <div *ngIf="!mostraForm()" class="bg-white border border-gray-200 rounded-xl shadow-sm overflow-hidden">

            <table class="w-full text-left border-collapse">
              <thead>
                <tr class="bg-gray-50 border-b border-gray-200 text-sm text-gray-500">
                  <th class="px-6 py-4 font-semibold">Prodotto</th>
                  <th class="px-6 py-4 font-semibold">Categoria</th>
                  <th class="px-6 py-4 font-semibold">Prezzo</th>
                  <th class="px-6 py-4 font-semibold text-right">Azioni</th>
                </tr>
              </thead>
              <tbody class="divide-y divide-gray-100">

                <!-- Stato di caricamento o lista vuota -->
                <tr *ngIf="prodotti().length === 0">
                  <td colspan="4" class="px-6 py-8 text-center text-gray-500">
                    Nessun prodotto trovato. Inizia ad aggiungerne uno!
                  </td>
                </tr>

                <!-- Ciclo sui prodotti reali -->
                <tr *ngFor="let p of prodotti()" class="hover:bg-gray-50 transition-colors group">

                  <td class="px-6 py-4">
                    <div class="flex items-center gap-4">
                      <!-- Immagine in miniatura -->
                      <div class="w-12 h-12 rounded-lg bg-gray-200 overflow-hidden shrink-0">
                        <img *ngIf="p.immagineUrl" [src]="p.immagineUrl" class="w-full h-full object-cover">
                      </div>
                      <div>
                        <div class="font-bold text-gray-900">{{ p.nome }}</div>
                        <div class="text-xs text-gray-500 truncate w-48" [title]="p.descrizione">{{ p.descrizione }}</div>
                      </div>
                    </div>
                  </td>

                  <td class="px-6 py-4">
                    <span class="bg-gray-100 text-gray-600 px-2.5 py-1 rounded-md text-xs font-medium">
                      {{ p.nomeCategoria }}
                    </span>
                  </td>

                  <td class="px-6 py-4 font-bold text-gray-900">
                    €{{ p.prezzo | number:'1.2-2' }}
                  </td>

                  <td class="px-6 py-4 text-right">
  <div class="flex items-center justify-end gap-2 opacity-0 group-hover:opacity-100 transition-opacity">

    <!-- Pulsante Modifica CHIUSO CORRETTAMENTE CON LA SUA ICONA -->
    <button (click)="modifica(p.id)" class="p-2 text-blue-600 hover:bg-blue-50 rounded-lg transition-colors" title="Modifica">
      <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="2" stroke="currentColor" class="w-4 h-4"><path stroke-linecap="round" stroke-linejoin="round" d="M16.862 4.487l1.687-1.688a1.875 1.875 0 112.652 2.652L6.832 19.82a4.5 4.5 0 01-1.897 1.13l-2.685.8.8-2.685a4.5 4.5 0 011.13-1.897L16.863 4.487zm0 0L19.5 7.125" /></svg>
    </button>

    <!-- Pulsante Elimina -->
    <button (click)="elimina(p.id, p.nome)" class="p-2 text-red-600 hover:bg-red-50 rounded-lg transition-colors" title="Elimina">
      <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="2" stroke="currentColor" class="w-4 h-4"><path stroke-linecap="round" stroke-linejoin="round" d="M14.74 9l-.346 9m-4.788 0L9.26 9m9.968-3.21c.342.052.682.107 1.022.166m-1.022-.165L18.16 19.673a2.25 2.25 0 01-2.244 2.077H8.084a2.25 2.25 0 01-2.244-2.077L4.772 5.79m14.456 0a48.108 48.108 0 00-3.478-.397m-12 .562c.34-.059.68-.114 1.022-.165m0 0a48.11 48.11 0 013.478-.397m7.5 0v-.916c0-1.18-.91-2.164-2.09-2.201a51.964 51.964 0 00-3.32 0c-1.18.037-2.09 1.022-2.09 2.201v.916m7.5 0a48.667 48.667 0 00-7.5 0" /></svg>
    </button>

  </div>
</td>

                </tr>
              </tbody>
            </table>
          </div>

        </main>
      </div>
    </div>
  `
})
export class DashboardAdminComponent implements OnInit {
  private prodottiService = inject(ProdottiService);

  mostraForm = signal(false);
  prodotti = signal<ProdottoReadDto[]>([]); // 👈 Qui teniamo in memoria le pizze!
  prodottoInModifica = signal<number | null>(null);

  ngOnInit() {
    this.caricaProdotti(); // Appena si apre la pagina, carichiamo le pizze dal backend
  }

  caricaProdotti() {
    this.prodottiService.getProdotti().subscribe({
      next: (dati) => this.prodotti.set(dati),
      error: (err) => console.error('Errore API', err)
    });
  }

apriForm() {
    this.prodottoInModifica.set(null); // Nuovo prodotto: azzeriamo l'ID
    this.mostraForm.set(true);
  }

  modifica(id: number) {
    this.prodottoInModifica.set(id); // Diciamo al form quale ID caricare
    this.mostraForm.set(true);
  }

  chiudiForm() {
    this.mostraForm.set(false);
    this.prodottoInModifica.set(null);
  }
  prodottoSalvato() {
    this.chiudiForm();
    this.caricaProdotti(); // 👈 MAGIA! Quando salvi una pizza, aggiorniamo la tabella automaticamente!
  }
  elimina(id: number, nome: string) {
    // Chiediamo conferma per evitare click per sbaglio!
    if (confirm(`Sei sicuro di voler eliminare la pizza "${nome}"?`)) {
      this.prodottiService.eliminaProdotto(id).subscribe({
        next: () => {
          // Se il backend ci dice OK, ricarichiamo la tabella!
          this.caricaProdotti();
        },
        error: (err) => alert('Errore durante l\'eliminazione')
      });
    }
  }
}
