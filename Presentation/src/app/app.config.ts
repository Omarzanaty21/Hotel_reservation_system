import { ApplicationConfig, provideBrowserGlobalErrorListeners, importProvidersFrom } from '@angular/core';
import { provideRouter } from '@angular/router';
import { HTTP_INTERCEPTORS, HttpClientModule, withInterceptorsFromDi } from '@angular/common/http';
import { provideHttpClient  } from '@angular/common/http';

import { routes } from './app.routes';
import { HttpErrorInterceptor } from './core/interceptors/error.interceptor';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    importProvidersFrom(HttpClientModule),
    provideRouter(routes),
    provideHttpClient(withInterceptorsFromDi()),
    {
    provide: HTTP_INTERCEPTORS,
    useClass: HttpErrorInterceptor,
    multi: true
    }
  ]
};
