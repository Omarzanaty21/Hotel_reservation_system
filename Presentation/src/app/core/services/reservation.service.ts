import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ReservationCreateRequest } from '../../models/reservation/reservation.model';

@Injectable({
  providedIn: 'root',
})
export class ReservationsService {
  private readonly base = '/api/roommanagement/Reservations';

  constructor(private http: HttpClient) {}

  createReservation(
    reservation: ReservationCreateRequest
  ): Observable<void> {
    return this.http.post<void>(this.base, reservation);
  }
}