import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ReservationsService } from '../../../core/services/reservation.service';
import { ReservationDto } from '../../../models/reservation/reservation.model';
import { ReservationFilter } from '../../../models/reservation/reservation-filter.model';
import { ApiError } from '../../../models/error.model';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-reservations',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './reservations.component.html',
  styleUrls: ['./reservations.component.css'],
})
export class ReservationsComponent implements OnInit {
  reservations: ReservationDto[] = [];
  loading = false;
  error: ApiError | null = null;
  deletingId: number | null = null;

  filter: ReservationFilter = { searchQuery: null, createdAt: null };
  pageIndex = 0;
  pageSize = 5;
  totalCount = 0;

  get totalPages(): number {
    return Math.ceil(this.totalCount / this.pageSize);
  }

  constructor(
    private reservationsService: ReservationsService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.loadReservations();
  }

  search(): void {
    this.pageIndex = 0;
    this.loadReservations();
  }

  goToPage(page: number): void {
    if (page < 0 || page >= this.totalPages) return;
    this.pageIndex = page;
    this.loadReservations();
  }

  cancelReservation(reservationId: number): void {
    this.deletingId = reservationId;
    this.error = null;
    this.cdr.detectChanges();

    this.reservationsService.cancelReservation(reservationId).subscribe({
      next: () => {
        this.deletingId = null;
        Swal.fire({
          icon: 'success',
          title: 'Success',
          text: 'Reservation cancelled successfully.',
          confirmButtonColor: '#22c55e',
        }).then(() => {
          this.loadReservations();
        });
      },
      error: (err: ApiError) => {
        this.deletingId = null;
        this.error = err;
        this.cdr.detectChanges();
      },
    });
  }

  private loadReservations(): void {
    this.loading = true;
    this.error = null;
    this.cdr.detectChanges();

    this.reservationsService
      .getReservations(this.filter, this.pageIndex, this.pageSize)
      .subscribe({
        next: (res) => {
          this.reservations = res.items || [];
          this.totalCount = res.totalCount;
          this.loading = false;
          this.cdr.detectChanges();
        },
        error: (err: ApiError) => {
          this.error = err;
          this.reservations = [];
          this.loading = false;
          this.cdr.detectChanges();
        },
      });
  }
}
