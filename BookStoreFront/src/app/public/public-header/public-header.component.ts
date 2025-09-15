import { Component } from '@angular/core';
import { ThemeControllerService } from '../../ui-service/theme-controller.service';

@Component({
  selector: 'app-public-header',
  standalone: false,
  templateUrl: './public-header.component.html',
  styleUrl: './public-header.component.css',
})
export class PublicHeaderComponent {
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
