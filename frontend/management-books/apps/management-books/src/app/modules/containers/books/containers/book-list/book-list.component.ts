import { Component, OnInit, ViewChild } from '@angular/core';
import { MatTableModule } from '@angular/material/table';
import {
  ApiPaginatedFilterRequest,
  BookRequestDto,
  BookResponseDto,
  BookService,
  GenreService,
  LoadingService,
  PublisherService,
} from '@management-books/data-access';
import { MatPaginator, MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { debounceTime, distinctUntilChanged, finalize, Subject } from 'rxjs';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { BookDialogComponent } from '../../../main-container/components/book-dialog/book-dialog.component';

@Component({
  selector: 'app-book-list',
  templateUrl: './book-list.component.html',
  styleUrls: ['./book-list.component.scss'],
  imports: [
    MatTableModule,
    MatInputModule,
    MatPaginatorModule,
    MatIconModule,
    MatButtonModule,
    MatCardModule,
    MatDialogModule,
  ],
  providers: [BookService, GenreService, PublisherService],
})
export class BookListComponent implements OnInit {
  displayedColumns: string[] = [
    'id',
    'title',
    'isbn',
    'author',
    'publisherId',
    'genreId',
    'actions',
  ];
  dataSource: BookResponseDto[] = [];

  totalItems = 0;
  pageSize = 10;
  pageIndex = 0;
  searchTerm = '';

  genreCache = new Map<number, string>();
  publisherCache = new Map<number, string>();

  private searchSubject = new Subject<string>();

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  constructor(
    private bookService: BookService,
    private loadingService: LoadingService,
    private dialog: MatDialog,
    private toastr: ToastrService,
    private genreService: GenreService,
    private publisherService: PublisherService,
  ) {}

  ngOnInit(): void {
    this.loadBooks(1, this.pageSize);
    this.searchSubject.pipe(debounceTime(300), distinctUntilChanged()).subscribe((term) => {
      this.searchTerm = term;
      this.pageIndex = 0;
      this.loadBooks(1, this.pageSize, this.searchTerm);
    });
  }

  loadBooks(page: number, pageSize: number, search: string = '') {
    let filter: ApiPaginatedFilterRequest = {
      page,
      pageSize,
    };

    if (search.trim()) {
      filter.search = search.trim();
    }

    this.loadingService.show();
    this.bookService
      .getPaginatedBooks(filter)
      .pipe(finalize(() => this.loadingService.hide()))
      .subscribe({
        next: (res) => {
          this.dataSource = res.data;
          this.totalItems = res.totalItems;
          this.pageSize = res.pageSize;
          this.pageIndex = res.page - 1;

          this.dataSource.forEach((book) => {
            if (book.genreId) this.loadGenreName(book.genreId);
            if (book.publisherId) this.loadPublisherName(book.publisherId);
          });
        },
      });
  }

  openNewDialog() {
    const dialogRef = this.dialog.open(BookDialogComponent, {
      width: '400px',
      data: {} as BookRequestDto,
    });

    dialogRef.afterClosed().subscribe((result: BookRequestDto) => {
      if (result) {
        this.loadingService.show();
        this.bookService
          .createBook({ ...result })
          .pipe(finalize(() => this.loadingService.hide()))
          .subscribe({
            next: () => {
              this.loadBooks(this.pageIndex + 1, this.pageSize);
              this.toastr.success('Valor criado com sucesso!');
            },
            error: (err) => {
              this.toastr.error(err.error[0].message);
            },
          });
      }
    });
  }

  getGenreName(id: number): string {
    return this.genreCache.get(id) || 'Carregando...';
  }

  getPublisherName(id: number): string {
    return this.publisherCache.get(id) || 'Carregando...';
  }

  loadGenreName(id: number) {
    if (!this.genreCache.has(id)) {
      this.genreService.getGenre(id).subscribe((res) => {
        this.genreCache.set(id, res.name);
      });
    }
  }

  loadPublisherName(id: number) {
    if (!this.publisherCache.has(id)) {
      this.publisherService.getPublisher(id).subscribe((res) => {
        this.publisherCache.set(id, res.name);
      });
    }
  }

  openEditDialog(book: BookRequestDto) {
    const dialogRef = this.dialog.open(BookDialogComponent, {
      width: '400px',
      data: { ...book } as BookRequestDto,
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.loadingService.show();
        this.bookService
          .updateBook({ id: book.id, ...result })
          .pipe(finalize(() => this.loadingService.hide()))
          .subscribe({
            next: () => {
              this.toastr.success('Valor alterado com sucesso!');
              this.loadBooks(this.pageIndex + 1, this.pageSize);
            },
            error: (err) => {
              this.toastr.error(err.error[0].message);
            },
          });
      }
    });
  }

  deleteBook(book: BookRequestDto) {
    this.loadingService.show();
    this.bookService
      .deleteBook(book.id!)
      .pipe(finalize(() => this.loadingService.hide()))
      .subscribe({
        next: () => {
          this.toastr.success('Livro removido com sucesso!');
          this.loadBooks(this.pageIndex + 1, this.pageSize);
        },
        error: (e) => {
          this.toastr.error(e.error[0].message, 'Houve um erro ao tentar remover!');
        },
      });
  }

  onPageChange(event: PageEvent) {
    this.loadBooks(event.pageIndex + 1, event.pageSize);
  }

  applyFilter(event: Event) {
    const value = (event.target as HTMLInputElement).value;
    this.searchSubject.next(value);
  }
}
