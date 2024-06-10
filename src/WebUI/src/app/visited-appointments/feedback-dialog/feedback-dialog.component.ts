import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { FeedbackService } from 'src/app/services/feedback.service';

@Component({
  selector: 'app-feedback-dialog',
  templateUrl: './feedback-dialog.component.html',
})
export class FeedbackDialogComponent implements OnInit {
  feedbackForm: FormGroup;
  ratings: number[] = [1, 2, 3, 4, 5];

  constructor(
    private fb: FormBuilder,
    private feedbackService: FeedbackService,
    public dialogRef: MatDialogRef<FeedbackDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { appointmentId: number }
  ) {
    this.feedbackForm = this.fb.group({
      rating: [null, Validators.required],
      text: ['', Validators.required]
    });
  }

  ngOnInit(): void {}

  onSubmit(): void {
    if (this.feedbackForm.valid) {
      const feedback = {
        ...this.feedbackForm.value,
        appointmentId: this.data.appointmentId,
        applicationUserId: localStorage.getItem('userId')
      };

      this.feedbackService.addNew(feedback).subscribe(() => {
        this.dialogRef.close(true);
      });
    }
  }

  onCancel(): void {
    this.dialogRef.close(false);
  }
}
