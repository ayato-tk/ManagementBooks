import { Route } from '@angular/router';

export const mainRoutes: Route[] = [
  {
    path: 'book',
    loadChildren: () => import('./containers/books/books.route').then((m) => m.booksRoutes),
  },
  {
    path: 'genre',
    loadChildren: () => import('./containers/genre/genre.route').then((m) => m.genreRoute),
  },
  {
    path: 'publisher',
    loadChildren: () => import('./containers/publisher/publisher.route').then((m) => m.publisherRoute),
  },
];
