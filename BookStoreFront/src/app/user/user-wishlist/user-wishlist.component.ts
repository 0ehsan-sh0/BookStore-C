import { Component, OnInit } from '@angular/core';
import { BookAllData } from '../../models/book';
import { AuthService } from '../../services/auth.service';
import { ImageService } from '../../services/image.service';

@Component({
  selector: 'app-user-wishlist',
  standalone: false,
  templateUrl: './user-wishlist.component.html',
  styleUrl: './user-wishlist.component.css',
})
export class UserWishlistComponent implements OnInit {
  wishlist: BookAllData[] = [];

  constructor(private auth: AuthService, private imageService: ImageService) {}

  ngOnInit(): void {
    // Subscribe to user BehaviorSubject
    this.auth.user.subscribe((user) => {
      this.wishlist = user?.wishList ?? [];
    });
  }

  getImage(book: BookAllData): string {
    const img = book.images?.[0];
    if (!img) return 'https://placehold.co/300x400?text=No+Image';

    return this.imageService.getUrl(img.relativePath, img.storedFileName);
  }
}
