import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from './services/authentication.service';
import { Roles } from './constants/role.constants';

@Component({
	selector: 'app-root',
	templateUrl: './app.component.html',
	styleUrls: ['./app.component.css'],
})
export class AppComponent implements OnInit {
	readonly roles = Roles;

	constructor(public authService: AuthenticationService) { }
	
	ngOnInit() {
	}

	ngOnDestroy() {
	}
}
