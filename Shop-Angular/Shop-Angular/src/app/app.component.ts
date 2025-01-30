// src/app/components/app.component.ts
import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  standalone: false,
  template: `
    <app-header></app-header>

    <div class="container">
      <app-article-list (addClicked)="onAddArticle()"></app-article-list>
      
      <!-- Przykład prostego modalu/okna lub sekcji widocznej warunkowo -->
      <div *ngIf="showForm" class="modal-backdrop">
        <div class="modal-content">
          <app-article-form (formClose)="showForm = false"></app-article-form>
        </div>
      </div>
    </div>

    <app-footer></app-footer>
  `,
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  showForm = false;

  onAddArticle() {
    this.showForm = true;
  }
}
