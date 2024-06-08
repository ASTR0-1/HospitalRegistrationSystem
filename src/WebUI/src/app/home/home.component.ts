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
	currentPage = 1;
	pageSize = 10;
	totalPages = 0;

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

		this.hospitalService.getAllHospitals(paging).subscribe((result) => {
			this.hospitals = result.body as HospitalDto[];
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
