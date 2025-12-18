import { Component, OnInit, inject, DestroyRef } from '@angular/core';
import { CategoryPublicService } from '../../services/Public/category-public.service';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

@Component({
  selector: 'app-public-sidebar',
  standalone: false,
  templateUrl: './public-sidebar.component.html',
  styleUrl: './public-sidebar.component.css',
})
export class PublicSidebarComponent implements OnInit {
  private destroyRef = inject(DestroyRef);
  public categoryPublicService = inject(CategoryPublicService);

  ngOnInit(): void {
    // Fetch the categories from the service
    this.categoryPublicService.getCategoriesWithSub();
  }

  closeSidebar(): void {
    const sidebar = document.getElementById(
      'public-sidebar'
    ) as HTMLInputElement | null;
    if (sidebar) {
      sidebar.checked = false;
    }
  }
}
