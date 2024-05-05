import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, tap } from 'rxjs';
import { environment } from 'src/environments/environment';
import { UserForAuthenticationDto } from '../entities/authentication/userForAuthenticationDto';
import { UserForRegistrationDto } from '../entities/authentication/userForRegistrationDto';

@Injectable({
	providedIn: 'root',
})
export class AuthenticationService {
	private readonly uri: string = `${environment.apiUrl}/authentication`;

	private isInvalidLoginSubject = new BehaviorSubject<boolean>(false);
	private isInvalidRegisterSubject = new BehaviorSubject<boolean>(false);

	public isInvalidLogin$ = this.isInvalidLoginSubject.asObservable();
	public isInvalidRegister$ = this.isInvalidRegisterSubject.asObservable();

	constructor(private http: HttpClient) {}

	public login(user: UserForAuthenticationDto) {
		return this.http
			.post(this.uri + '/login', user, {
				headers: new HttpHeaders({
					'Content-Type': 'application/json',
				}),
				withCredentials: true,
			})
			.pipe(
				tap({
					next: (response) => {
						this.processSucceedAuth(response);
						this.isInvalidLoginSubject.next(false);
					},
					error: (error) => {
						if (!environment.production) {
							console.log(error);
						}
						this.isInvalidLoginSubject.next(true);
					},
				})
			);
	}

	public register(user: UserForRegistrationDto) {
		return this.http
			.post(this.uri + '/register', user, {
				headers: new HttpHeaders({
					'Content-Type': 'application/json',
				}),
				withCredentials: true,
			})
			.pipe(
				tap({
					next: (response) => {
						this.processSucceedAuth(response);
						this.isInvalidRegisterSubject.next(false);
					},
					error: (error) => {
						if (!environment.production) {
							console.log(error);
						}
						this.isInvalidRegisterSubject.next(true);
					},
				})
			);
	}

	private processSucceedAuth(response: Object) {
		const token = (<any>response).token;
		const userId = (<any>response).userId;

		this.saveCredsToLocalStorage(token, userId);
	}

	private saveCredsToLocalStorage(token: string, userId: string) {
		localStorage.setItem('jwt', token);
		localStorage.setItem('userId', userId);
	}
}
