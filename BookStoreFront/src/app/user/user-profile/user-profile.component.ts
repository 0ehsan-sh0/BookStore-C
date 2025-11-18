import { Component } from '@angular/core';
import { User } from '../../models/user';
import { AuthService } from '../../services/auth.service';
import { NgForm } from '@angular/forms';
import { UserPanelService } from '../../services/user/user-panel.service';

@Component({
  selector: 'app-user-profile',
  standalone: false,
  templateUrl: './user-profile.component.html',
  styleUrl: './user-profile.component.css',
})
export class UserProfileComponent {
  user: User = {
  name: '',
  lastName: '',
  mobile: '',
  // add any other fields your User model requires
} as User;

  constructor(
    private authService: AuthService,
    private userPanelService: UserPanelService
  ) {}

  ngOnInit() {
    this.authService.user.subscribe((user) => {
        if (user) this.user = user;
    });
  }

  onSubmit(form: NgForm) {
    if (form.valid) {
      this.userPanelService.updateUser(form.value);
    }
  }
}
