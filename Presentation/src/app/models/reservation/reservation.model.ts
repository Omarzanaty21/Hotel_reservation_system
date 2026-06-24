export interface RoomDto {
  id: number;
  name: string;
  description: string;
  capacity: 'Single' | 'Double' | 'Queen' | 'King';
  pricePerNight: number;
  photo: string;
}

export interface ReservationDto {
  id: number;
  checkIn: string;
  checkOut: string;
  room: RoomDto;
  guestName: string;
  guestEmail: string;
  guestNumber: string;
}

export interface ReservationCreateRequest {
  roomId: number;
  checkIn: string; // ISO date string: YYYY-MM-DD
  checkOut: string; // ISO date string: YYYY-MM-DD
  guestName: string;
  guestEmail: string;
  guestNumber: string;
}
