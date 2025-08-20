import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AdminComponent } from './admin/admin/admin.component';
import { CommonModule, NgOptimizedImage } from '@angular/common';
import {
  Book,
  BookType,
  ChartColumnBig,
  Grid2x2,
  House,
  LucideAngularModule,
  Menu,
  MessageCircleMore,
  Pencil,
  PencilLine,
  Plus,
  Tag,
  Trash,
} from 'lucide-angular';
import { HeaderComponent } from './admin/header/header.component';
import { SidebarComponent } from './admin/sidebar/sidebar.component';
import { AuthorComponent } from './admin/author/author.component';
import {
  provideHttpClient,
  withInterceptors,
  withInterceptorsFromDi,
} from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { CreateComponent } from './admin/author/create/create.component';
import { ModalComponent } from './ui-service/modal/modal.component';
import { FormsModule } from '@angular/forms';
import { UpdateComponent } from './admin/author/update/update.component';
import { TranslatorComponent } from './admin/translator/translator.component';
import { CreateTranslatorComponent } from './admin/translator/create-translator/create-translator.component';
import { UpdateTranslatorComponent } from './admin/translator/update-translator/update-translator.component';
import { CategoryComponent } from './admin/category/category.component';
import { CreateCategoryComponent } from './admin/category/create-category/create-category.component';
import { UpdateCategoryComponent } from './admin/category/update-category/update-category.component';
import { environment } from '../environments/environment';
import { API_URL } from './models/apiResponse';
import { baseUrlInterceptor } from './interceptors/base-url.interceptor';
import { BookComponent } from './admin/book/book.component';
import { CreateBookComponent } from './admin/book/create-book/create-book.component';
import { UpdateBookComponent } from './admin/book/update-book/update-book.component';
import { BookCategoryComponent } from './admin/book/book-category/book-category.component';
import { BookTranslatorComponent } from './admin/book/book-translator/book-translator.component';

@NgModule({
  declarations: [
    AppComponent,
    AdminComponent,
    HeaderComponent,
    SidebarComponent,
    AuthorComponent,
    CreateComponent,
    ModalComponent,
    UpdateComponent,
    TranslatorComponent,
    CreateTranslatorComponent,
    UpdateTranslatorComponent,
    CategoryComponent,
    CreateCategoryComponent,
    UpdateCategoryComponent,
    BookComponent,
    CreateBookComponent,
    UpdateBookComponent,
    BookCategoryComponent,
    BookTranslatorComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    CommonModule,
    BrowserAnimationsModule,
    LucideAngularModule.pick({
      Menu,
      PencilLine,
      Book,
      Grid2x2,
      BookType,
      MessageCircleMore,
      Tag,
      Pencil,
      Trash,
      House,
      Plus,
      ChartColumnBig,
    }),
    FormsModule,
  ],
  providers: [
    provideHttpClient(withInterceptorsFromDi()),
    { provide: API_URL, useValue: environment.apiUrl },
    provideHttpClient(withInterceptors([baseUrlInterceptor])),
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
