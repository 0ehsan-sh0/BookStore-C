import { Component } from '@angular/core';
import { User } from '../../models/user';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-sidebar-user',
  standalone: false,
  templateUrl: './sidebar-user.component.html',
  styleUrl: './sidebar-user.component.css',
})
export class SidebarUserComponent {
  user: User | null = null;

  constructor(private authService: AuthService) {
    this.authService.initUser();
  }

  ngOnInit() {
    this.authService.user.subscribe((user) => {
      this.user = user;
    });
  }
}
