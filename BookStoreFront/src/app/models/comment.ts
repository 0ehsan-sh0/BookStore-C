export interface Comment {
  id: number;
  comment: string;
  status: boolean;
  foreignTable: string;
  foreignId: number;
  createdAt: Date;
  updatedAt: Date;
}