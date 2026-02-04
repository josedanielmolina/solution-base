import { HttpInterceptorFn } from '@angular/common/http';
import { StorageUtils } from '@core/utils/common.utils';

const TOKEN_KEY = 'auth_token';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const token = StorageUtils.get<string>(TOKEN_KEY);

  if (token) {
    req = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });
  }

  return next(req);
};
