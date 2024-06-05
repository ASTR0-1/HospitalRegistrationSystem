import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { ApplicationUserDto } from '../entities/applicationUser/applicationUserDto';

@Injectable({
	providedIn: 'root',
})
export class UserService {
	private apiUrl = `${environment.apiUrl}/users`;

	constructor(private http: HttpClient) {}

	getUser(): Observable<ApplicationUserDto> {
		let userId = localStorage.getItem('userId');

		return this.http.get<ApplicationUserDto>(`${this.apiUrl}/${userId}`);
	}

	uploadProfilePhoto(file: File): Observable<any> {
		const formData = new FormData();
		formData.append('file', file);

		return this.http.post(`${this.apiUrl}/uploadProfilePhoto`, formData);
	}

	updateUser(user: ApplicationUserDto): Observable<any> {
		return this.http.put(`${this.apiUrl}`, user);
	}
}
