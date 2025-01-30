import { Component, EventEmitter, Output } from '@angular/core';
import { ArticlesService } from '../../services/articles.service';
import { Article } from '../../models/article.model';

@Component({
  selector: 'app-article-form',
  standalone: false,
  templateUrl: './article-form.component.html',
  styleUrls: ['./article-form.component.css']
})
export class ArticleFormComponent {
  @Output() formClose = new EventEmitter<void>();

  // Prosta reprezentacja wartości formularza
  articleData = {
    name: '',
    category: '',
    description: ''
    // imageUrl: null
  };

  constructor(private articlesService: ArticlesService) {}

  onSubmit() {
    // Na szybko generujemy ID – w realnej aplikacji ID zwykle przychodzi z bazy
    const newId = Math.floor(Math.random() * 100000);

    const newArticle: Article = {
      id: newId,
      name: this.articleData.name,
      category: this.articleData.category,
      description: this.articleData.description,
      imageUrl: null
    };

    this.articlesService.addArticle(newArticle);
    this.onClose(); // Zamknięcie formularza
  }

  onClose() {
    this.formClose.emit();
  }

  getCategories() {
    return this.articlesService.categories;
  }
}
