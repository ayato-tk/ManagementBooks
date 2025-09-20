import {
  AfterViewInit,
  Component,
  EventEmitter,
  Input,
  OnInit,
  Output,
  ViewChild,
} from '@angular/core';
import { ApiResponseDto } from '@management-books/data-access';
import { MatSelect, MatSelectModule } from '@angular/material/select';
import { MatOptionModule } from '@angular/material/core';
import { ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { finalize, take } from 'rxjs/operators';
import { Observable } from 'rxjs';

@Component({
  selector: 'ui-paged-select',
  templateUrl: './paged-select.component.html',
  styleUrls: ['./paged-select.component.scss'],
  imports: [MatFormFieldModule, MatSelectModule, MatOptionModule, ReactiveFormsModule],
})
export class PagedSelectComponent<T> implements AfterViewInit, OnInit {
  @Input() label: string = '';
  @Input() control: any;
  @Input() pageSize = 10;
  @Input() fetchPage!: (page: number, pageSize: number) => Observable<ApiResponseDto<T>>;

  @Output() selected = new EventEmitter<T>();

  @ViewChild('select') select!: MatSelect;

  items: { value: any; label: string }[] = [];
  page = 0;
  totalPages = 0;
  loading = false;

  ngOnInit(): void {
    this.resetAndLoad();
  }

  ngAfterViewInit(): void {}

  onOpenedChange(opened: boolean) {
    if (opened) {
      setTimeout(() => {
        const panel = this.select.panel?.nativeElement;
        if (panel) {
          panel.addEventListener('scroll', this.onScroll.bind(this));
        }
      });
    }
  }

  onScroll(event: any) {
    const panel = event.target;
    const atBottom = panel.scrollTop + panel.clientHeight >= panel.scrollHeight - 10;
    if (atBottom) {
      this.loadMore();
    }
  }

  private resetAndLoad() {
    this.items = [];
    this.page = 0;
    this.totalPages = 0;
    this.loadMore();
  }

  loadMore() {
    if (this.loading) return;
    if (this.totalPages > 0 && this.page >= this.totalPages) return;

    this.loading = true;
    this.page++;

    this.fetchPage(this.page, this.pageSize)
      .pipe(
        take(1),
        finalize(() => (this.loading = false)),
      )
      .subscribe((res) => {
        this.items = [
          ...this.items,
          ...res.data.map((d: any) => ({ value: (d as any).id, label: (d as any).name })),
        ];
        this.totalPages = res.totalPages;
      });
  }
}
