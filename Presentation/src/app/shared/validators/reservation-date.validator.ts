import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

/**
 * Cross-field validator: checkOut must be strictly after checkIn.
 * Apply to the FormGroup that contains both controls.
 */
export function checkOutAfterCheckIn(): ValidatorFn {
  return (group: AbstractControl): ValidationErrors | null => {
    const checkIn = group.get('checkIn')?.value as string | null;
    const checkOut = group.get('checkOut')?.value as string | null;

    if (!checkIn || !checkOut) {
      return null; // Let required validators handle empty values
    }

    return new Date(checkOut) > new Date(checkIn)
      ? null
      : { checkOutBeforeCheckIn: true };
  };
}

/**
 * Validator: date value must be today or in the future.
 */
export function notInPast(): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    if (!control.value) {
      return null;
    }
    const today = new Date();
    today.setHours(0, 0, 0, 0);
    const selected = new Date(control.value);
    return selected >= today ? null : { pastDate: true };
  };
}
