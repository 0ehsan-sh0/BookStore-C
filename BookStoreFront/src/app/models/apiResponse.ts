export interface ApiResponse<T> {
  message: string;
  data?: T;        // present on success
  errors?: any;    // present on error
}