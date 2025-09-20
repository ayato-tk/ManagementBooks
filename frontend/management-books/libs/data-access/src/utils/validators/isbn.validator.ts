import { AbstractControl, ValidationErrors } from '@angular/forms';

export function isbnValidator(control: AbstractControl): ValidationErrors | null {
  const value = control.value?.toString() || '';
  if (value.length === 10 || value.length === 13) {
    return null; 
  }
  return { invalidIsbn: true };
}