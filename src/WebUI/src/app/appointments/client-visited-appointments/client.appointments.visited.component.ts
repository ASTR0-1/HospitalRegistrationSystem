import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { ClientVisitedAppointment } from 'src/app/entities/clientVisitedAppointment';
import { AppointmentService } from 'src/app/services/appointment.service';

@Component({
	selector: 'client-appointments',
	templateUrl: './client.appointments.visited.component.html',
	styleUrls: ['./client.appointments.visited.component.css'],
	providers: [AppointmentService],
})
export class ClientVisitedAppointmentsComponent implements OnInit {
	clientId: number = 0;
    clientFullName: string = "";
	clientVisitedAppointments: ClientVisitedAppointment[] | null = [];

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

		this.loadVisitedAppointments();
	}

    getAppointmentEmojiGender(appointment: ClientVisitedAppointment) {
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
			this.loadVisitedAppointments();
		}
	}

	nextPage() {
		if (this.hasNext) {
			this.pageNumber++;
			this.loadVisitedAppointments();
		}
	}

	private loadVisitedAppointments(): void {
		this.appointmentService
			.getClientVisitedAppointments(this.pageNumber, this.clientId)
			.subscribe((resp) => {
				var pagingParams = JSON.parse(
					resp.headers.get('x-pagination') || '{}'
				);

				this.hasPrevious = pagingParams.HasPrevious;
				this.hasNext = pagingParams.HasNext;

				this.clientVisitedAppointments = <ClientVisitedAppointment[]>resp.body;
			});
	}
}
