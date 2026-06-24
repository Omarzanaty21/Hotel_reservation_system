import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ReservationCreateRequest, ReservationDto } from '../../models/reservation/reservation.model';
import { ReservationFilter } from '../../models/reservation/reservation-filter.model';
import { PagedResult } from '../../shared/models/pagedResult.model';

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

  getReservations(
    filter: ReservationFilter,
    pageIndex = 0,
    pageSize = 5
  ): Observable<PagedResult<ReservationDto>> {
    const params =
      `?pageIndex=${pageIndex}&pageSize=${pageSize}` +
      `&filter.searchQuery=${filter.searchQuery ?? ''}` +
      `&filter.createdAt=${filter.createdAt ?? ''}`;
    return this.http.get<PagedResult<ReservationDto>>(`${this.base}${params}`);
  }
}