import { PaginationInfo } from './apiResponse';
import { Author } from './author';
import { Category } from './category';
import { Image } from './image';
import { Tag } from './tag';
import { Translator } from './translator';

export interface Book {
  id: number;
  name: string;
  englishName: string;
  description: string;
  price: number;
  printSeries: number; // سری چاپ
  isbn: string; // شابک
  coverType: string; // نوع جلد
  format: string; // قطع
  pages: string;
  publishYear: string;
  publisher: string;
  isRecommended: boolean;
  stock: number;
  authorId: number;
  createdAt?: Date;
  updatedAt?: Date;
}

export interface BookAllData {
  id: number;
  name: string;
  englishName: string;
  description: string;
  price: number;
  printSeries: number; // سری چاپ
  isbn: string; // شابک
  coverType: string; // نوع جلد
  format: string; // قطع
  pages: string;
  publishYear: string;
  publisher: string;
  isRecommended: boolean;
  stock: number;
  quantity: number;
  authorId: number;
  createdAt?: Date;
  updatedAt?: Date;

  comments?: Comment[];
  translators?: Translator[];
  categories?: Category[];
  tags?: Tag[];
  images?: Image[];
  author?: Author;
}

export interface BookListResponse {
  books?: BookAllData[];
  pagination?: BPaginationInfo;
}

export interface BPaginationInfo extends PaginationInfo {}

export interface CreateBookRequest {
  name: string;
  englishName?: string;
  description?: string;
  price: number;
  printSeries: number;
  isbn: string;
  coverType: string;
  format: string;
  pages: number;
  publishYear: number;
  publisher: string;
  stock: number;
  authorId: number;
  images: Image[];
  translators?: number[];
  categories: number[];
  tags: number[];
}

export interface UpdateBookRequest {
  name: string;
  englishName?: string;
  description?: string;
  price: number;
  printSeries: number;
  isbn: string;
  coverType: string;
  format: string;
  pages: number;
  publishYear: number;
  publisher: string;
  stock: number;
  authorId: number;
  translators?: number[];
  categories: number[];
  tags: number[];
}
