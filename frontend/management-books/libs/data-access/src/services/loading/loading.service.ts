import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class LoadingService {
  private _loading$ = new BehaviorSubject<boolean>(false);
  loading$: Observable<boolean> = this._loading$.asObservable();

  show() { this._loading$.next(true); }
  hide() { this._loading$.next(false); }
}