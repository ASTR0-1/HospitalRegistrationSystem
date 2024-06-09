import { Component, OnInit } from '@angular/core';
import { ApplicationUserDto } from '../entities/applicationUser/applicationUserDto';
import { UserService } from '../services/user.service';
import { AuthenticationService } from '../services/authentication.service';
import { Roles } from '../constants/role.constants';
import { Router } from '@angular/router';

@Component({
	selector: 'app-user-personal-page',
	templateUrl: './user-personal-page.component.html',
	styleUrls: ['./user-personal-page.component.css'],
})
export class UserPersonalPageComponent implements OnInit {
	user: ApplicationUserDto = {
		id: 0,
		firstName: '',
		middleName: '',
		lastName: '',
		gender: '',
	};
	imageUrl: string | ArrayBuffer | null = null;

	constructor(
		private userService: UserService,
		private authenticatonService: AuthenticationService,
		private router: Router
	) {}

	ngOnInit(): void {
		this.loadUserData();
	}

	loadUserData(): void {
		this.userService.getUser().subscribe({
			next: (data) => {
				console.log(data);
				this.user = data;
				this.imageUrl = data.profilePhotoUrl
					? data.profilePhotoUrl
					: null;
			},
			error: (error) => {
				console.error('Error fetching user data', error);
			},
		});
	}

	onFileSelected(event: Event) {
		const file = (event.target as HTMLInputElement).files?.[0];

		if (file) {
			if (!file.type.startsWith('image/')) {
				console.error('Selected file is not an image');
				return;
			}

			const reader = new FileReader();
			reader.onload = () => {
				this.imageUrl = reader.result;
			};
			reader.readAsDataURL(file);

			this.userService.uploadProfilePhoto(file).subscribe({
				next: () => {
					console.log('Profile photo uploaded successfully');
				},
				error: (error) => {
					console.error('Error uploading profile photo', error);
				},
			});
		}
	}

	redirectToDoctorSchedule() {
		this.router.navigate(['/doctor-schedule']);
	}

	saveUser() {
		this.userService.updateUser(this.user).subscribe({
			next: () => {
				console.log('User information updated successfully');
			},
			error: (error) => {
				console.error('Error updating user information', error);
			},
		});
	}

	viewAppointments() {
		this.router.navigate(['/scheduled-appointments']);
	}

	userIsDoctor(): boolean {
		return this.authenticatonService.hasRole(Roles.DOCTOR);
	}
}
