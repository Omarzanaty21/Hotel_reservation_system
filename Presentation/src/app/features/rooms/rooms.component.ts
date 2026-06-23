import { Component, OnInit, ChangeDetectorRef } from '@angular/core'; // 1. Import ChangeDetectorRef
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RoomsService } from '../../core/services/rooms.service';
import { Room } from '../../models/room/room.model';
import { RoomFilter } from '../../models/room/filter.model';
import { ApiError } from '../../models/error.model';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-rooms',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './rooms.component.html',
  styleUrls: ['./rooms.component.css'],
})
export class RoomsComponent implements OnInit {
  rooms: Room[] = [];
  defaultPhoto = '/images/hotel_default_pic.avif';
  loading = false;
  error: ApiError | null = null;

  filter: RoomFilter = { checkIn: null, checkOut: null, capacity: null };
  pageIndex = 0;
  pageSize = 12;
  totalCount = 0;

  get totalPages(): number {
    return Math.ceil(this.totalCount / this.pageSize);
  }

  // 2. Inject ChangeDetectorRef here
  constructor(private roomsService: RoomsService, private cdr: ChangeDetectorRef) {}

  ngOnInit(): void {
    this.loadRooms();
  }

  search(): void {
    this.pageIndex = 0;
    this.loadRooms();
  }

  goToPage(page: number): void {
    if (page < 0 || page >= this.totalPages) return;
    this.pageIndex = page;
    this.loadRooms();
  }

  private loadRooms(): void {
    this.loading = true;
    this.error = null;
    
    // Explicitly detect changes to show the "loading" spinner in HTML
    this.cdr.detectChanges(); 

    const payload = {
      checkIn: this.filter.checkIn || null,
      checkOut: this.filter.checkOut || null,
      capacity: this.filter.capacity || null,
    };

    this.roomsService.getAvailableRooms(payload, this.pageIndex, this.pageSize).subscribe({
      next: (res) => {
        this.rooms = res.items || [];
        this.totalCount = res.totalCount;
        this.loading = false;
        
        // 3. Force Angular to re-render the view
        this.cdr.detectChanges(); 
      },
      error: (err: ApiError) => {
        this.error = err;
        this.rooms = [];
        this.loading = false;
        
        // Force Angular to re-render the view with the error state
        this.cdr.detectChanges(); 
      },
    });
  }
}
