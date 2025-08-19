import { Component, Input } from '@angular/core';
import { trigger, transition, style, animate } from '@angular/animations';
import { CommonModule } from '@angular/common';
@Component({
  selector: 'app-alert',
  templateUrl: './alert.component.html',
  styleUrl: './alert.component.css',
  standalone: true,
  imports: [CommonModule],
  animations: [
    trigger('fade', [
      transition(':enter', [
        style({ opacity: 0, transform: 'translateY(-10px)' }),
        animate('300ms ease-out', style({ opacity: 1, transform: 'translateY(0)' })),
      ]),
      transition(':leave', [
        animate('300ms ease-in', style({ opacity: 0, transform: 'translateY(-10px)' }))
      ]),
    ])
  ],
})
export class AlertComponent {
  @Input() message: string = 'Alert!';
  @Input() type: 'success' | 'error' | 'info' | 'warning' = 'success';

  visible = false;

  show(msg: string = this.message, type: typeof this.type = this.type) {
    this.message = msg;
    this.type = type;
    this.visible = true;
  }

  hide() {
    setTimeout(() => this.visible = false, 3000);
  }
}
