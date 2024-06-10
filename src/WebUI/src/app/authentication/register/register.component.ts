import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { UserForRegistrationDto } from 'src/app/entities/authentication/userForRegistrationDto';
import { AuthenticationService } from 'src/app/services/authentication.service';

@Component({
	selector: 'app-register',
	templateUrl: './register.component.html',
	styleUrl: './register.component.css',
	providers: [AuthenticationService],
})
export class RegisterComponent {
	public isInvalidRegister$: Observable<boolean>;
	public user: UserForRegistrationDto = new UserForRegistrationDto();

	public passwordErrors = {
		minlength: false,
	};
	public phoneNumberErrors = {
		minlength: false,
	};

	constructor(
		private router: Router,
		private authenticationService: AuthenticationService,
	) {
		this.isInvalidRegister$ = this.authenticationService.isInvalidRegister$;
	}

	public register(): void {
		this.authenticationService.register(this.user).subscribe(() => {
			this.router.navigate(['']);
		});
	}
}
