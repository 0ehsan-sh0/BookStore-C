export interface Translator {
  id: number;
  name: string;
  description: string;
  createdAt?: Date;
  updatedAt?: Date;
}

export interface TPaginationInfo {
  totalCount: number;
  pageSize: number;
  pageNumber: number;
  totalPages: number;
}

export interface TranslatorListResponse {
  translators?: Translator[];
  pagination?: TPaginationInfo;
}

// create-Translator-request.ts
export interface CreateTranslatorRequest {
  name: string;
  description: string;
}

// update-Translator-request.ts
export interface UpdateTranslatorRequest {
  name: string;
  description: string;
}