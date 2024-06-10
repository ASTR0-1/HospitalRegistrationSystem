import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from './services/authentication.service';
import { Roles } from './constants/role.constants';
import { Router } from '@angular/router';

@Component({
	selector: 'app-root',
	templateUrl: './app.component.html',
	styleUrls: ['./app.component.css'],
})
export class AppComponent implements OnInit {
	readonly roles = Roles;

	constructor(public authService: AuthenticationService, public router: Router) { }
	
	ngOnInit() {
	}

	ngOnDestroy() {
	}

	viewHospital() {
		const hospitalId = this.authService.getHospitalId();
		this.router.navigate(['/doctors-by-hospital', hospitalId]);
	}
}
