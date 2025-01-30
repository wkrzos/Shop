// src/app/models/article.model.ts
export interface Article {
    id: number;
    name: string;
    category: string;
    description?: string;
    imageUrl?: string | null;
  }
  