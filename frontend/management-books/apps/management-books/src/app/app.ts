import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { SpinnerComponent } from '@management-books/ui';

@Component({
  imports: [RouterModule, SpinnerComponent],
  selector: 'app-root',
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App {
  protected title = 'management-books';
}
