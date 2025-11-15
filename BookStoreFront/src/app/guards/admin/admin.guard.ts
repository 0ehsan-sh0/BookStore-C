import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { User, UserRole } from '../../models/user';

export const adminGuard: CanActivateFn = (route, state) => {
  const router = inject(Router);
  const userStr = localStorage.getItem('user');
  if (userStr) {
    const user = JSON.parse(userStr) as User;
    if (user && user.role === UserRole.Admin) {
      return true;
    }
  }
  router.navigate(['/']);
  return false;
};
