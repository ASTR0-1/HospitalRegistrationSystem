import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { Doctor } from '../entities/doctor';
import { DoctorService } from '../services/doctor.service';

@Component({
	selector: 'app-doctors',
	templateUrl: './doctors.component.html',
	styleUrls: ['./doctors.component.css'],
	providers: [DoctorService],
})
export class DoctorsComponent implements OnInit {
	doctors: Doctor[] | null = [];

	hasPrevious: boolean = false;
	hasNext: boolean = false;

	pageNumber: number = 1;

	searchString: string | null = null;

	constructor(
		private doctorService: DoctorService,
		private route: ActivatedRoute
	) {}

	ngOnInit(): void {
		this.searchString =
			this.route.snapshot.queryParamMap.get('SearchString');

		if (this.searchString) {
			this.searchDoctors(this.searchString);
		} else {
			this.loadDoctors();
		}
	}

	getEmojiGender(doctor: Doctor) {
		if (doctor.gender === 'Male') {
			return '🧑‍⚕️';
		} else if (doctor.gender === 'Female') {
			return '👩‍⚕️';
		} else {
			return '❔';
		}
	}

    getDoctorFullName(doctor: Doctor): string {
        return doctor.firstName + ' ' + doctor.middleName + ' ' + doctor.lastName;
    }

	prevPage() {
		if (this.hasPrevious && this.pageNumber > 1 && this.searchString) {
			this.pageNumber--;
			this.searchDoctors(this.searchString);
		} else if (this.hasPrevious && this.pageNumber > 1) {
			this.pageNumber--;
			this.loadDoctors();
		}
	}

	nextPage() {
		if (this.hasNext && this.searchString) {
			this.pageNumber++;
			this.searchDoctors(this.searchString);
		} else if (this.hasNext) {
			this.pageNumber++;
			this.loadDoctors();
		}
	}

	private loadDoctors() {
		this.doctorService.getDoctors(this.pageNumber).subscribe((resp) => {
			var pagingParams = JSON.parse(
				resp.headers.get('x-pagination') || '{}'
			);

			this.hasPrevious = pagingParams.HasPrevious;
			this.hasNext = pagingParams.HasNext;

			this.doctors = <Doctor[]>resp.body;
		});
	}

	private searchDoctors(searchString: string) {
		this.doctorService
			.searchDoctors(this.pageNumber, searchString)
			.subscribe((resp) => {
				var pagingParams = JSON.parse(
					resp.headers.get('x-pagination') || '{}'
				);

				this.hasPrevious = pagingParams.HasPrevious;
				this.hasNext = pagingParams.HasNext;

				this.doctors = <Doctor[]>resp.body;
			});
	}
}
