import { Routes } from '@angular/router';
import { RoomsComponent } from './features/rooms/rooms.component';
import { CreateReservationComponent } from './features/reservations/createReservation/createReservation.component';
import { ReservationsComponent } from './features/reservations/reservations/reservations.component';
import { AddRoomComponent } from './features/rooms/addRoom/addRoom.component';

export const routes: Routes = [
	{ path: '', component: RoomsComponent },
	{ path: 'rooms', component: RoomsComponent },
	{ path: 'addRoom', component: AddRoomComponent },
	{ path: 'reservations', component: ReservationsComponent },
	{ path: 'reservations/create', component: CreateReservationComponent },
];
