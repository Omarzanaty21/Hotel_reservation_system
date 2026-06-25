import { Component } from '@angular/core';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { Router } from '@angular/router';
import { RoomsService } from '../../../core/services/rooms.service';
import { CreateRoom, RoomCapacity } from '../../../models/room/room.model';
import { ApiError } from '../../../models/error.model';
import Swal from 'sweetalert2';

interface AddRoomForm {
  name: FormControl<string>;
  description: FormControl<string>;
  capacity: FormControl<RoomCapacity | ''>;
  pricePerNight: FormControl<number | null>;
}

@Component({
  selector: 'app-add-room',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './addRoom.component.html',
  styleUrl: './addRoom.component.css',
})
export class AddRoomComponent {
  form!: FormGroup<AddRoomForm>;
  submitting = false;
  photoUpload: File | null = null;

  readonly capacityOptions: RoomCapacity[] = ['Single', 'Double', 'Queen', 'King'];

  constructor(
    private readonly fb: FormBuilder,
    private readonly roomsService: RoomsService,
    private readonly router: Router,
  ) {
    this.form = this.fb.group<AddRoomForm>({
      name: this.fb.nonNullable.control('', [Validators.required]),
      description: this.fb.nonNullable.control('', [Validators.required]),
      capacity: this.fb.nonNullable.control('' as RoomCapacity | '', [
        Validators.required,
        Validators.pattern(/^(Single|Double|Queen|King)$/),
      ]),
      pricePerNight: this.fb.control<number | null>(null, [
        Validators.required,
        Validators.min(0.01),
      ]),
    });
  }

  // ── Typed getters ────────────────────────────────────────────────────────────

  get name(): FormControl<string> {
    return this.form.controls.name;
  }

  get description(): FormControl<string> {
    return this.form.controls.description;
  }

  get capacity(): FormControl<RoomCapacity | ''> {
    return this.form.controls.capacity;
  }

  get pricePerNight(): FormControl<number | null> {
    return this.form.controls.pricePerNight;
  }

  // ── File selection ────────────────────────────────────────────────────────────

  onFileChange(event: Event): void {
    const input = event.target as HTMLInputElement;
    this.photoUpload = input.files?.[0] ?? null;
  }

  // ── Submit ───────────────────────────────────────────────────────────────────

  onSubmit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.submitting = true;
    const raw = this.form.getRawValue();

    const payload: CreateRoom = {
      name: raw.name,
      description: raw.description,
      capacity: raw.capacity as RoomCapacity,
      pricePerNight: raw.pricePerNight!,
      photoUpload: this.photoUpload,
    };

    this.roomsService.createRoom(payload).subscribe({
      next: () => {
        this.submitting = false;
        Swal.fire({
          icon: 'success',
          title: 'Success',
          text: 'Room created successfully.',
          confirmButtonColor: '#22c55e',
        }).then(() => {
          this.router.navigate(['/rooms']);
        });
      },
      error: (err: ApiError) => {
        this.submitting = false;
        Swal.fire({
          icon: 'error',
          title: 'Error',
          text: err.message,
          confirmButtonColor: '#ef4444',
        });
      },
    });
  }
}
