import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';
import { AppointmentForCreationDto } from '../entities/appointment/appointmentForCreationDto';
import { AppointmentDto } from '../entities/appointment/appointmentDto';

@Injectable({
	providedIn: 'root',
})
export class AppointmentService {
	readonly url: string = `${environment.apiUrl}/appointments`;

	constructor(private http: HttpClient) {}

	addNew(appointment: AppointmentForCreationDto): Observable<any> {
		return this.http.post(`${this.url}`, appointment);
	}

	get(appointmentId: number): Observable<any> {
		return this.http.get(`${this.url}/${appointmentId}`);
	}

	getIncomingByUserId(
		paging: any,
		userId: number
	): Observable<HttpResponse<AppointmentDto[]>> {
		let params = new HttpParams()
			.set('pageNumber', paging.pageNumber.toString())
			.set('pageSize', paging.pageSize.toString());

		return this.http.get<AppointmentDto[]>(
			`${this.url}/incoming/${userId}`,
			{
				params,
				observe: 'response',
			}
		);
	}

	getAllByUserId(
		paging: any,
		userId: number
	): Observable<HttpResponse<AppointmentDto[]>> {
		return this.http.get<AppointmentDto[]>(`${this.url}/all/${userId}`, {
			params: paging,
			observe: 'response',
		});
	}

	getMissedByUserId(
		paging: any,
		userId: number
	): Observable<HttpResponse<AppointmentDto[]>> {
		return this.http.get<AppointmentDto[]>(`${this.url}/missed/${userId}`, {
			params: paging,
			observe: 'response',
		});
	}

	getVisitedByUserId(
		paging: any,
		userId: number
	): Observable<HttpResponse<AppointmentDto[]>> {
		return this.http.get<AppointmentDto[]>(
			`${this.url}/visited/${userId}`,
			{
				params: paging,
				observe: 'response',
			}
		);
	}

	markAsVisited(appointmentId: number, diagnosis: string): Observable<any> {
		return this.http.put(`${this.url}/markAsVisited/${appointmentId}`, {
			diagnosis,
		});
	}
}
