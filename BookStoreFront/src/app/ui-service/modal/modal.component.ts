import { Component, ElementRef, Input, ViewChild } from '@angular/core';

@Component({
  selector: 'app-modal',
  standalone: false,
  templateUrl: './modal.component.html',
  styleUrl: './modal.component.css',
})
export class ModalComponent {
  @Input() id: string = 'modal'; 
  @ViewChild('dialogRef') dialogRef!: ElementRef<HTMLDialogElement>;

  open() {
    this.dialogRef.nativeElement?.showModal();
  }

  close() {
    this.dialogRef.nativeElement?.close();
  }
}