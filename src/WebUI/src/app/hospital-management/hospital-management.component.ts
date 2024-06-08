import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';
import { HospitalService } from '../services/hospital.service';
import { HospitalDto } from '../entities/hospital/hospitalDto';
import { PagingParameters } from '../entities/utility/pagingParameters';
import { AddHospitalDialogComponent } from './add-hospital-dialog/add-hospital-dialog.component';

@Component({
	selector: 'app-hospital-management',
	templateUrl: './hospital-management.component.html',
	styleUrls: ['./hospital-management.component.css'],
})
export class HospitalManagementComponent implements OnInit {
	displayedColumns: string[] = [
		'id',
		'name',
		'region',
		'city',
		'street',
		'actions',
	];
	dataSource: MatTableDataSource<HospitalDto> =
		new MatTableDataSource<HospitalDto>();
	totalHospitals = 0;

	@ViewChild(MatPaginator) paginator: MatPaginator | undefined;

	constructor(
		private hospitalService: HospitalService,
		public dialog: MatDialog
	) {}

	ngOnInit(): void {
		this.fetchHospitals();
	}

	fetchHospitals(pageNumber: number = 1, pageSize: number = 10): void {
		pageNumber = Math.max(1, pageNumber);

		const pagingParameters: PagingParameters = { pageNumber, pageSize };
		this.hospitalService
			.getAllHospitals(pagingParameters)
			.subscribe((response) => {
				this.dataSource.data = response.body!;
				const paginationData = JSON.parse(
					response.headers.get('X-Pagination')!
				);
				this.paginator!.length = paginationData.totalCount;
				this.paginator!.pageIndex = paginationData.currentPage - 1;
				this.paginator!.pageSize = paginationData.pageSize;
			});
	}

	onPageChange(event: any): void {
		this.fetchHospitals(event.pageIndex + 1, event.pageSize);
	}

	openAddDialog(): void {
		const dialogRef = this.dialog.open(AddHospitalDialogComponent, {
			width: '600px',
			disableClose: true,
		});

		dialogRef.afterClosed().subscribe((result) => {
			if (result) {
				this.fetchHospitals(
					this.paginator!.getNumberOfPages(),
					this.paginator!.pageSize
				);
			}
		});
	}

	deleteHospital(hospitalId: number): void {
		this.hospitalService.deleteHospital(hospitalId).subscribe(() => {
			this.fetchHospitals(
				this.paginator!.getNumberOfPages(),
				this.paginator!.pageSize
			);
		});
	}
}
