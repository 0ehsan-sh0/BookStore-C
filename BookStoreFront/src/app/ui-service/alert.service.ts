import { ComponentRef, Injectable, ViewContainerRef } from '@angular/core';
import { AlertComponent } from './alert/alert.component';

@Injectable({
  providedIn: 'root',
})
export class AlertService {
  private containerRef: ViewContainerRef | null = null;
  private currentAlert: ComponentRef<AlertComponent> | null = null;

  registerContainer(ref: ViewContainerRef) {
    this.containerRef = ref;
  }

  show(
    message: string,
    type: 'success' | 'error' | 'info' | 'warning' = 'success'
  ) {
    console.log('show');
    if (!this.containerRef) return;
    console.log('show after if');

    // Remove previous alert
    this.currentAlert?.destroy();

    const alertRef = this.containerRef.createComponent(AlertComponent);

    (alertRef.instance as AlertComponent).message = message;
    alertRef.instance.type = type;
    alertRef.instance.show();
    // Auto-hide after 3s
    alertRef.instance.hide();
    setTimeout(() => alertRef.destroy(), 4000);

    this.currentAlert = alertRef;
  }
}
