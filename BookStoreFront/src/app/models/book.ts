import { Author } from "./author";
import { Category } from "./category";
import { Image } from "./image";
import { Tag } from "./tag";
import { Translator } from "./translator";

export interface Book {
  id: number;
  name: string;
  englishName: string;
  description: string;
  price: number;
  printSeries: number; // سری چاپ
  isbn: string;        // شابک
  coverType: string;   // نوع جلد
  format: string;      // قطع
  pages: string;
  publishYear: string;
  publisher: string;
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
  printSeries: number;   // سری چاپ
  isbn: string;          // شابک
  coverType: string;     // نوع جلد
  format: string;        // قطع
  pages: string;
  publishYear: string;
  publisher: string;
  authorId: number;
  createdAt?: Date;
  updatedAt?: Date;

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

export interface BPaginationInfo {
  totalCount: number;
  pageSize: number;
  pageNumber: number;
  totalPages: number;
}

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
  authorId: number;
  images: File[];        
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
  authorId: number;
  translators?: number[];
  categories: number[];
  tags: number[];
}