import { Component, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ReservationCreateRequest } from '../../../models/reservation/reservation.model';
import {
  checkOutAfterCheckIn,
  notInPast,
} from '../../../shared/validators/reservation-date.validator';
import { ReservationsService } from '../../../core/services/reservation.service';
import { ApiError } from '../../../models/error.model';
import Swal from 'sweetalert2';

interface ReservationForm {
  roomId: FormControl<number>;
  checkIn: FormControl<string>;
  checkOut: FormControl<string>;
}

@Component({
  selector: 'app-create-reservation',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './createReservation.component.html',
  styleUrl: './createReservation.component.css',
})
export class CreateReservationComponent implements OnInit {
  form!: FormGroup<ReservationForm>;
  roomName = '';
  capacity = '';

  /** Minimum selectable date for the date inputs (today). */
  readonly todayIso: string = new Date().toISOString().split('T')[0];

  constructor(
    private readonly fb: FormBuilder,
    private readonly route: ActivatedRoute,
    private reservationsService: ReservationsService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.form = this.fb.group<ReservationForm>(
      {
        roomId: this.fb.nonNullable.control(0, [
          Validators.required,
          Validators.min(1),
        ]),
        checkIn: this.fb.nonNullable.control('', [
          Validators.required,
          notInPast(),
        ]),
        checkOut: this.fb.nonNullable.control('', [Validators.required])
      },
      { validators: checkOutAfterCheckIn() },
    );

    this.route.queryParams.subscribe((params) => {
      const id = +params['roomId'];
      if (id > 0) {
        this.roomId.setValue(id);
      }
      this.roomName = params['roomName'];
      this.capacity = params['capacity'];
    });
  }

  // ── Typed getters ────────────────────────────────────────────────────────────

  get roomId(): FormControl<number> {
    return this.form.controls.roomId;
  }

  get checkIn(): FormControl<string> {
    return this.form.controls.checkIn;
  }

  get checkOut(): FormControl<string> {
    return this.form.controls.checkOut;
  }


  // ── Submit ───────────────────────────────────────────────────────────────────

  onSubmit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const payload: ReservationCreateRequest = this.form.getRawValue();
    
    this.reservationsService.createReservation(payload).subscribe({
      next: (res) => {
        Swal.fire({
          icon: 'success',
          title: 'Success',
          text: 'Reservation created successfully!',
          confirmButtonColor: '#22c55e'
        });

        this.router.navigate(['/rooms']);
      },
      error: (err: ApiError) => {
        Swal.fire({
          icon: 'error',
          title: 'Error',
          text: err.message,
          confirmButtonColor: '#ef4444'
        });
      },
    });
  }
}
