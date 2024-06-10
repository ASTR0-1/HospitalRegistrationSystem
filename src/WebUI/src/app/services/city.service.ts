import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { CityDto } from '../entities/location/city/cityDto';
import { PagingParameters } from '../entities/utility/pagingParameters';

@Injectable({
	providedIn: 'root',
})
export class CityService {
	private readonly uri: string = `${environment.apiUrl}/cities`;

	constructor(private http: HttpClient) {}

	public getAllCities(paging: PagingParameters): Observable<CityDto[]> {
		let params = {
			pageNumber: String(paging.pageNumber),
			pageSize: String(paging.pageSize),
		};

		const headers = new HttpHeaders({
			Accept: 'application/json',
		});

		return this.http.get<CityDto[]>(this.uri, {
			params,
			headers,
		});
	}
}
