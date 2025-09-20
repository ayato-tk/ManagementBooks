import { Route } from '@angular/router';
import { MainContainerComponent } from './modules/containers/main-container/container/main-container.component';
import { AuthGuard } from './auth/guards/auth.guard';

export const appRoutes: Route[] = [
  {
    path: '',
    redirectTo: 'book',
    pathMatch: 'full',
  },
  {
    path: 'auth',
    loadChildren: () => import('./auth/auth.route').then((m) => m.authRoutes),
  },
  {
    path: '',
    component: MainContainerComponent,
    loadChildren: () => import('./modules/main.route').then((m) => m.mainRoutes),
    canActivate: [AuthGuard]
  },
];
