import { Routes } from '@angular/router';
import { RoomsComponent } from './features/rooms/rooms.component';
import { CreateReservationComponent } from './features/reservations/createReservation/createReservation.component';

export const routes: Routes = [
	{ path: '', component: RoomsComponent },
	{ path: 'rooms', component: RoomsComponent },
	{ path: 'reservations/create', component: CreateReservationComponent },
];
