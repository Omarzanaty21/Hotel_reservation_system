export type RoomCapacity = 'Single' | 'Double' | 'Queen' | 'King';

export interface Room {
  id: number;
  name: string;
  description: string;
  capacity: RoomCapacity;
  pricePerNight: number;
  photo: string;
}

export interface CreateRoom {
  name: string;
  description: string;
  capacity: RoomCapacity;
  pricePerNight: number;
  photoUpload: File | null;
  photo?: string | null;
}
