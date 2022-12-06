import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { DoctorAppointment } from 'src/app/entities/doctorAppointment';
import { AppointmentService } from 'src/app/services/appointment.service';

@Component({
	selector: 'doctor-appointments',
	templateUrl: './doctor.appointments.component.html',
	styleUrls: ['./doctor.appointments.component.css'],
	providers: [AppointmentService]
})
export class DoctorAppointmentsComponent implements OnInit {
	doctorId: number = 0;
    doctorFullName: string = "";
	doctorAppointments: DoctorAppointment[] | null = [];

	hasPrevious: boolean = false;
	hasNext: boolean = false;

	pageNumber: number = 1;

	constructor(
		private appointmentService: AppointmentService,
		private route: ActivatedRoute
	) {}

	ngOnInit(): void {
		this.route.params.subscribe(
			(params) => (this.doctorId = params['doctorId'])
		);

        this.route.queryParams.subscribe(
            (params) => (this.doctorFullName = params['doctorFullName'])
        );

		this.loadAppointments();
	}

    getAppointmentEmojiGender(appointment: DoctorAppointment) {
        if (appointment.clientGender === 'Male') {
            return '🧑';
        } else if (appointment.clientGender === 'Female') {
            return '👩';
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
			.getDoctorAppointments(this.pageNumber, this.doctorId)
			.subscribe((resp) => {
				var pagingParams = JSON.parse(
					resp.headers.get('x-pagination') || '{}'
				);

				this.hasPrevious = pagingParams.HasPrevious;
				this.hasNext = pagingParams.HasNext;

				this.doctorAppointments = <DoctorAppointment[]>resp.body;
			});
	}
}
