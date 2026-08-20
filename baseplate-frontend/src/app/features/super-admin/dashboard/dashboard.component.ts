import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router'; // 👈 IMPORT FONDAMENTALE

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [RouterOutlet], // 👈 AGGIUNGILO QUI
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})
export class DashboardComponent { }
