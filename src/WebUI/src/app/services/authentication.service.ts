import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { environment } from 'src/environments/environment';
import { UserForAuthenticationDto } from '../entities/authentication/userForAuthenticationDto';
import { UserForRegistrationDto } from '../entities/authentication/userForRegistrationDto';
import { jwtDecode } from 'jwt-decode';

@Injectable({
	providedIn: 'root',
})
export class AuthenticationService {
	private readonly uri: string = `${environment.apiUrl}/authentication`;

	private isInvalidLoginSubject = new BehaviorSubject<boolean>(false);
	private isInvalidRegisterSubject = new BehaviorSubject<boolean>(false);

	public isInvalidLogin$ = this.isInvalidLoginSubject.asObservable();
	public isInvalidRegister$ = this.isInvalidRegisterSubject.asObservable();

	private isRefreshing = false;
	private refreshTokenSubject: BehaviorSubject<any> =
		new BehaviorSubject<any>(null);

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

	public refreshToken(): Observable<any> {
		const jwt = localStorage.getItem('jwt');
		const refreshToken = localStorage.getItem('refresh');

		return this.http
			.post(this.uri + '/refresh', { accessToken: jwt, refreshToken })
			.pipe(
				tap((response: any) => {
					const userId = <string>localStorage.getItem('userId');
					const token = response.accessToken;
					const refreshToken = response.refreshToken;

					this.saveCredsToLocalStorage(token, userId, refreshToken);
				})
			);
	}

	public logout() {
		localStorage.removeItem('jwt');
		localStorage.removeItem('refresh');
		localStorage.removeItem('userId');

		this.isInvalidLoginSubject.next(false);
		this.isInvalidRegisterSubject.next(false);
	}

	public getAuthToken() {
		return localStorage.getItem('jwt');
	}

	public isAuthenticated(): boolean {
		const token = this.getAuthToken();
		
		return token !== null && token !== undefined;
	}

	public hasRole(role: string): boolean {
		const roles = this.getRoles();
		
		return roles.includes(role);
	}

	private processSucceedAuth(response: any) {
		const token = response.token.accessToken;
		const refreshToken = response.token.refreshToken;
		const userId = response.userId;

		this.saveCredsToLocalStorage(token, userId, refreshToken);
	}

	private saveCredsToLocalStorage(
		token: string,
		userId: string,
		refreshToken: string
	) {
		localStorage.setItem('jwt', token);
		localStorage.setItem('refresh', refreshToken);
		localStorage.setItem('userId', userId);
	}

	private getRoles(): string[] {
		const token = this.getAuthToken();
		if (!token) {
			return [];
		}

		const decodedToken = jwtDecode(token) as any;
		const roles =
			decodedToken[
				'http://schemas.microsoft.com/ws/2008/06/identity/claims/role'
			];

		return roles;
	}
}
