import { Pipe, PipeTransform } from '@angular/core';
import { toJalaali } from 'jalaali-js';

@Pipe({
  name: 'jalaliDate',
  standalone: false,
})
export class JalaliDatePipe implements PipeTransform {
  transform(value: Date | string, withTime: boolean = false): string {
    try {
      if (!value) return '';

      const date = new Date(value);
      const jDate = toJalaali(date);
      console.log('Pipe input:', value, 'Converted:', jDate);
      const formattedDate = `${jDate.jy}/${this.pad(jDate.jm)}/${this.pad(
        jDate.jd
      )}`;

      if (withTime) {
        const hours = this.pad(date.getHours());
        const minutes = this.pad(date.getMinutes());
        return `${formattedDate} - ${hours}:${minutes}`;
      }

      return formattedDate;
    } catch (error) {
      return '';
    }
  }

  private pad(num: number): string {
    return num.toString().padStart(2, '0');
  }
}
