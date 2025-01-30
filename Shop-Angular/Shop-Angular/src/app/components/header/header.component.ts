// src/app/components/header/header.component.ts
import { Component } from '@angular/core';

@Component({
  selector: 'app-header',
  standalone: false,
  template: `
    <header class="app-header">
      <h1>Moja Aplikacja Sklepowa</h1>
    </header>
  `,
  styleUrls: ['./header.component.css']
})
export class HeaderComponent {}
