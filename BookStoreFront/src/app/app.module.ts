import { APP_INITIALIZER, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AdminComponent } from './admin/admin/admin.component';
import { CommonModule } from '@angular/common';
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
  ShoppingCart,
  Instagram,
  Phone,
  Search,
  ShieldCheck,
  ChartPie,
  Truck,
  UserPlus,
  ChevronLeft,
  BookOpen,
  Target,
  Eye,
  Heart,
  Users,
  HelpCircle,
  ArrowLeft,
  MessagesSquare,
  MapPin,
  Mail,
  Send,
  SendHorizontal,
  User,
  LogOut,
  LogIn,
  AlertCircle,
  Home,
  CreditCard,
  ShoppingBag,
  Minus,
  Trash2,
  PlusCircle,
  CheckCircle,
  Printer,
  AlertTriangle,
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
import { BookComponent as BookPublicComponent } from './public/book/book.component';
import { CreateBookComponent } from './admin/book/create-book/create-book.component';
import { UpdateBookComponent } from './admin/book/update-book/update-book.component';
import { BookCategoryComponent } from './admin/book/book-category/book-category.component';
import { BookTranslatorComponent } from './admin/book/book-translator/book-translator.component';
import { JalaliDatePipe } from './pipes/jalali-date.pipe';
import { TagComponent } from './admin/tag/tag.component';
import { CreateTagComponent } from './admin/tag/create-tag/create-tag.component';
import { UpdateTagComponent } from './admin/tag/update-tag/update-tag.component';
import { BookTagComponent } from './admin/book/book-tag/book-tag.component';
import { BookImageComponent } from './admin/book/book-image/book-image.component';
import { CommentComponent } from './admin/comment/comment.component';
import { PublicComponent } from './public/public/public.component';
import { PublicHeaderComponent } from './public/public-header/public-header.component';
import { PublicSidebarComponent } from './public/public-sidebar/public-sidebar.component';
import { PublicFooterComponent } from './public/public-footer/public-footer.component';
import { HomeComponent } from './public/home/home.component';
import { HorizontalScrollDirective } from './directives/horizontal-scroll.directive';
import { PublicBannersComponent } from './public/home/public-banners/public-banners.component';
import { BookDetailsComponent } from './public/book-details/book-details.component';
import { AboutUsComponent } from './public/about-us/about-us.component';
import { ContactUsComponent } from './public/contact-us/contact-us.component';
import { AlertService } from './ui-service/alert.service';
import { LoginComponent } from './public/login/login.component';
import { RegisterComponent } from './public/register/register.component';
import { authCredentialsInterceptor } from './interceptors/auth-credentials.interceptor';
import { AuthService } from './services/auth.service';
import { UserPublicComponent } from './user/user-public/user-public.component';
import { SidebarUserComponent } from './user/sidebar-user/sidebar-user.component';
import { UserProfileComponent } from './user/user-profile/user-profile.component';
import { UserOrdersComponent } from './user/user-orders/user-orders.component';
import { UserWishlistComponent } from './user/user-wishlist/user-wishlist.component';
import { UserAddressesComponent } from './user/user-addresses/user-addresses.component';
import { UserSettingComponent } from './user/user-setting/user-setting.component';
import { UserHeaderComponent } from './user/user-header/user-header.component';
import { UserFooterComponent } from './user/user-footer/user-footer.component';
import { UserAddressCreateComponent } from './user/user-addresses/user-address-create/user-address-create.component';
import { UserAddressUpdateComponent } from './user/user-addresses/user-address-update/user-address-update.component';
import { UserCartComponent } from './user/user-cart/user-cart.component';
import { UserCartItemComponent } from './user/user-cart/user-cart-item/user-cart-item.component';
import { UserCartCheckoutComponent } from './user/user-cart/user-cart-checkout/user-cart-checkout.component';
import { CheckoutComponent } from './user/checkout/checkout.component';
import { UserInvoiceComponent } from './user/user-invoice/user-invoice.component';

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
    JalaliDatePipe,
    TagComponent,
    CreateTagComponent,
    UpdateTagComponent,
    BookTagComponent,
    BookImageComponent,
    CommentComponent,
    PublicComponent,
    PublicHeaderComponent,
    PublicSidebarComponent,
    PublicFooterComponent,
    HomeComponent,
    HorizontalScrollDirective,
    PublicBannersComponent,
    BookPublicComponent,
    BookDetailsComponent,
    AboutUsComponent,
    ContactUsComponent,
    LoginComponent,
    RegisterComponent,
    UserPublicComponent,
    SidebarUserComponent,
    UserProfileComponent,
    UserOrdersComponent,
    UserWishlistComponent,
    UserAddressesComponent,
    UserSettingComponent,
    UserHeaderComponent,
    UserFooterComponent,
    UserAddressCreateComponent,
    UserAddressUpdateComponent,
    UserCartComponent,
    UserCartItemComponent,
    UserCartCheckoutComponent,
    CheckoutComponent,
    UserInvoiceComponent,
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
      ShoppingCart,
      Phone,
      Search,
      ShieldCheck,
      ChartPie,
      Truck,
      UserPlus,
      ChevronLeft,
      BookOpen,
      Target,
      Eye,
      Heart,
      Users,
      HelpCircle,
      ArrowLeft,
      MessagesSquare,
      MapPin,
      Mail,
      Send,
      SendHorizontal,
      User,
      LogOut,
      LogIn,
      AlertCircle,
      Home,
      CreditCard,
      ShoppingBag,
      Minus,
      Trash2,
      PlusCircle,
      CheckCircle,
      Printer,
      AlertTriangle
    }),
    FormsModule,
  ],
  providers: [
    provideHttpClient(withInterceptorsFromDi()),
    { provide: API_URL, useValue: environment.apiUrl },
    provideHttpClient(
      withInterceptors([baseUrlInterceptor, authCredentialsInterceptor])
    ),
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
