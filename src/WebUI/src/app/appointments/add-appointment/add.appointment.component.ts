import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { AppointmentForCreation } from 'src/app/entities/appointmentForCreation';
import { AppointmentService } from 'src/app/services/appointment.service';

@Component({
	selector: 'client-appointments',
	templateUrl: './add.appointment.component.html',
	styleUrls: ['./add.appointment.component.css'],
	providers: [AppointmentService],
})
export class AddAppointmentComponent implements OnInit {
	doctorFullName: string = '';

    appointmentToAdd: AppointmentForCreation = new AppointmentForCreation();

	constructor(
		private appointmentService: AppointmentService,
		private route: ActivatedRoute,
        private location: Location
	) {}

	ngOnInit(): void {
        this.route.params.subscribe(
			(params) => (this.appointmentToAdd.doctorId = params['doctorId'])
		);

        this.route.queryParams.subscribe(
            (params) => (this.doctorFullName = params['doctorFullName'])
        );
    }

    addAppointment() {
        this.appointmentService.postAppointment(this.appointmentToAdd).subscribe({
            error: (error) => console.log(error)
        });

        this.location.back();
    }
}
