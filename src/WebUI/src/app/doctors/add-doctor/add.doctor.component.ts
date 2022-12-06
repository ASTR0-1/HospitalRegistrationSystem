import { Component } from '@angular/core';
import { Location } from '@angular/common';

import { DoctorForCreation } from 'src/app/entities/doctorForCreation';
import { DoctorService } from 'src/app/services/doctor.service';

@Component({
	selector: 'app-doctors',
	templateUrl: './add.doctor.component.html',
	styleUrls: ['./add.doctor.component.css'],
	providers: [DoctorService],
})
export class AddDoctorComponent {
	doctorToAdd: DoctorForCreation = new DoctorForCreation();

	constructor(
		private doctorService: DoctorService,
		private location: Location
	) {}

	addDoctor() {
		this.doctorService.postDoctor(this.doctorToAdd).subscribe({
			error: (error) => console.log(error),
		});

		this.location.back();
	}
}
