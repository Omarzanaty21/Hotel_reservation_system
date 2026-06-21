// api-error.model.ts

import { HttpErrorResponse } from "@angular/common/http";

export class ApiError {
  public message: string;
  public errorCode: string;
  public details: { [key: string]: string[] } | null;

  constructor(message: string, errorCode: string, details: { [key: string]: string[] } | null = null) {
    this.message = message;
    this.errorCode = errorCode;
    this.details = details;
  }

  /**
   * Helper method to flatten all dictionary values into a single array of strings
   */
  public getFlattenedDetails(): string[] {
    if (!this.details) return [];
    return Object.values(this.details).reduce((acc, currentArray) => {
      return acc.concat(currentArray);
    }, [] as string[]);
  }

  /**
   * Factory method to map an incoming HttpErrorResponse to this class
   */
  public static fromHttpError(error: HttpErrorResponse): ApiError {
    console.log(error.error);
    if (error.status === 0 || error.status === 500) {
      return new ApiError(
        'Our servers are currently unreachable. Please check your connection or try again later.',
        'SERVER_UNREACHABLE');
    }
    
    // Attempt to read the typed payload from the backend response body
    const serverPayload = error.error;

    return new ApiError(
      serverPayload?.message || `Error ${error.status}: ${error.statusText}`,
      serverPayload?.errorCode || `HTTP_${error.status}`,
      serverPayload?.details || null
    );
  }
}
