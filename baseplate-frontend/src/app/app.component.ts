import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { ImpersonationBannerComponent } from './core/components/impersonation-banner/impersonation-banner.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, ImpersonationBannerComponent],
  template: `
    <div class="app-layout">
      <app-impersonation-banner></app-impersonation-banner>
      <div class="app-content">
        <router-outlet></router-outlet>
      </div>
    </div>
  `,
  styles: [`
    .app-layout {
      display: flex;
      flex-direction: column;
      min-height: 100vh;
      width: 100%;
    }
    .app-content {
      flex: 1;
      display: flex;
      flex-direction: column;
      overflow: hidden;
    }
  `]
})
export class AppComponent { }

