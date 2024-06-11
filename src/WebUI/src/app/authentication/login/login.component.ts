import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { UserForAuthenticationDto } from 'src/app/entities/authentication/userForAuthenticationDto';
import { AuthenticationService } from 'src/app/services/authentication.service';

@Component({
	selector: 'app-login',
	templateUrl: './login.component.html',
	styleUrls: ['./login.component.css'],
	providers: [AuthenticationService],
})
export class LoginComponent {
	public isInvalidLogin$: Observable<boolean>;
	public user: UserForAuthenticationDto = new UserForAuthenticationDto();

	constructor(
		private router: Router,
		private authenticationService: AuthenticationService,
	) {
		this.isInvalidLogin$ = this.authenticationService.isInvalidLogin$;
	}

	public login(): void {
		this.authenticationService.login(this.user).subscribe(() => {
			this.router.navigate(['']);
		});
	}
}
