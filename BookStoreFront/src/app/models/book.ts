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