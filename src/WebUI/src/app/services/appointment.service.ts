import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { AppointmentForCreation } from '../entities/appointmentForCreation';
import { environment } from 'src/environments/environment';

@Injectable()
export class AppointmentService {
	readonly uri: string = `${environment.apiUrl}/api/appointments`;

	constructor(private http: HttpClient) {}

	getClientAppointments(pageNumber: number, clientId: number) {
		const params = new HttpParams().set(
			'PageNumber',
			pageNumber.toString()
		);

		return this.http.get(this.uri + '/client' + `/${clientId}`, {
			observe: 'response',
			params,
		});
	}

	getDoctorAppointments(pageNumber: number, doctorId: number) {
		const params = new HttpParams().set(
			'PageNumber',
			pageNumber.toString()
		);

		return this.http.get(this.uri + '/doctor' + `/${doctorId}`, {
			observe: 'response',
			params,
		});
	}

	getClientVisitedAppointments(pageNumber: number, clientId: number) {
		const params = new HttpParams().set(
			'PageNumber',
			pageNumber.toString()
		);

		return this.http.get(
			this.uri + '/client' + `/${clientId}` + '/visited',
			{ observe: 'response', params }
		);
	}

	postAppointment(appointment: AppointmentForCreation) {
		return this.http.post(this.uri, appointment);
	}

	putAppointment(appointmentId: number, diagnosis: string) {
		return this.http.put(
			this.uri +
				`/${appointmentId}` +
				'/markAsVisited' +
				`?diagnosis=${diagnosis}`,
			null
		);
	}
}
