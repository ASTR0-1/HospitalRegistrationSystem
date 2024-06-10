import { Component, OnInit } from '@angular/core';
import { AppointmentService } from '../services/appointment.service';
import { AppointmentDto } from '../entities/appointment/appointmentDto';
import { PageEvent } from '@angular/material/paginator';
import { AuthenticationService } from '../services/authentication.service';
import { Roles } from '../constants/role.constants';

@Component({
	selector: 'app-scheduled-appointments',
	templateUrl: './scheduled-appointments.component.html',
	styleUrls: ['./scheduled-appointments.component.css'],
})
export class ScheduledAppointmentsComponent implements OnInit {
	appointments: AppointmentDto[] = [];
	totalAppointments = 0;
	pageSize = 8;
	pageIndex = 0;
	userId!: string;
	currentUserId: number = +localStorage.getItem('userId')!;
	isReceptionist: boolean = false;

	constructor(
		private appointmentService: AppointmentService,
		private authService: AuthenticationService
	) {}

	ngOnInit(): void {
		const queryParams = new URLSearchParams(window.location.search);
		this.isReceptionist = this.authService.hasRole(Roles.RECEPTIONIST) && queryParams.has('userId');
		
		if (this.isReceptionist) {
			this.userId =
				queryParams.get('userId') ||
				localStorage.getItem('userId') ||
				'';
			this.pageSize = 4;
		} else {
			this.userId = localStorage.getItem('userId') || '';
		}

		console.log(this.userId)

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
			.getIncomingByUserId(pagingParameters, +this.userId)
			.subscribe((response) => {
				this.appointments = response.body!;
				console.log(response.body)
				const paginationData = JSON.parse(
					response.headers.get('X-Pagination')!
				);
				this.totalAppointments = paginationData.totalCount;
				this.pageSize = paginationData.pageSize;
				this.pageIndex = paginationData.currentPage - 1;
			});
	}
}
