import { Inject, Injectable } from '@angular/core';
import { API_URL } from '../models/apiResponse';

@Injectable({
  providedIn: 'root',
})
export class ImageService {
  constructor(@Inject(API_URL) private apiUrl: string) {}

  getUrl(relativePath: string, fileName: string): string {
    // ensure slashes are correct
    return `${this.apiUrl}/images/${relativePath}${fileName}`;
  }
}
