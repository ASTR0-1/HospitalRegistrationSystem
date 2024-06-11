import { Injectable } from '@angular/core';
import {
	HttpInterceptor,
	HttpRequest,
	HttpHandler,
	HttpEvent,
} from '@angular/common/http';
import {
	BehaviorSubject,
	Observable,
	catchError,
	filter,
	switchMap,
	take,
	throwError,
} from 'rxjs';
import { AuthenticationService } from '../services/authentication.service';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
	private isRefreshing = false;
	private refreshTokenSubject: BehaviorSubject<any> =
		new BehaviorSubject<any>(null);

	constructor(private authenticationService: AuthenticationService) {}

	intercept(
		request: HttpRequest<any>,
		next: HttpHandler,
	): Observable<HttpEvent<any>> {
		let authToken = this.authenticationService.getAuthToken();
		if (authToken) {
			request = this.addToken(request, authToken);
		}

		return next.handle(request).pipe(
			catchError((error) => {
				if (
					error.status === 401 &&
					!request.url.includes('/authentication/refresh')
				) {
					return this.handle401Error(request, next);
				}

				return throwError(() => new Error(error));
			}),
		);
	}

	private addToken(request: HttpRequest<any>, token: string) {
		return request.clone({
			setHeaders: {
				Authorization: `Bearer ${token}`,
			},
		});
	}

	private handle401Error(request: HttpRequest<any>, next: HttpHandler) {
		if (!this.isRefreshing) {
			this.isRefreshing = true;
			this.refreshTokenSubject.next(null);

			return this.authenticationService.refreshToken().pipe(
				switchMap((token: any) => {
					this.isRefreshing = false;
					this.refreshTokenSubject.next(token.accessToken);

					return next.handle(
						this.addToken(request, token.accessToken),
					);
				}),
				catchError((err) => {
					this.isRefreshing = false;
					this.authenticationService.logout();

					return throwError(() => new Error(err));
				}),
			);
		} else {
			return this.refreshTokenSubject.pipe(
				filter((token) => token != null),
				take(1),
				switchMap((jwt) => {
					return next.handle(this.addToken(request, jwt));
				}),
			);
		}
	}
}
