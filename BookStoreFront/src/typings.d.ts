declare module 'jalaali-js' {
  export interface JalaaliDate {
    jy: number;
    jm: number;
    jd: number;
  }

  export function toJalaali(date: Date | number | string): JalaaliDate;
  export function toGregorian(jy: number, jm: number, jd: number): Date;
}