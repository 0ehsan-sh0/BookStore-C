import { Component, OnInit } from '@angular/core';
import { ThemeControllerService } from '../../ui-service/theme-controller.service';

@Component({
  selector: 'app-admin-header',
  standalone: false,
  templateUrl: './header.component.html',
  styleUrl: './header.component.css',
})
export class HeaderComponent implements OnInit {
  public isLight = true;

 constructor(private themeController: ThemeControllerService) {}

  toggleTheme() {
    this.themeController.toggleTheme();
    this.isLight = !this.isLight;
  }

   ngOnInit(): void {
    this.themeController.theme.getValue() === 'cupcake'
      ? (this.isLight = true)
      : (this.isLight = false);
  }
}
