import {
	Component,
	OnInit,
	ElementRef,
	ViewChild,
	AfterViewInit,
} from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { MatSelect } from '@angular/material/select';
import { HospitalForCreationDto } from 'src/app/entities/hospital/hospitalForCreationDto';
import { CityDto } from 'src/app/entities/location/city/cityDto';
import { PagingParameters } from 'src/app/entities/utility/pagingParameters';
import { CityService } from 'src/app/services/city.service';
import { HospitalService } from 'src/app/services/hospital.service';

@Component({
	selector: 'app-add-hospital-dialog',
	templateUrl: './add-hospital-dialog.component.html',
	styleUrls: ['./add-hospital-dialog.component.css'],
})
export class AddHospitalDialogComponent implements OnInit {
	scrollCheckInterval: any;
	hospitalForm!: FormGroup;
	cities: CityDto[] = [];
	isLoading = false;
	currentPage = 1;
	pageSize = 10;

	@ViewChild(MatSelect) matSelect!: MatSelect;

	constructor(
		private fb: FormBuilder,
		private hospitalService: HospitalService,
		private cityService: CityService,
		private dialogRef: MatDialogRef<AddHospitalDialogComponent>
	) {}

	ngOnInit(): void {
		this.hospitalForm = this.fb.group({
			name: ['', Validators.required],
			hospitalFeePercent: ['', Validators.required],
			cityId: ['', Validators.required],
			street: ['', Validators.required],
		});
		this.loadCities();
	}

	onOpenedChange(opened: boolean): void {
		if (opened) {
			const panel = this.matSelect.panel.nativeElement;
			panel.addEventListener('scroll', this.onScroll.bind(this));
		}
	}

	onScroll(event: any): void {
		const panel = this.matSelect.panel.nativeElement;
		const atBottom =
			event.target.scrollTop === panel.scrollHeight - panel.clientHeight;

		if (atBottom) {
			this.loadMoreCities();
		}
	}

	loadMoreCities(): void {
		this.currentPage++;
		this.loadCities();
	}

	loadCities(): void {
		const pagingParameters: PagingParameters = {
			pageNumber: this.currentPage,
			pageSize: this.pageSize,
		};
		this.isLoading = true;
		this.cityService.getAllCities(pagingParameters).subscribe((data) => {
			this.cities = [...this.cities, ...data];
			this.isLoading = false;
		});
	}

	onSubmit(): void {
		if (this.hospitalForm.valid) {
			const newHospital: HospitalForCreationDto = this.hospitalForm.value;
			this.hospitalService.addNewHospital(newHospital).subscribe(() => {
				this.dialogRef.close(true);
			});
		}
	}

	onCancel(): void {
		this.dialogRef.close();
	}
}
