import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

@Component({
	selector: 'app-appointment-dialog',
	templateUrl: './create-appointment-dialog.component.html',
})
export class CreateAppointmentDialogComponent {
	constructor(
		public dialogRef: MatDialogRef<CreateAppointmentDialogComponent>,
		@Inject(MAT_DIALOG_DATA) public data: { day: Date; hour: number },
	) {}

	confirm(): void {
		this.dialogRef.close(true);
	}

	onCancel(): void {
		this.dialogRef.close(false);
	}
}
