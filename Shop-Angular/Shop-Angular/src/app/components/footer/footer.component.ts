// src/app/components/footer/footer.component.ts
import { Component } from '@angular/core';

@Component({
  selector: 'app-footer',
  standalone: false,
  template: `
    <footer class="app-footer">
      <p>Stopka &copy; 2025</p>
    </footer>
  `,
  styleUrls: ['./footer.component.css']
})
export class FooterComponent {}
