import { Pipe, PipeTransform } from '@angular/core';
import { UserRole } from '../models/user';

@Pipe({
  name: 'userRole',
  standalone: false
})
export class UserRolePipe implements PipeTransform {

  transform(value: UserRole): string {
    if (!value) return '';

    switch (value) {
      case UserRole.Admin:
        return 'مدیر';
      case UserRole.User:
        return 'کاربر';
      default:
        return 'نقش نامشخص';
    }
  }

}
