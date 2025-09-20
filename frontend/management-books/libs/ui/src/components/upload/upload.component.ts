import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'ui-upload',
  templateUrl: './upload.component.html',
  styleUrls: ['./upload.component.scss']
})
export class UploadComponent {
  @Input() initialImage: string | null = null;
  @Output() fileChanged = new EventEmitter<string>();

  imagePreview: string | null = null;

  ngOnInit() {
    this.imagePreview = this.initialImage ?? null;
  }

  onFileSelected(event: Event) {
    const file = (event.target as HTMLInputElement).files?.[0];
    if (!file) return;

    const reader = new FileReader();
    reader.onload = () => {
      this.imagePreview = reader.result as string;
      this.fileChanged.emit(this.imagePreview);
    };
    reader.readAsDataURL(file);
  }

  removeImage() {
    this.imagePreview = null;
    this.fileChanged.emit('');
  }
}
