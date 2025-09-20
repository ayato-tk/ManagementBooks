import { Component, OnInit, ViewChild } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatDialog } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatPaginator, MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatTableModule } from '@angular/material/table';
import {
  ApiPaginatedFilterRequest,
  BookChildDto,
  GenreService,
  LoadingService,
} from '@management-books/data-access';
import { debounceTime, distinctUntilChanged, finalize, Subject } from 'rxjs';
import { BookChildDialogComponent } from '../../../main-container/components/book-child-dialog/book-child-dialog.component';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-genre-list',
  templateUrl: './genre-list.component.html',
  styleUrls: ['./genre-list.component.scss'],
  imports: [MatTableModule, MatInputModule, MatPaginatorModule, MatIconModule, MatButtonModule],
  providers: [GenreService],
})
export class GenreListComponent implements OnInit {
  displayedColumns: string[] = ['id', 'name', 'actions'];
  dataSource: BookChildDto[] = [];

  totalItems = 0;
  pageSize = 10;
  pageIndex = 0;
  searchTerm = '';

  private searchSubject = new Subject<string>();

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  constructor(
    private genreService: GenreService,
    private loadingService: LoadingService,
    private dialog: MatDialog,
    private toastr: ToastrService,
  ) {}

  ngOnInit(): void {
    this.loadGenres(1, this.pageSize);
    this.searchSubject.pipe(debounceTime(300), distinctUntilChanged()).subscribe((term) => {
      this.searchTerm = term;
      this.pageIndex = 0;
      this.loadGenres(1, this.pageSize, this.searchTerm);
    });
  }

  loadGenres(page: number, pageSize: number, search: string = '') {
    let filter: ApiPaginatedFilterRequest = {
      page,
      pageSize,
    };

    if (search.trim()) {
      filter.search = search.trim();
    }

    this.loadingService.show();
    this.genreService
      .getPaginatedGenre(filter)
      .pipe(finalize(() => this.loadingService.hide()))
      .subscribe({
        next: (res) => {
          this.dataSource = res.data;
          this.totalItems = res.totalItems;
          this.pageSize = res.pageSize;
          this.pageIndex = res.page - 1;
        },
      });
  }

  openNewDialog() {
    const dialogRef = this.dialog.open(BookChildDialogComponent, {
      width: '400px',
      data: {} as BookChildDto,
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.loadingService.show();
        this.genreService
          .createGenre({ name: result.name })
          .pipe(finalize(() => this.loadingService.hide()))
          .subscribe({
            next: () => {
              this.loadGenres(this.pageIndex + 1, this.pageSize);
              this.toastr.success('Valor criado com sucesso!');
            },
            error: (err) => {
              this.toastr.error(err.error[0].message);
            },
          });
      }
    });
  }

  openEditDialog(genre: BookChildDto) {
    const dialogRef = this.dialog.open(BookChildDialogComponent, {
      width: '400px',
      data: { id: genre.id, name: genre.name } as BookChildDto,
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.loadingService.show();
        this.genreService
          .updateGenre({ id: genre.id, name: result.name })
          .pipe(finalize(() => this.loadingService.hide()))
          .subscribe({
            next: () => {
              this.toastr.success('Valor alterado com sucesso!');
              this.loadGenres(this.pageIndex + 1, this.pageSize);
            },
            error: (err) => {
              this.toastr.error(err.error[0].message);
            },
          });
      }
    });
  }

  deleteGenre(genre: BookChildDto) {
    this.loadingService.show();
    this.genreService
      .deleteGenre(genre.id!)
      .pipe(finalize(() => this.loadingService.hide()))
      .subscribe({
        next: () => {
          this.toastr.success('Valor removido com sucesso!');
          this.loadGenres(this.pageIndex + 1, this.pageSize);
        },
        error: (e) => {
          this.toastr.error(e.error[0].message, 'Houve um erro ao tentar remover!');
        },
      });
  }

  onPageChange(event: PageEvent) {
    this.loadGenres(event.pageIndex + 1, event.pageSize);
  }

  applyFilter(event: Event) {
    const value = (event.target as HTMLInputElement).value;
    this.searchSubject.next(value);
  }
}
