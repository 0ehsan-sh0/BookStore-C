import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { API_URL } from '../models/apiResponse';

export const baseUrlInterceptor: HttpInterceptorFn = (req, next) => {
  const baseUrl = inject(API_URL);

  const apiReq = req.clone({ url: `${baseUrl}/${req.url}` });

  return next(apiReq);
};
