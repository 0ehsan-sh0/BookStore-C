import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { User, UserRole } from '../../models/user';
import { AuthService } from '../../services/auth.service';
import { map } from 'rxjs';

export const adminGuard: CanActivateFn = (route, state) => {
  const auth = inject(AuthService);
  const router = inject(Router);

  return auth.user.pipe(
    map((user) => {
      if (user?.role === UserRole.Admin) return true;
      router.navigate(['/']);
      return false;
    })
  );
};
