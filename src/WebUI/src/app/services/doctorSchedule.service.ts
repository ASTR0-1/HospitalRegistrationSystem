import { HttpClient, HttpHeaders, HttpParams, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { DoctorScheduleDto } from '../entities/doctorSchedule/doctorScheduleDto';
import { DoctorScheduleForManipulationDto } from '../entities/doctorSchedule/doctorScheduleForManipulationDto';
import { DoctorScheduleParameters } from '../entities/doctorSchedule/doctorScheduleParameters';

@Injectable({
	providedIn: 'root',
})
export class DoctorScheduleService {
	private readonly uri: string = `${environment.apiUrl}/doctorSchedules`;

	constructor(private http: HttpClient) {}

	public getDoctorSchedule(
		doctorScheduleId: number
	): Observable<DoctorScheduleDto> {
		return this.http.get<DoctorScheduleDto>(
			`${this.uri}/${doctorScheduleId}`
		);
	}

	public getDoctorSchedules(
		paging: DoctorScheduleParameters,
		doctorId: number
	  ): Observable<HttpResponse<DoctorScheduleDto[]>> {
		let params = new HttpParams()
		  .set('pageNumber', paging.pageNumber.toString())
		  .set('pageSize', paging.pageSize.toString())
		  .set('doctorId', doctorId.toString())
		  .set('from', paging.from)
		  .set('to', paging.to);
	  
		const headers = new HttpHeaders({
		  'Accept': 'application/json',
		});
	  
		return this.http.get<DoctorScheduleDto[]>(this.uri, {
		  params: params,
		  headers: headers,
		  observe: 'response',
		});
	  }

	public createDoctorSchedule(
		doctorScheduleDto: DoctorScheduleForManipulationDto
	): Observable<void> {
		return this.http.post<void>(this.uri, doctorScheduleDto, {
			headers: new HttpHeaders({
				'Content-Type': 'application/json',
			}),
		});
	}

	public updateDoctorSchedule(
		doctorScheduleDto: DoctorScheduleForManipulationDto
	): Observable<void> {
		return this.http.put<void>(
			`${this.uri}`,
			doctorScheduleDto,
			{
				headers: new HttpHeaders({
					'Content-Type': 'application/json',
				}),
			}
		);
	}

	public deleteDoctorSchedule(doctorScheduleId: number): Observable<void> {
		return this.http.delete<void>(`${this.uri}/${doctorScheduleId}`);
	}
}
