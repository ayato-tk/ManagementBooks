import { Component } from '@angular/core';
import { Observable } from 'rxjs';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { LoadingService } from '@management-books/data-access';
import { AsyncPipe } from '@angular/common';

@Component({
  selector: 'app-spinner',
  imports: [MatProgressSpinnerModule, AsyncPipe],
  template: `
    @if(loading$ | async) {
      <div class="spinner-backdrop">
        <mat-spinner></mat-spinner>
      </div>
    }
  `,
  styleUrls: ['./spinner.component.scss']
})
export class SpinnerComponent {
  loading$: Observable<boolean>;

  constructor(private loadingService: LoadingService) {
    this.loading$ = this.loadingService.loading$;
  }
}