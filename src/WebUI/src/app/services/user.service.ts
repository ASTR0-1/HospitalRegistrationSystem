import { Injectable } from '@angular/core';
import {
	HttpClient,
	HttpHeaders,
	HttpParams,
	HttpResponse,
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { ApplicationUserDto } from '../entities/applicationUser/applicationUserDto';
import { PagingParameters } from '../entities/utility/pagingParameters';
import { HospitalDto } from '../entities/hospital/hospitalDto';

@Injectable({
	providedIn: 'root',
})
export class UserService {
	private headers = new HttpHeaders({
		Accept: 'application/json',
	});

	private apiUrl = `${environment.apiUrl}/users`;

	constructor(private http: HttpClient) {}

	getUser(): Observable<ApplicationUserDto> {
		let userId = localStorage.getItem('userId');

		return this.http.get<ApplicationUserDto>(`${this.apiUrl}/${userId}`);
	}

	getAllByRole(
		paging: PagingParameters,
		role: string,
		hospitalId?: number,
		searchQuery?: string
	): Observable<HttpResponse<ApplicationUserDto[]>> {
		let params = new HttpParams()
			.set('pageNumber', paging.pageNumber.toString())
			.set('pageSize', paging.pageSize.toString())
			.set('role', role);

		if (searchQuery) {
			params = params.set('searchQuery', searchQuery);
		}

		if (hospitalId) {
			return this.http.get<ApplicationUserDto[]>(
				`${this.apiUrl}/hospital/${hospitalId}/role/${role}`,
				{
					params,
					headers: this.headers,
					observe: 'response',
				}
			);
		}

		return this.http.get<ApplicationUserDto[]>(
			`${this.apiUrl}/role/${role}`,
			{
				params,
				headers: this.headers,
				observe: 'response',
			}
		);
	}

	uploadProfilePhoto(file: File): Observable<any> {
		const formData = new FormData();
		formData.append('file', file);

		return this.http.post(`${this.apiUrl}/uploadProfilePhoto`, formData);
	}

	updateUser(user: ApplicationUserDto): Observable<any> {
		return this.http.put(`${this.apiUrl}`, user);
	}

	assignEmployee(
		userId: number,
		role: string,
		hospitalId: number,
		specialty: string,
		doctorPrice: number
	): Observable<any> {
		const body = {
			hospitalId: hospitalId,
			specialty: specialty,
			doctorPrice: doctorPrice
		};

		return this.http.post(`${this.apiUrl}/${userId}/role/${role}`, body);
	}
}
