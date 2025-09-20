import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { MatDialogRef } from '@angular/material/dialog';
import { MatInputModule } from '@angular/material/input';
import { BookChildDto, noWhitespaceValidator } from '@management-books/data-access';

@Component({
  selector: 'app-book-child-dialog',
  templateUrl: './book-child-dialog.component.html',
  styleUrls: ['./book-child-dialog.component.scss'],
  imports: [MatDialogModule, MatInputModule, ReactiveFormsModule, MatButtonModule],
})
export class BookChildDialogComponent implements OnInit {
  form: FormGroup;

  constructor(
    private fb: FormBuilder,
    public dialogRef: MatDialogRef<BookChildDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: BookChildDto,
  ) {}

  ngOnInit(): void {
    this.form = this.fb.group({
      id: [{ value: this.data?.id || null, disabled: !!this.data?.id }],
      name: [this.data?.name || '', [Validators.required, Validators.minLength(3), noWhitespaceValidator]],
    });
  }

  save() {
    if (this.form.valid) {
      this.dialogRef.close(this.form.value);
    }
  }

  close() {
    this.dialogRef.close();
  }
}
