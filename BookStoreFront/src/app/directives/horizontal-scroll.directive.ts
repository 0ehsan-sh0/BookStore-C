import { Directive, ElementRef, HostListener } from '@angular/core';

@Directive({
  selector: '[appHorizontalScroll]',
  standalone: false
})
export class HorizontalScrollDirective {

   constructor(private el: ElementRef) {}

  @HostListener('wheel', ['$event'])
  onWheel(event: WheelEvent) {
    event.preventDefault();
    this.el.nativeElement.scrollLeft += event.deltaY;
  }

}
