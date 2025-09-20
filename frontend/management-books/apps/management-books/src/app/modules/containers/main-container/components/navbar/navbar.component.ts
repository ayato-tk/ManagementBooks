import { Component } from '@angular/core';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatMenuModule } from '@angular/material/menu';
import { MatButtonModule } from '@angular/material/button';
import { RouterModule } from '@angular/router';
import { BookService, LoadingService, UserService } from '@management-books/data-access';
import { finalize } from 'rxjs';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss'],
  imports: [MatToolbarModule, MatMenuModule, MatButtonModule, RouterModule],
  providers: [BookService, LoadingService, ToastrService, UserService],
})
export class NavbarComponent {
  constructor(
    private bookService: BookService,
    private loadingService: LoadingService,
    private userService: UserService,
    private toastr: ToastrService
  ) {}

  logout() {
    this.userService.logoff();
  }

  downloadBooksReport() {
    this.loadingService.show();
    this.bookService
      .getBookReport()
      .pipe(finalize(() => this.loadingService.hide()))
      .subscribe({
        next: (blob) => {
          const url = window.URL.createObjectURL(blob);
          const a = document.createElement('a');
          a.href = url;
          a.download = 'relatorio-livros.pdf';
          a.click();
          window.URL.revokeObjectURL(url);
          this.toastr.success('Relatório emitido com sucesso!');
        },
        error: (e) => {
          this.toastr.error(e.error[0].message, 'Houve um erro ao tentar gerar o relatório!');
        },
      });
  }
}
