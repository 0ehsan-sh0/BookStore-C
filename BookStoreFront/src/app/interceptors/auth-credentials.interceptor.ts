import { HttpInterceptorFn } from '@angular/common/http';

export const authCredentialsInterceptor: HttpInterceptorFn = (req, next) => {
  const clonedRequest = req.clone({
    withCredentials: true, // ✅ apply cookies globally
  });

  return next(clonedRequest);
};
