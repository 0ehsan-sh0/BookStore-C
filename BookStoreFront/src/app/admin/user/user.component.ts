import { Component, OnInit, viewChild, inject } from '@angular/core';
import { UPaginationInfo, User } from '../../models/user';
import { UserService } from '../../services/admin/user.service';
import { ModalComponent } from '../../ui-service/modal/modal.component';

@Component({
  selector: 'app-user',
  standalone: false,
  templateUrl: './user.component.html',
  styleUrl: './user.component.css',
})
export class UserComponent implements OnInit {
  userService = inject(UserService);
  users = this.userService.users;
  pagination = this.userService.pagination;
  deleteId: number = 0;
  createUserModal = viewChild<ModalComponent>('createUser');
  updateUserModal = viewChild<ModalComponent>('updateUser');
  deleteUserModal = viewChild<ModalComponent>('deleteUser');
  searchText = '';

  constructor() {}

  ngOnInit() {
    this.fetchUsers();
  }

  fetchUsers() {
    this.userService.getUsers(
      this.pagination().pageNumber,
      this.pagination().pageSize,
      this.searchText
    );
  }

  create() {
    this.createUserModal()!.open();
  }

  update(id: number) {
    this.userService.getById(id);
    this.updateUserModal()!.open();
  }

  delete(id: number) {
    this.deleteId = id;
    this.deleteUserModal()!.open();
  }

  deleteConfirmed() {
    this.userService.delete(this.deleteId);
    this.closeDialog('deleteUserModal');
  }

  onSearch() {
    this.fetchUsers();
  }

  changePage(page: number) {
    if (page >= 1 && page <= this.pagination().totalPages) {
      this.userService.getUsers(
        page,
        this.pagination().pageSize,
        this.searchText
      );
    }
  }

  getPageArray(): number[] {
    const total = this.pagination().totalPages;
    const current = this.pagination().pageNumber;
    const pages: number[] = [];

    if (total <= 7) {
      for (let i = 1; i <= total; i++) {
        pages.push(i);
      }
    } else {
      pages.push(1);
      if (current > 3) {
        pages.push(-1); // Ellipsis
      }
      for (
        let i = Math.max(2, current - 1);
        i <= Math.min(total - 1, current + 1);
        i++
      ) {
        pages.push(i);
      }
      if (current < total - 2) {
        pages.push(-1); // Ellipsis
      }
      pages.push(total);
    }
    return pages;
  }

  closeDialog(tab: string) {
    switch (tab) {
      case 'createUserModal':
        this.createUserModal()!.close();
        break;
      case 'updateUserModal':
        this.updateUserModal()!.close();
        break;
      case 'deleteUserModal':
        this.deleteUserModal()!.close();
        break;
    }
  }
}
