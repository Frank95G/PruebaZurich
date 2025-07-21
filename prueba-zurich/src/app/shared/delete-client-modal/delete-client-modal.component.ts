import { Component } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-delete-client-modal',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './delete-client-modal.component.html'
})
export class DeleteClientModalComponent {
  loading = false;

  constructor(public activeModal: NgbActiveModal) {}

  confirm(): void {
    this.loading = true;
    this.activeModal.close(true);
  }

  cancel(): void {
    this.activeModal.dismiss(false);
  }
}