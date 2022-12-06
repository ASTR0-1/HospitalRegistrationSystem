import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { DoctorForCreation } from '../entities/doctorForCreation';

@Injectable()
export class DoctorService {
    readonly uri: string = 'https://localhost:7247/api/doctors';

    constructor(private http: HttpClient) { }

    getDoctors(pageNumber: number) {
        const params = new HttpParams()
            .set("PageNumber", pageNumber.toString());

        return this.http.get(this.uri, {observe: 'response', params});
    }

    searchDoctors(pageNumber: number, searchString: string) {
        const params = new HttpParams()
            .set("PageNumber", pageNumber.toString())
            .set("SearchString", searchString);

         return this.http.get(this.uri, {observe: 'response', params});
    }

    postDoctor(doctor: DoctorForCreation) {
        
        return this.http.post(this.uri, doctor);
    }
}