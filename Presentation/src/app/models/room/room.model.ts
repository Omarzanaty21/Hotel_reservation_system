export interface Room {
  id: number;
  name: string;
  description: string;
  capacity: 'Single' | 'Double' | 'Queen' | 'King';
  pricePerNight: number;
  photo: string;
}


