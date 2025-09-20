import { ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { BookChildDialogComponent } from './book-child-dialog.component';

describe('BookChildDialogComponent', () => {
  let component: BookChildDialogComponent;
  let fixture: ComponentFixture<BookChildDialogComponent>;

  beforeEach(() => {
    fixture = TestBed.createComponent(BookChildDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
