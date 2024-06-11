import { Component, Inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

@Component({
	selector: 'app-mark-appointment-as-visited-dialog',
	templateUrl: './mark-appointment-as-visited-dialog.component.html',
})
export class MarkAppointmentAsVisitedDialogComponent {
	markAsVisitedForm: FormGroup;

	constructor(
		private fb: FormBuilder,
		public dialogRef: MatDialogRef<MarkAppointmentAsVisitedDialogComponent>,
		@Inject(MAT_DIALOG_DATA) public data: { appointmentId: number },
	) {
		this.markAsVisitedForm = this.fb.group({
			diagnosis: ['', Validators.required],
		});
	}

	onSubmit(): void {
		if (this.markAsVisitedForm.valid) {
			this.dialogRef.close(this.markAsVisitedForm.value);
		}
	}

	onCancel(): void {
		this.dialogRef.close();
	}
}
