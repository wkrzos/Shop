// src/app/components/article-list/article-list.component.ts
import { Component, Output, EventEmitter } from '@angular/core';
import { ArticlesService } from '../../services/articles.service';
import { Article } from '../../models/article.model';
import { signal, effect, computed } from '@angular/core';

@Component({
  selector: 'app-article-list',
  standalone: false,
  templateUrl: './article-list.component.html',
  styleUrls: ['./article-list.component.css']
})
export class ArticleListComponent {

  @Output() addClicked = new EventEmitter<void>();

  articles;

  constructor(private articlesService: ArticlesService) {
    this.articles = this.articlesService.articles;
  }

  onRemoveArticle(id: number) {
    this.articlesService.removeArticle(id);
  }

  onAddNew() {
    this.addClicked.emit();
  }
}
