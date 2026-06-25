import { Routes } from '@angular/router';
import { RoomsComponent } from './features/rooms/rooms.component';
import { CreateReservationComponent } from './features/reservations/createReservation/createReservation.component';
import { ReservationsComponent } from './features/reservations/reservations/reservations.component';
import { AddRoomComponent } from './features/rooms/addRoom/addRoom.component';
import { LoginComponent } from './features/auth/login/login.component';
import { authGuard } from './core/guards/auth.guard';

export const routes: Routes = [
  { path: '', component: RoomsComponent },
  { path: 'rooms', component: RoomsComponent },
  { path: 'login', component: LoginComponent },
  { path: 'addRoom', component: AddRoomComponent, canActivate: [authGuard] },
  { path: 'reservations', component: ReservationsComponent, canActivate: [authGuard] },
  { path: 'reservations/create', component: CreateReservationComponent },
];
