import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminComponent } from './admin/admin/admin.component';
import { AuthorComponent } from './admin/author/author.component';
import { TranslatorComponent } from './admin/translator/translator.component';
import { CategoryComponent } from './admin/category/category.component';
import { BookComponent } from './admin/book/book.component';
import { BookComponent as BookPublicComponent } from './public/book/book.component';
import { TagComponent } from './admin/tag/tag.component';
import { CommentComponent } from './admin/comment/comment.component';
import { PublicComponent } from './public/public/public.component';
import { HomeComponent } from './public/home/home.component';
import { BookDetailsComponent } from './public/book-details/book-details.component';
import { AboutUsComponent } from './public/about-us/about-us.component';
import { ContactUsComponent } from './public/contact-us/contact-us.component';
import { LoginComponent } from './public/login/login.component';
import { RegisterComponent } from './public/register/register.component';
import { adminGuard } from './guards/admin/admin.guard';
import { UserPublicComponent } from './user/user-public/user-public.component';
import { UserProfileComponent } from './user/user-profile/user-profile.component';
import { UserOrdersComponent } from './user/user-orders/user-orders.component';
import { UserWishlistComponent } from './user/user-wishlist/user-wishlist.component';
import { UserAddressesComponent } from './user/user-addresses/user-addresses.component';
import { UserSettingComponent } from './user/user-setting/user-setting.component';
import { UserCartComponent } from './user/user-cart/user-cart.component';
import { CheckoutComponent } from './user/checkout/checkout.component';
import { UserInvoiceComponent } from './user/user-invoice/user-invoice.component';
import { InvoiceComponent } from './admin/invoice/invoice.component';
import { PaymentComponent } from './admin/payment/payment.component';
import { UserComponent } from './admin/user/user.component';
import { UserCommentComponent } from './user/user-comment/user-comment.component';
import { userPanelGuard } from './guards/user/user-panel.guard';

const routes: Routes = [
  {
    path: 'admin',
    component: AdminComponent,
    canActivate: [adminGuard],
    children: [
      { path: 'author', component: AuthorComponent },
      { path: 'translator', component: TranslatorComponent },
      { path: 'category', component: CategoryComponent },
      { path: 'book', component: BookComponent },
      { path: 'tag', component: TagComponent },
      { path: 'comment', component: CommentComponent },
      { path: 'invoice', component: InvoiceComponent },
      { path: 'payment', component: PaymentComponent },
      { path: 'user', component: UserComponent },
    ],
  },
  {
    path: '',
    component: PublicComponent,
    children: [
      { path: '', component: HomeComponent },
      { path: 'books', component: BookPublicComponent },
      { path: 'books/:id', component: BookDetailsComponent },
      { path: 'about-us', component: AboutUsComponent },
      { path: 'contact-us', component: ContactUsComponent },
      { path: 'login', component: LoginComponent },
      { path: 'register', component: RegisterComponent },
      { path: 'cart', component: UserCartComponent },
    ],
  },
  {
    path: 'purchase',
    component: PublicComponent,
    children: [
      { path: 'checkout', component: CheckoutComponent },
      { path: 'invoice/:id', component: UserInvoiceComponent },
    ],
  },
  {
    path: 'user',
    component: UserPublicComponent,
    canActivate: [userPanelGuard],
    children: [
      { path: '', component: UserProfileComponent },
      { path: 'orders', component: UserOrdersComponent },
      { path: 'wishlist', component: UserWishlistComponent },
      { path: 'addresses', component: UserAddressesComponent },
      { path: 'settings', component: UserSettingComponent },
      { path: 'comments', component: UserCommentComponent },
    ],
  },

  { path: '**', redirectTo: '' }, // fallback
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
