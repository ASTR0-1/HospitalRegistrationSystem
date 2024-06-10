import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ApplicationUserDto } from '../entities/applicationUser/applicationUserDto';
import { PagingParameters } from '../entities/utility/pagingParameters';
import { UserService } from '../services/user.service';
import { Roles } from '../constants/role.constants';
import { HospitalDto } from '../entities/hospital/hospitalDto';
import { HospitalService } from '../services/hospital.service';
import { AuthenticationService } from '../services/authentication.service';
import { FeedbackService } from '../services/feedback.service';
import { MatDialog } from '@angular/material/dialog';
import { FeedbackListDialogComponent } from './feedbacks-list-dialog/feedback-list-dialog.component';

@Component({
	selector: 'app-doctors-by-hospital',
	templateUrl: './doctors-by-hospital.component.html',
	styleUrls: ['./doctors-by-hospital.component.css'],
})
export class DoctorsByHospitalComponent implements OnInit {
	doctors: ApplicationUserDto[] = [];
	hospitalId!: number;
	hospital!: HospitalDto;
	paging: PagingParameters = {
		pageNumber: 1,
		pageSize: 5,
	};
	totalPages: number = 0;
	hasNext: boolean = false;
	hasPrevious: boolean = false;

	constructor(
		private router: Router,
		private route: ActivatedRoute,
		private userService: UserService,
		private hospitalService: HospitalService,
		private authService: AuthenticationService,
		private feedbackService: FeedbackService,
		private dialog: MatDialog
	) {}

	ngOnInit(): void {
		this.hospitalId = +this.route.snapshot.paramMap.get('id')!;
		this.hospitalService
			.getHospital(this.hospitalId)
			.subscribe((hospital) => {
				this.hospital = hospital;
			});
		this.loadDoctors();
	}

	loadDoctors(): void {
		this.userService
			.getAllByRole(this.paging, Roles.DOCTOR, this.hospitalId)
			.subscribe((response) => {
				this.doctors = response.body!;
				console.log(response.body);
				const paginationData = JSON.parse(
					response.headers.get('X-Pagination')!
				);

				this.totalPages = paginationData.totalPages;
				this.paging.pageNumber = paginationData.currentPage;
				this.paging.pageSize = paginationData.pageSize;
				this.hasNext = paginationData.hasNext;
				this.hasPrevious = paginationData.hasPrevious;

				this.doctors.forEach((doctor) => {
					this.feedbackService
						.getAverageRating(doctor.id)
						.subscribe((averageRating) => {
							doctor.averageRating = averageRating;
						});
				});
			});
	}

	openFeedbackDialog(doctorId: number): void {
		this.dialog.open(FeedbackListDialogComponent, {
			width: '400px',
			data: { doctorId },
		});
	}

	redirectToDoctorSchedule(doctorId: number) {
		this.router.navigate(['/doctor-schedule'], {
			queryParams: { doctorId: doctorId },
		});
	}

	redirectToDoctorAppointments(doctorId: number) {
		this.router.navigate(['/scheduled-appointments'], {
			queryParams: { userId: doctorId },
		});
	}

	isReceptionist(): boolean {
		return this.authService.hasRole(Roles.RECEPTIONIST);
	}

	nextPage(): void {
		if (this.paging.pageNumber < this.totalPages) {
			this.paging.pageNumber++;
			this.loadDoctors();
		}
	}

	prevPage(): void {
		if (this.paging.pageNumber > 1) {
			this.paging.pageNumber--;
			this.loadDoctors();
		}
	}
}
