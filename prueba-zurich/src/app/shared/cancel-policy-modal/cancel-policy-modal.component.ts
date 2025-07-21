import { Component } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule  } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-cancel-policy-modal',
  standalone: true,
  templateUrl: './cancel-policy-modal.component.html',
  imports: [CommonModule, ReactiveFormsModule],
})
export class CancelPolicyModalComponent {
  cancelForm: FormGroup;
  loading = false;
  error: string | null = null;

  constructor(
    public activeModal: NgbActiveModal,
    private fb: FormBuilder
  ) {
    this.cancelForm = this.fb.group({
      motivo: ['', [Validators.required, Validators.minLength(10)]]
    });
  }

  get motivo() {
    return this.cancelForm.get('motivo');
  }

  confirm() {
    if (this.cancelForm.invalid) {
      return;
    }

    this.loading = true;
    this.error = null;
    this.activeModal.close(this.cancelForm.value.motivo);
  }
}