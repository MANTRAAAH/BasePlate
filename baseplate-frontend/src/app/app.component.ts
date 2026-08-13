import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router'; // 👈 Importiamo il RouterOutlet

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet], // 👈 Mettiamo solo RouterOutlet negli imports
  template: `
    <!-- Questo è il "buco" magico.
         Angular ci inietterà dentro il componente giusto in base all'URL -->
    <router-outlet></router-outlet>
  `,
})
export class AppComponent { }
