// src/app/services/articles.service.ts
import { Injectable, signal, computed } from '@angular/core';
import { Article } from '../models/article.model';

@Injectable({
  providedIn: 'root'
})
export class ArticlesService {
  
  categories = ['Elektronika', 'Książki', 'Inne'];

  private articlesSignal = signal<Article[]>([
    {
      id: 1,
      name: 'Smartfon X',
      category: 'Elektronika',
      description: 'Nowoczesny smartfon z dużym ekranem',
      imageUrl: null
    },
    {
      id: 2,
      name: 'Książka o Angularze',
      category: 'Książki',
      description: 'Podręcznik opisujący podstawy Angulara',
      imageUrl: null
    }
  ]);

  articles = computed(() => this.articlesSignal());

  constructor() {}

  addArticle(article: Article) {
    const current = this.articlesSignal();
    this.articlesSignal.set([...current, article]);
  }

  removeArticle(articleId: number) {
    const filtered = this.articlesSignal().filter(a => a.id !== articleId);
    this.articlesSignal.set(filtered);
  }

  updateArticle(updatedArticle: Article) {
    const newList = this.articlesSignal().map(a =>
      a.id === updatedArticle.id ? { ...a, ...updatedArticle } : a
    );
    this.articlesSignal.set(newList);
  }
}
