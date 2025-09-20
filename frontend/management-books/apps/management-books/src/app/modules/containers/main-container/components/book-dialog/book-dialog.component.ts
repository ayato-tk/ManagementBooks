import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatInputModule } from '@angular/material/input';
import {
  BookRequestDto,
  GenreService,
  isbnValidator,
  noWhitespaceValidator,
  PublisherService,
} from '@management-books/data-access';
import { MatSelectModule } from '@angular/material/select';
import { MatOptionModule } from '@angular/material/core';
import { NgScrollbarModule } from 'ngx-scrollbar';
import { PagedSelectComponent, UploadComponent } from '@management-books/ui';

@Component({
  selector: 'app-book-dialog',
  templateUrl: './book-dialog.component.html',
  styleUrls: ['./book-dialog.component.scss'],
  imports: [
    MatDialogModule,
    MatInputModule,
    ReactiveFormsModule,
    MatButtonModule,
    MatOptionModule,
    MatSelectModule,
    NgScrollbarModule,
    PagedSelectComponent,
    UploadComponent
  ],
  providers: [GenreService, PublisherService],
})
export class BookDialogComponent implements OnInit {
  form: FormGroup;

  constructor(
    private fb: FormBuilder,
    private genreService: GenreService,
    private publisherService: PublisherService,
    public dialogRef: MatDialogRef<BookDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: BookRequestDto,
  ) {}

  ngOnInit(): void {
    this.form = this.fb.group({
      id: [{ value: this.data?.id || null, disabled: !!this.data?.id }],
      title: [
        this.data?.title || '',
        [Validators.required, Validators.minLength(3), noWhitespaceValidator],
      ],
      isbn: [this.data?.isbn || '', [Validators.required, isbnValidator]],
      author: [
        this.data?.author || '',
        [Validators.required, Validators.minLength(3), noWhitespaceValidator],
      ],
      synopsis: [this.data?.synopsis || ''],
      genreId: [this.data?.genreId || '', Validators.required],
      publisherId: [this.data?.publisherId || '', Validators.required],
      coverImagePath: [this.data?.coverImagePath || ''],
    });
  }

  fetchGenrePage(page: number, pageSize: number) {
    return this.genreService.getPaginatedGenre({ page, pageSize });
  }

  fetchPublisherPage(page: number, pageSize: number) {
    return this.publisherService.getPaginatedPublisher({ page, pageSize });
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