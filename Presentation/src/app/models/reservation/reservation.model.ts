export interface ReservationCreateRequest {
  roomId: number;
  checkIn: string; // ISO date string: YYYY-MM-DD
  checkOut: string; // ISO date string: YYYY-MM-DD
}
