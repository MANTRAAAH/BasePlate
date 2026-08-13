import { Component, OnInit, inject, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProdottiService } from '../../core/services/prodotti.service';
import { ProdottoReadDto } from '../../core/models/prodotto.model';

@Component({
  selector: 'app-lista-prodotti',
  standalone: true,
  imports: [CommonModule],
  template: `
    <!-- Sfondo scuro globale in stile Ti Delizio -->
    <div class="min-h-screen bg-[#121212] text-neutral-200 font-sans pb-12">

      <!-- Finto Header con Logo (sostituisci l'immagine con il tuo logo reale) -->
      <header class="w-full relative h-32 md:h-48 overflow-hidden flex items-center justify-center border-b border-neutral-800">
        <div class="absolute inset-0 bg-black/60 z-10"></div>
        <!-- Immagine di background della testata -->
        <img src="https://images.unsplash.com/photo-1513104890138-7c749659a591?q=80&w=2000&auto=format&fit=crop" class="absolute inset-0 w-full h-full object-cover" />
        <!-- Logo centrale -->
        <div class="z-20 bg-black w-24 h-24 rounded-full flex items-center justify-center text-white font-bold text-xl border-2 border-neutral-700 shadow-xl">
          LOGO
        </div>
      </header>

      <main class="max-w-4xl mx-auto px-4 py-8">

        <!-- ========================================== -->
        <!-- STATO 1: CAROSELLO CATEGORIE               -->
        <!-- ========================================== -->
        <ng-container *ngIf="!categoriaSelezionata()">

          <div *ngIf="prodotti().length === 0" class="text-center text-neutral-500 py-10 animate-pulse">
            Caricamento menù...
          </div>

          <!-- Contenitore Scroll Orizzontale -->
          <div class="flex gap-4 overflow-x-auto hide-scroll snap-x snap-mandatory py-4">

            <article *ngFor="let cat of categorieUniche()"
                     (click)="selezionaCategoria(cat.nome)"
                     class="snap-center shrink-0 w-64 h-[380px] md:w-72 md:h-[420px] relative rounded-2xl overflow-hidden cursor-pointer group border border-neutral-800">
              <!-- Immagine di sfondo categoria (usa l'immagine del primo prodotto o un placeholder) -->
              <img [src]="cat.immagineUrl || 'https://images.unsplash.com/photo-1604382355076-af4b0eb60143?q=80&w=600&auto=format&fit=crop'"
                   class="absolute inset-0 w-full h-full object-cover group-hover:scale-105 transition-transform duration-700" />

              <!-- Overlay sfumato scuro -->
              <div class="absolute inset-0 bg-gradient-to-b from-black/20 via-black/40 to-black/90"></div>

              <!-- Badge Dettagli -->
              <div class="absolute top-4 left-4">
                <span class="text-xs text-white/80 bg-black/50 backdrop-blur-md px-2 py-1 rounded">Dettagli +</span>
              </div>

              <!-- Titolo Categoria -->
              <div class="absolute bottom-6 left-0 w-full text-center px-4">
                <h2 class="text-2xl font-bold text-white uppercase tracking-widest">{{ cat.nome }}</h2>
              </div>
            </article>

          </div>

          <!-- Indicatori di scorrimento finti sotto il carosello -->
          <div class="flex justify-center gap-2 mt-4" *ngIf="categorieUniche().length > 0">
            <div *ngFor="let c of categorieUniche(); let i = index" class="w-1.5 h-1.5 rounded-full" [ngClass]="i === 0 ? 'bg-white' : 'bg-neutral-600'"></div>
          </div>

        </ng-container>


        <!-- ========================================== -->
        <!-- STATO 2: LISTA PRODOTTI DELLA CATEGORIA    -->
        <!-- ========================================== -->
        <ng-container *ngIf="categoriaSelezionata()">

          <!-- Pulsante Torna Indietro -->
          <button (click)="deselezionaCategoria()" class="flex items-center gap-2 text-sm text-neutral-400 hover:text-white mb-6 transition-colors w-full border-b border-neutral-800 pb-4">
            <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="2" stroke="currentColor" class="w-4 h-4">
              <path stroke-linecap="round" stroke-linejoin="round" d="M9 15L3 9m0 0l6-6M3 9h12a6 6 0 010 12h-3" />
            </svg>
            Torna indietro
          </button>

          <!-- Intestazione Lista -->
          <div class="flex justify-between items-end mb-6">
            <div>
              <span class="text-xs text-neutral-500 font-semibold tracking-widest">MENU /</span>
              <h2 class="text-2xl font-bold text-white">{{ categoriaSelezionata() }}</h2>
            </div>
            <!-- Finto pulsante Allergeni (come nel video) -->
            <button class="flex items-center gap-1 text-xs text-neutral-400 border border-neutral-700 px-2 py-1 rounded-full">
              <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor" class="w-3 h-3"><path stroke-linecap="round" stroke-linejoin="round" d="M11.25 11.25l.041-.02a.75.75 0 011.063.852l-.708 2.836a.75.75 0 001.063.853l.041-.021M21 12a9 9 0 11-18 0 9 9 0 0118 0zm-9-3.75h.008v.008H12V8.25z" /></svg>
              Allergeni
            </button>
          </div>

          <!-- Prodotti Orizzontali -->
          <div class="flex flex-col gap-4">
            <article *ngFor="let p of prodottiFiltrati()" class="bg-[#1a1a1a] border border-neutral-800 rounded-xl overflow-hidden flex items-stretch">

              <!-- Immagine Prodotto quadrata a sinistra -->
              <div class="w-24 md:w-32 relative shrink-0">
                <img [src]="p.immagineUrl || 'https://images.unsplash.com/photo-1574071318508-1cdbab80d002?q=80&w=300&auto=format&fit=crop'" class="w-full h-full object-cover" />
                <div class="absolute top-1 left-1">
                  <span class="text-[9px] text-white/90 bg-black/60 px-1.5 py-0.5 rounded">Dettagli +</span>
                </div>
              </div>

              <!-- Testo Prodotto a destra -->
              <div class="p-3 flex flex-col justify-between grow">
                <div>
                  <h3 class="font-bold text-white text-sm md:text-base leading-tight">{{ p.nome }}</h3>
                  <p class="text-xs text-neutral-400 mt-1 line-clamp-2 leading-relaxed">
                    {{ p.descrizione }}
                    <span *ngIf="p.ingredienti.length > 0"> ({{ p.ingredienti.join(', ') }})</span>
                  </p>

                  <!-- Badge Allergeni minimali -->
                  <div class="flex gap-1 mt-1.5" *ngIf="p.allergeni.length > 0">
                     <span *ngFor="let all of p.allergeni" class="text-[10px] text-amber-500 uppercase">{{ all }}</span>
                  </div>
                </div>

                <!-- Prezzo allineato a destra in basso -->
                <div class="text-right mt-2">
                  <span class="font-bold text-white">{{ p.prezzo | number:'1.2-2' }} €</span>
                </div>
              </div>

            </article>
          </div>

        </ng-container>

      </main>

      <!-- Footer scuro -->
      <footer class="max-w-4xl mx-auto px-4 mt-12 border-t border-neutral-800 pt-8 pb-12 flex flex-col md:flex-row items-center gap-6 text-neutral-400 text-sm">
        <div class="w-16 h-16 rounded-full bg-black border border-neutral-700 flex items-center justify-center text-white shrink-0">LOGO</div>
        <div>
          <h4 class="font-bold text-white mb-1">IL TUO LOCALE</h4>
          <p>P.IVA: 0123456789</p>
          <p class="mt-2 text-white">Orari: Lun-Dom 19:00-24:00</p>
          <p>Martedì: chiuso</p>
        </div>
      </footer>

    </div>
  `,
  styles: [`
    .hide-scroll::-webkit-scrollbar {
      display: none;
    }
    .hide-scroll {
      -ms-overflow-style: none;
      scrollbar-width: none;
    }
    /* Classe per tagliare il testo su più righe se troppo lungo */
    .line-clamp-2 {
      display: -webkit-box;
      -webkit-line-clamp: 2;
      -webkit-box-orient: vertical;
      overflow: hidden;
    }
  `]
})
export class ListaProdottiComponent implements OnInit {
  private prodottiService = inject(ProdottiService);

