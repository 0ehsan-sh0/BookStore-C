import { Component, OnInit } from '@angular/core';
import { Category } from '../../models/category';
import { CategoryPublicService } from '../../services/Public/category-public.service';

@Component({
  selector: 'app-public-sidebar',
  standalone: false,
  templateUrl: './public-sidebar.component.html',
  styleUrl: './public-sidebar.component.css',
})
export class PublicSidebarComponent implements OnInit {
  categories: Category[] = [];

  constructor(private categoryPublicService: CategoryPublicService) {}

  ngOnInit(): void {
    // Fetch the categories from the service
    this.categoryPublicService.getCategoriesWithSub();

    // Subscribe to the BehaviorSubject to get updates
    this.categoryPublicService.categories.subscribe((data) => {
      this.categories = data;
    });
  }

  closeSidebar() : void {
    const sidebar = document.getElementById(
      'public-sidebar'
    ) as HTMLInputElement | null;
    if (sidebar) {
      sidebar.checked = false;
    }
  }
}
