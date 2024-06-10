import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { FeedbackDto } from 'src/app/entities/feedback/feedbackDto';
import { FeedbackService } from 'src/app/services/feedback.service';

@Component({
	selector: 'app-feedback-dialog',
	templateUrl: './feedback-list-dialog.component.html',
})
export class FeedbackListDialogComponent {
	feedbacks: FeedbackDto[] = [];

	constructor(
		public dialogRef: MatDialogRef<FeedbackListDialogComponent>,
		@Inject(MAT_DIALOG_DATA) public data: { doctorId: number },
		private feedbackService: FeedbackService,
	) {}

	ngOnInit(): void {
		this.loadFeedbacks();
	}

	loadFeedbacks(): void {
		const pagingParameters = { pageNumber: 1, pageSize: 10 };
		this.feedbackService
			.getAll(pagingParameters, this.data.doctorId)
			.subscribe((feedbacks) => {
				this.feedbacks = feedbacks;
			});
	}

	onClose(): void {
		this.dialogRef.close();
	}

	starsArray(rating: number): string[] {
		const fullStars = Math.floor(rating);
		const halfStar = rating % 1 >= 0.5 ? 1 : 0;
		const emptyStars = 5 - fullStars - halfStar;

		return [
			...Array(fullStars).fill('full'),
			...Array(halfStar).fill('half'),
			...Array(emptyStars).fill('empty'),
		];
	}
}
