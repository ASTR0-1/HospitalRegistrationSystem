import { Component, OnInit } from '@angular/core';
import { AppointmentService } from '../services/appointment.service';
import { AppointmentDto } from '../entities/appointment/appointmentDto';
import { PageEvent } from '@angular/material/paginator';
import { AuthenticationService } from '../services/authentication.service';
import { Roles } from '../constants/role.constants';

@Component({
	selector: 'app-visited-appointments',
	templateUrl: './visited-appointments.component.html',
	styleUrls: ['./visited-appointments.component.css'],
})
export class VisitedAppointmentsComponent implements OnInit {
	appointments: AppointmentDto[] = [];
	totalAppointments = 0;
	pageSize = 8;
	pageIndex = 0;
	userId!: string;
	currentUserId: number = +localStorage.getItem('userId')!;

	constructor(
		private appointmentService: AppointmentService,
		private authService: AuthenticationService
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
			.getVisitedByUserId(pagingParameters, +this.userId)
			.subscribe((response) => {
				this.appointments = response.body!;
        for (let i = 0; i < 7; i++) {
              this.appointments.push(this.appointments[0]);
        }
        
				const paginationData = JSON.parse(
					response.headers.get('X-Pagination')!
				);

				this.totalAppointments = paginationData.totalCount;
				this.pageSize = paginationData.pageSize;
				this.pageIndex = paginationData.currentPage - 1;
			});
	}
}
