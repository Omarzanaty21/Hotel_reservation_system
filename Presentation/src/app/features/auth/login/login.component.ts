import { Component, ChangeDetectionStrategy, ChangeDetectorRef } from '@angular/core';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';
import { ApiError } from '../../../models/error.model';

interface LoginForm {
  userName: FormControl<string>;
  password: FormControl<string>;
}

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class LoginComponent {
  form: FormGroup<LoginForm>;
  submitting = false;
  error: ApiError | null = null;

  constructor(
    private readonly fb: FormBuilder,
    private readonly authService: AuthService,
    private readonly router: Router,
    private readonly cdr: ChangeDetectorRef,
  ) {
    this.form = this.fb.group<LoginForm>({
      userName: this.fb.nonNullable.control('', [Validators.required]),
      password: this.fb.nonNullable.control('', [Validators.required]),
    });
  }

  // ── Typed getters ────────────────────────────────────────────────────────────

  get userName(): FormControl<string> {
    return this.form.controls.userName;
  }

  get password(): FormControl<string> {
    return this.form.controls.password;
  }

  // ── Submit ───────────────────────────────────────────────────────────────────

  onSubmit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      this.cdr.markForCheck();
      return;
    }

    this.submitting = true;
    this.error = null;
    this.cdr.markForCheck();

    this.authService.login(this.form.getRawValue()).subscribe({
      next: () => {
        this.submitting = false;
        this.cdr.markForCheck();
        this.router.navigate(['/rooms']);
      },
      error: (err: ApiError) => {
        this.submitting = false;
        this.error = err;
        this.cdr.markForCheck();
      },
    });
  }
}
