import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminComponent } from './admin/admin/admin.component';
import { AuthorComponent } from './admin/author/author.component';
import { TranslatorComponent } from './admin/translator/translator.component';
import { CategoryComponent } from './admin/category/category.component';
import { BookComponent } from './admin/book/book.component';
import { TagComponent } from './admin/tag/tag.component';
import { CommentComponent } from './admin/comment/comment.component';

const routes: Routes = [
  {
    path: 'admin',
    component: AdminComponent,
    children: [
      { path: 'author', component: AuthorComponent },
      { path: 'translator', component: TranslatorComponent },
      { path: 'category', component: CategoryComponent },
      { path: 'book', component: BookComponent },
      { path: 'tag', component: TagComponent },
      { path: 'comment', component: CommentComponent },
    ],
  },
  { path: '**', redirectTo: '' }, // fallback
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
