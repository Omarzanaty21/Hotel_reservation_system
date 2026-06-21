import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { ApiError } from '../../models/error.model'; // Import your new class

@Injectable()
export class HttpErrorInterceptor implements HttpInterceptor {
  
  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(
      catchError((error: HttpErrorResponse) => {
        // Instantiate your class using the factory method
        const apiError = ApiError.fromHttpError(error);

        // Pass the strongly typed class instance down to your components
        return throwError(() => apiError);
      })
    );
  }
}
