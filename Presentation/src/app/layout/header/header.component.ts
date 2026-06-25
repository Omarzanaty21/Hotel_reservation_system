import { ChangeDetectionStrategy, Component, ChangeDetectorRef } from '@angular/core';
import { RouterModule, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../core/services/auth.service';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [RouterModule, CommonModule],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class HeaderComponent {
  menuOpen = false;

  constructor(
    readonly authService: AuthService,
    private readonly router: Router,
    private readonly cdr: ChangeDetectorRef,
  ) {}

  logout(): void {
    this.authService.logout().subscribe({
      next: () => {
        this.cdr.markForCheck();
        this.router.navigate(['/rooms']);
      },
      error: () => {
        // Even on error, clear the token and redirect
        localStorage.removeItem('auth_token');
        this.cdr.markForCheck();
        this.router.navigate(['/login']);
      },
    });
  }
}
