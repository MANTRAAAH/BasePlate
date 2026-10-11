import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ImpersonationService } from '../../services/impersonation.service';

@Component({
  selector: 'app-impersonation-banner',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div *ngIf="impersonationService.isImpersonating()" class="impersonation-banner">
      <div class="banner-content">
        <!-- ICONA E TITOLO -->
        <div class="badge-group">
          <div class="pulse-indicator">
            <span class="dot"></span>
          </div>
          <span class="badge-label">🎭 MODALITÀ IMPERSONATION</span>
        </div>

        <div class="separator">|</div>

        <!-- DETTAGLIO TENANT -->
        <div class="tenant-details">
          <span class="tenant-prefix">Stai operando come:</span>
          <span class="tenant-name">{{ impersonationService.currentTenantName() || 'Tenant Selezionato' }}</span>
          <span class="tenant-id-pill" [title]="impersonationService.currentTenantId()">
            ID: {{ (impersonationService.currentTenantId() || '').slice(0, 8) }}...
          </span>
        </div>

        <!-- ACTION BUTTON -->
        <button class="btn-exit" (click)="esci()" title="Termina impersonation e torna al SuperAdmin">
          <svg class="exit-icon" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
            <path d="M9 21H5a2 2 0 0 1-2-2V5a2 2 0 0 1 2-2h4" stroke-linecap="round" stroke-linejoin="round"/>
            <polyline points="16 17 21 12 16 7" stroke-linecap="round" stroke-linejoin="round"/>
            <line x1="21" y1="12" x2="9" y2="12" stroke-linecap="round" stroke-linejoin="round"/>
          </svg>
          <span>Esci & Torna a God Mode 👑</span>
        </button>
      </div>
    </div>
  `,
  styles: [`
    .impersonation-banner {
      width: 100%;
      background: linear-gradient(90deg, #78350f 0%, #b45309 30%, #d97706 70%, #78350f 100%);
      color: #ffffff;
      border-bottom: 1px solid rgba(251, 191, 36, 0.4);
      box-shadow: 0 4px 16px rgba(0, 0, 0, 0.35);
      z-index: 9999;
      position: sticky;
      top: 0;
      flex-shrink: 0;
      font-family: system-ui, -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif;
    }

    .banner-content {
      max-width: 1400px;
      margin: 0 auto;
      padding: 9px 24px;
      display: flex;
      align-items: center;
      justify-content: space-between;
      gap: 16px;
      font-size: 0.88rem;
    }

    .badge-group {
      display: flex;
      align-items: center;
      gap: 8px;
      flex-shrink: 0;
    }

    .pulse-indicator {
      display: flex;
      align-items: center;
      justify-content: center;

      .dot {
        width: 8px;
        height: 8px;
        border-radius: 50%;
        background-color: #fef08a;
        box-shadow: 0 0 8px #fef08a;
        animation: pulse 1.5s infinite;
      }
    }

    .badge-label {
      font-weight: 800;
      letter-spacing: 0.05em;
      font-size: 0.78rem;
      color: #fef08a;
    }

    .separator {
      color: rgba(255, 255, 255, 0.3);
      font-weight: 300;
    }

    .tenant-details {
      display: flex;
      align-items: center;
      gap: 8px;
      flex: 1;
      overflow: hidden;
      white-space: nowrap;
      text-overflow: ellipsis;

      .tenant-prefix {
        color: rgba(255, 255, 255, 0.85);
        font-size: 0.84rem;
      }

      .tenant-name {
        font-weight: 700;
        color: #ffffff;
        font-size: 0.95rem;
        text-shadow: 0 1px 2px rgba(0, 0, 0, 0.3);
      }

      .tenant-id-pill {
        font-family: monospace;
        font-size: 0.72rem;
        background: rgba(0, 0, 0, 0.25);
        border: 1px solid rgba(255, 255, 255, 0.15);
        padding: 2px 8px;
        border-radius: 9999px;
        color: #fef3c7;
      }
    }

    .btn-exit {
      display: inline-flex;
      align-items: center;
      gap: 8px;
      background: #111827;
      color: #fbbf24;
      border: 1px solid rgba(251, 191, 36, 0.4);
      padding: 6px 14px;
      border-radius: 8px;
      font-size: 0.82rem;
      font-weight: 700;
      cursor: pointer;
      transition: all 0.2s ease;
      white-space: nowrap;
      flex-shrink: 0;
      box-shadow: 0 2px 6px rgba(0, 0, 0, 0.3);

      .exit-icon {
        width: 15px;
        height: 15px;
        stroke: currentColor;
      }

      &:hover {
        background: #030712;
        color: #ffffff;
        border-color: #fbbf24;
        transform: scale(1.03);
        box-shadow: 0 4px 12px rgba(0, 0, 0, 0.5);
      }
    }

    @keyframes pulse {
      0% {
        transform: scale(0.9);
        opacity: 0.7;
      }
      50% {
        transform: scale(1.2);
        opacity: 1;
      }
      100% {
        transform: scale(0.9);
        opacity: 0.7;
      }
    }

    @media (max-width: 768px) {
      .banner-content {
        flex-direction: column;
        align-items: flex-start;
        gap: 8px;
      }
      .separator {
        display: none;
      }
      .btn-exit {
        width: 100%;
        justify-content: center;
      }
    }
  `]
})
export class ImpersonationBannerComponent {
  impersonationService = inject(ImpersonationService);

  esci(): void {
    this.impersonationService.stopImpersonation();
  }
}

