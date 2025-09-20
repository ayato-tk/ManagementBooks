import { Route } from '@angular/router';
import { BookListComponent } from './containers/book-list/book-list.component';

export const booksRoutes: Route[] = [
  { path: '', component: BookListComponent }
];
