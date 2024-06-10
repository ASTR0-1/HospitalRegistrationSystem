import { Component, OnInit } from '@angular/core';
import { AppointmentService } from '../services/appointment.service';
import { AppointmentDto } from '../entities/appointment/appointmentDto';
import { PageEvent } from '@angular/material/paginator';
import { AuthenticationService } from '../services/authentication.service';
import { Roles } from '../constants/role.constants';
import { MatDialog } from '@angular/material/dialog';
import { MarkAppointmentAsVisitedDialogComponent } from './mark-appointment-as-visited-dialog/mark-appointment-as-visited-dialog.component';

@Component({
	selector: 'app-missed-appointments',
	templateUrl: './missed-appointments.component.html',
	styleUrls: ['./missed-appointments.component.css'],
})
export class MissedAppointmentsComponent implements OnInit {
	appointments: AppointmentDto[] = [];
	totalAppointments = 0;
	pageSize = 8;
	pageIndex = 0;
	userId!: string;
	currentUserId: number = +localStorage.getItem('userId')!;

	constructor(
		private appointmentService: AppointmentService,
		private authService: AuthenticationService,
		private dialog: MatDialog,
	) {}

	ngOnInit(): void {
		if (this.authService.hasRole(Roles.RECEPTIONIST)) {
			const queryParams = new URLSearchParams(window.location.search);
			this.userId =
				queryParams.get('userId') ||
				localStorage.getItem('userId') ||
				'';
		} else {
			this.userId = localStorage.getItem('userId') || '';
		}

		this.fetchAppointments();
	}

	fetchAppointments(pageEvent?: PageEvent): void {
		const pagingParameters = {
			pageNumber: pageEvent
				? pageEvent.pageIndex + 1
				: this.pageIndex + 1,
			pageSize: pageEvent ? pageEvent.pageSize : this.pageSize,
		};

		this.appointmentService
			.getMissedByUserId(pagingParameters, +this.userId)
			.subscribe((response) => {
				this.appointments = response.body!;
				const paginationData = JSON.parse(
					response.headers.get('X-Pagination')!,
				);
				this.totalAppointments = paginationData.totalCount;
				this.pageSize = paginationData.pageSize;
				this.pageIndex = paginationData.currentPage - 1;
			});
	}

	openMarkAsVisitedDialog(appointmentId: number): void {
		const dialogRef = this.dialog.open(
			MarkAppointmentAsVisitedDialogComponent,
			{
				data: { appointmentId },
				width: '500px',
			},
		);

		dialogRef.afterClosed().subscribe((result) => {
			if (result) {
				this.appointmentService
					.markAsVisited(appointmentId, result.diagnosis)
					.subscribe(() => {
						this.fetchAppointments(); // Refresh the list after marking as visited
					});
			}
		});
	}
}
