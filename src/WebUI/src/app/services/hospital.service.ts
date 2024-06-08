import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { HospitalDto } from '../entities/hospital/hospitalDto';
import { HospitalForCreationDto } from '../entities/hospital/hospitalForCreationDto';
import { PagingParameters } from '../entities/utility/pagingParameters';

@Injectable({
	providedIn: 'root',
})
export class HospitalService {
	private readonly uri: string = `${environment.apiUrl}/hospitals`;

	constructor(private http: HttpClient) {}

	public getHospital(hospitalId: number): Observable<HospitalDto> {
		return this.http.get<HospitalDto>(`${this.uri}/${hospitalId}`);
	}

	public getAllHospitals(
		paging: PagingParameters,
		searchQuery?: string
	): Observable<HospitalDto[]> {
		let params = {
			pageNumber: String(paging.pageNumber),
			pageSize: String(paging.pageSize),
			searchQuery: searchQuery || '',
		};

		const headers = new HttpHeaders({
			Accept: 'application/json',
		});

		return this.http.get<HospitalDto[]>(this.uri, {
			params,
			headers,
		});
	}

	public addNewHospital(
		hospitalCreationDto: HospitalForCreationDto
	): Observable<void> {
		return this.http.post<void>(this.uri, hospitalCreationDto, {
			headers: new HttpHeaders({
				'Content-Type': 'application/json',
			}),
		});
	}

	public deleteHospital(hospitalId: number): Observable<void> {
		return this.http.delete<void>(`${this.uri}/${hospitalId}`);
	}
}
