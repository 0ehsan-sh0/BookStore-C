import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { User, UserRole } from '../../models/user';
import { AuthService } from '../../services/auth.service';
import { filter, map, take } from 'rxjs';

export const adminGuard: CanActivateFn = (route, state) => {
  const auth = inject(AuthService);
  const router = inject(Router);

  return auth.user$.pipe(
    // 1. Wait until the user object is NOT null
    filter((user) => user !== null),

    // 2. Take the first non-null value and then complete the observable
    take(1),

    // 3. Now that we have a user, perform the role check
    map((user) => {
      if (user?.role === UserRole.Admin) {
        return true; // Access granted
      }

      // If not an admin, redirect
      router.navigate(['/']);
      return false; // Access denied
    })
  );
};
