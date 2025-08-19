import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminComponent } from './admin/admin/admin.component';
import { AuthorComponent } from './admin/author/author.component';
import { TranslatorComponent } from './admin/translator/translator.component';

const routes: Routes = [
  {
    path: 'admin',
    component: AdminComponent,
    children: [
      { path: 'author', component: AuthorComponent },
      { path: 'translator', component: TranslatorComponent },
    ],
  },
  { path: '**', redirectTo: '' }, // fallback
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
