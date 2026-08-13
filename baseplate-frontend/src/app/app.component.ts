import { Component } from '@angular/core';
import { ListaProdottiComponent } from './features/lista-prodotti/lista-prodotti.component'; // 👈 Importalo

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [ListaProdottiComponent], // 👈 Aggiungilo agli imports
  template: `<app-lista-prodotti></app-lista-prodotti>`, // 👈 Usalo nel template
})
export class AppComponent { }
