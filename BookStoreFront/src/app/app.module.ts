import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AdminComponent } from './admin/admin/admin.component';
import { CommonModule } from '@angular/common';
import {
  Book,
  BookType,
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
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { CreateComponent } from './admin/author/create/create.component';
import { ModalComponent } from './ui-service/modal/modal.component';
import { FormsModule } from '@angular/forms';
import { UpdateComponent } from './admin/author/update/update.component';

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
      Plus
    }),
    FormsModule,
  ],
  providers: [
    provideHttpClient(withInterceptorsFromDi())
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
