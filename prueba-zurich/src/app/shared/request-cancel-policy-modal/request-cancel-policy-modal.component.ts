import { Component } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-request-cancel-policy-modal',
  standalone: true,
  templateUrl: './request-cancel-policy-modal.component.html',
  imports: [CommonModule],
})
export class RequestCancelPolicyModalComponent {
  loading = false;

  constructor( public activeModal: NgbActiveModal) {}

  confirm(): void {
    this.loading = true;
    this.activeModal.close(true);
  }

  cerrar(): void {
    this.activeModal.dismiss(false);
  }
}