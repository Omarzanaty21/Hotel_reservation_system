import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PagedResult } from '../../shared/models/pagedResult.model';
import { Room, CreateRoom } from '../../models/room/room.model';

@Injectable({
  providedIn: 'root',
})
export class RoomsService {
  private readonly base = '/api/roommanagement/Rooms';

  constructor(private http: HttpClient) {}

  getAvailableRooms(filter: any, pageIndex = 0, pageSize = 10): Observable<PagedResult<Room>> {
    const params = `?pageIndex=${pageIndex}&pageSize=${pageSize}&checkIn=${filter.checkIn || ''}&checkOut=${filter.checkOut || ''}&capacity=${filter.capacity || ''}`;
    return this.http.get<PagedResult<Room>>(`${this.base}${params}`);
  }

  createRoom(room: CreateRoom): Observable<void> {
    const formData = new FormData();
    formData.append('name', room.name);
    formData.append('description', room.description);
    formData.append('capacity', room.capacity);
    formData.append('pricePerNight', room.pricePerNight.toString());
    if (room.photoUpload) {
      formData.append('photoUpload', room.photoUpload, room.photoUpload.name);
    }
    return this.http.post<void>(this.base, formData);
  }
}
