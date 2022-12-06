import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';

import { ClientForCreation } from 'src/app/entities/clientForCreation';

@Injectable()
export class ClientService {
    readonly uri: string = 'https://localhost:7247/api/clients';

    constructor(private http: HttpClient) { }

    getClients(pageNumber: number) {
        const params = new HttpParams()
            .set("PageNumber", pageNumber.toString());

        return this.http.get(this.uri, {observe: 'response', params});
    }

    searchClients(pageNumber: number, searchString: string) {
        const params = new HttpParams()
            .set("PageNumber", pageNumber.toString())
            .set("SearchString", searchString);

         return this.http.get(this.uri, {observe: 'response', params});
    }

    postClient(client: ClientForCreation) {
        
        return this.http.post(this.uri, client);        
    }
}