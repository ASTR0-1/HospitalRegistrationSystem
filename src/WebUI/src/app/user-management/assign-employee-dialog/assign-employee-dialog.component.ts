import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ValidationErrors, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { UserService } from 'src/app/services/user.service';
import { HospitalService } from 'src/app/services/hospital.service';
import { HospitalDto } from 'src/app/entities/hospital/hospitalDto';
import { ApplicationUserDto } from 'src/app/entities/applicationUser/applicationUserDto';
import { Roles } from 'src/app/constants/role.constants';
import { AuthenticationService } from 'src/app/services/authentication.service';

@Component({
	selector: 'app-assign-employee-dialog',
	templateUrl: './assign-employee-dialog.component.html',
	styleUrls: ['./assign-employee-dialog.component.css'],
})
export class AssignEmployeeDialogComponent implements OnInit {
	assignForm: FormGroup;
	hospitals: HospitalDto[] = [];
	roles: string[] = [Roles.DOCTOR, Roles.RECEPTIONIST];

	constructor(
		private fb: FormBuilder,
		private userService: UserService,
		private hospitalService: HospitalService,
		private authService: AuthenticationService,
		public dialogRef: MatDialogRef<AssignEmployeeDialogComponent>,
		@Inject(MAT_DIALOG_DATA) public data: ApplicationUserDto
	) {
		this.assignForm = this.fb.group({
			hospitalId: [null, Validators.required],
			role: [null, Validators.required],
			specialty: [''],
			doctorPrice: [0],
		});

		if (this.authService.hasRole(Roles.MASTER_SUPERVISOR)) {
			this.roles.push(Roles.SUPERVISOR);
		}
	}

	ngOnInit(): void {
		this.loadHospitals();
	}

	loadHospitals(): void {
		const pagingParameters = { pageNumber: 1, pageSize: 50 };
		this.hospitalService
			.getAllHospitals(pagingParameters)
			.subscribe((response) => {
				this.hospitals = response.body!;
			});
	}

	onSubmit(): void {
		if (this.assignForm.valid) {
			const { hospitalId, role, specialty, doctorPrice } =
				this.assignForm.value;
			this.userService
				.assignEmployee(
					this.data.id,
					role,
					hospitalId,
					specialty,
					doctorPrice
				)
				.subscribe(() => {
					this.dialogRef.close();
				});
		}
	}
}
