import { Injectable } from '@angular/core';
import {
	ActivatedRouteSnapshot,
	Router,
	RouterStateSnapshot,
	UrlTree,
} from '@angular/router';
import { Observable } from 'rxjs';
import { AuthenticationService } from '../services/authentication.service';

@Injectable({
	providedIn: 'root',
})
export class RoleGuard {
	constructor(private router: Router, private authService: AuthenticationService) {}

	canActivate(
		next: ActivatedRouteSnapshot,
		state: RouterStateSnapshot
	):
		| Observable<boolean | UrlTree>
		| Promise<boolean | UrlTree>
		| boolean
		| UrlTree {
		const expectedRole = next.data['expectedRole'];
		const isInRole = this.authService.hasRole(expectedRole);

		if (!isInRole) {
			this.router.navigate(['/']);
		}

		return isInRole;
	}
}
