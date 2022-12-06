import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { AppointmentService } from 'src/app/services/appointment.service';

@Component({
	selector: 'mark-appointment',
	templateUrl: './mark.appointment.component.html',
	styleUrls: ['./mark.appointment.component.css'],
	providers: [AppointmentService],
})
export class MarkAppointmentComponent implements OnInit {
	appointmentId: number = 0;
	fullName: string = '';

	constructor(
		private appointmentService: AppointmentService,
		private route: ActivatedRoute,
        private location: Location
	) {}

	ngOnInit(): void {
		this.route.queryParams.subscribe(
			(params) => (this.appointmentId = params['appointmentId'])
		);

		this.route.queryParams.subscribe(
			(params) => (this.fullName = params['fullName'])
		);
	}

	markAsVisited(diagnosis: string) {
		this.appointmentService
			.putAppointment(this.appointmentId, diagnosis)
			.subscribe({
				error: (err) => console.log(err),
			});

        this.location.back();
	}
}
