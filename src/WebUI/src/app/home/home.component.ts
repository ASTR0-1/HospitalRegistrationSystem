import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { HospitalService } from '../services/hospital.service';
import { HospitalDto } from '../entities/hospital/hospitalDto';
import { PagingParameters } from '../entities/utility/pagingParameters';

@Component({
	selector: 'app-home',
	templateUrl: './home.component.html',
	styleUrls: ['./home.component.css'],
})
export class HomeComponent implements OnInit {
	hospitals: HospitalDto[] = [];

	currentPage: number = 1;
	pageSize: number = 6;
	totalPages: number = 0;
	hasNext: boolean = false;
    hasPrevious: boolean = false;

	searchQuery = '';

	constructor(
		private router: Router,
		private hospitalService: HospitalService
	) {}

	ngOnInit(): void {
		this.loadHospitals();
	}

	loadHospitals(): void {
		const paging: PagingParameters = {
			pageNumber: this.currentPage,
			pageSize: this.pageSize,
		};

		console.log(this.searchQuery)
		this.hospitalService.getAllHospitals(paging, this.searchQuery).subscribe((result) => {
			this.hospitals = result.body as HospitalDto[];

			const paginationData = JSON.parse(
				result.headers.get('X-Pagination')!
			);

			this.totalPages = paginationData.totalPages;
			this.currentPage = paginationData.currentPage;
			this.pageSize = paginationData.pageSize;
			this.hasNext = paginationData.hasNext;
        	this.hasPrevious = paginationData.hasPrevious;
		});
	}

	nextPage(): void {
		if (this.currentPage < this.totalPages) {
			this.currentPage++;
			this.loadHospitals();
		}
	}

	prevPage(): void {
		if (this.currentPage > 1) {
			this.currentPage--;
			this.loadHospitals();
		}
	}
}
