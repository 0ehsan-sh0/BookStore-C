export interface Image {
  id: number;
  height?: string;
  width?: string;
  isPrimary: boolean;
  storedFileName: string;
  relativePath: string;
  fileSize?: string;
  mimeType?: string;
  createdAt?: Date;
  updatedAt?: Date;
  foreignId: number;
  foreignTable: string;
}