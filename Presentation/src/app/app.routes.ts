import { Routes } from '@angular/router';
import { RoomsComponent } from './features/rooms/rooms.component';

export const routes: Routes = [
	{ path: '', component: RoomsComponent },
	{ path: 'rooms', component: RoomsComponent }
];
