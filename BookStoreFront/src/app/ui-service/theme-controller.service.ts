import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ThemeControllerService {

  public theme = new BehaviorSubject<string>(localStorage.getItem('theme') || 'cupcake');

  constructor() { }

  toggleTheme() {
    if (this.theme.getValue() === 'forest') {
      localStorage.setItem('theme', 'cupcake');
      this.theme.next('cupcake');
    } else {
      localStorage.setItem('theme', 'forest');
      this.theme.next('forest');
    }
  }
}
