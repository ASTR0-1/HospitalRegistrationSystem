import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { ClientAppointment } from 'src/app/entities/clientAppointment';
import { AppointmentService } from 'src/app/services/appointment.service';

@Component({
	selector: 'client-appointments',
	templateUrl: './client.appointments.component.html',
	styleUrls: ['./client.appointments.component.css'],
	providers: [AppointmentService],
})
export class ClientAppointmentsComponent implements OnInit {
	clientId: number = 0;
    clientFullName: string = "";
	clientAppointments: ClientAppointment[] | null = [];

	hasPrevious: boolean = false;
	hasNext: boolean = false;

	pageNumber: number = 1;

	constructor(
		private appointmentService: AppointmentService,
		private route: ActivatedRoute
	) {}

	ngOnInit(): void {
		this.route.params.subscribe(
			(params) => (this.clientId = params['clientId'])
		);

        this.route.queryParams.subscribe(
            (params) => (this.clientFullName = params['clientFullName'])
        );

		this.loadAppointments();
	}

    getAppointmentEmojiGender(appointment: ClientAppointment) {
        if (appointment.doctorGender === 'Male') {
            return '🧑‍⚕️';
        } else if (appointment.doctorGender === 'Female') {
            return '👩‍⚕️';
        } else {
            return '❔';
        }
    }

	prevPage() {
		if (this.hasPrevious && this.pageNumber > 1) {
			this.pageNumber--;
			this.loadAppointments();
		}
	}

	nextPage() {
		if (this.hasNext) {
			this.pageNumber++;
			this.loadAppointments();
		}
	}

	private loadAppointments(): void {
		this.appointmentService
			.getClientAppointments(this.pageNumber, this.clientId)
			.subscribe((resp) => {
				var pagingParams = JSON.parse(
					resp.headers.get('x-pagination') || '{}'
				);

				this.hasPrevious = pagingParams.HasPrevious;
				this.hasNext = pagingParams.HasNext;

				this.clientAppointments = <ClientAppointment[]>resp.body;
			});
	}
}
