import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { FeedbackDto } from '../entities/feedback/feedbackDto';
import { FeedbackForCreationDto } from '../entities/feedback/feedbackForCreationDto';
import { PagingParameters } from '../entities/utility/pagingParameters';
import { environment } from 'src/environments/environment';

@Injectable({
	providedIn: 'root',
})
export class FeedbackService {
	private apiUrl = `${environment.apiUrl}/feedbacks`;

	constructor(private http: HttpClient) {}

	getAll(
		parameters: PagingParameters,
		userId: number,
	): Observable<FeedbackDto[]> {
		let params = new HttpParams()
			.set('pageNumber', parameters.pageNumber.toString())
			.set('pageSize', parameters.pageSize.toString())
			.set('userId', userId.toString());

		return this.http.get<FeedbackDto[]>(this.apiUrl, { params });
	}

	getAverageRating(userId: number): Observable<number> {
		return this.http.get<number>(`${this.apiUrl}/averageRating/${userId}`);
	}

	addNew(feedback: FeedbackForCreationDto): Observable<void> {
		return this.http.post<void>(this.apiUrl, feedback);
	}
}