  // Tutti i prodotti dall'API
  prodotti = signal<ProdottoReadDto[]>([]);

  // Lo stato della navigazione (null = mostra carosello, stringa = mostra lista)
  categoriaSelezionata = signal<string | null>(null);

  // Deriviamo dinamicamente le categorie dai prodotti! (Magia dei Signals)
  categorieUniche = computed(() => {
    const list = this.prodotti();
    const mappa = new Map<string, string>(); // NomeCategoria -> Immagine

    list.forEach(p => {
      if (!mappa.has(p.nomeCategoria)) {
        // Assegniamo alla categoria l'immagine del suo primo prodotto (se ce l'ha)
        mappa.set(p.nomeCategoria, p.immagineUrl || '');
      }
    });

    return Array.from(mappa, ([nome, immagineUrl]) => ({ nome, immagineUrl }));
  });

  // Filtra dinamicamente i prodotti in base alla categoria cliccata
  prodottiFiltrati = computed(() => {
    const cat = this.categoriaSelezionata();
    if (!cat) return [];
    return this.prodotti().filter(p => p.nomeCategoria === cat);
  });

  ngOnInit(): void {
    this.prodottiService.getProdotti().subscribe({
      next: (dati) => this.prodotti.set(dati),
      error: (err) => console.error('Errore API:', err)
    });
  }

  selezionaCategoria(nome: string) {
    this.categoriaSelezionata.set(nome);
    // Scorri in alto quando si cambia vista
    window.scrollTo({ top: 0, behavior: 'smooth' });
  }

  deselezionaCategoria() {
    this.categoriaSelezionata.set(null);
  }
}
