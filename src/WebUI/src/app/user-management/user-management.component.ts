import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';
import { UserService } from '../services/user.service';
import { ApplicationUserDto } from '../entities/applicationUser/applicationUserDto';
import { PagingParameters } from '../entities/utility/pagingParameters';
import { Roles } from '../constants/role.constants';
import { HttpResponse } from '@angular/common/http';
import { ActivatedRoute } from '@angular/router';
import { AuthenticationService } from '../services/authentication.service';
import { UpdateUserDialogComponent } from './update-user-dialog/update-user-dialog.component';
import { AssignEmployeeDialogComponent } from './assign-employee-dialog/assign-employee-dialog.component';

@Component({
	selector: 'app-user-management',
	templateUrl: './user-management.component.html',
	styleUrls: ['./user-management.component.css'],
})
export class UserManagementComponent implements OnInit {
	displayedColumns: string[] = [
		'id',
		'firstName',
		'middleName',
		'lastName',
		'gender',
		'specialty',
		'visitCost',
		'totalServiceCost',
		'hospitalId',
		'actions',
	];
	dataSource: MatTableDataSource<ApplicationUserDto> =
		new MatTableDataSource<ApplicationUserDto>();
	totalUsers = 0;
	pageSize = 25;
	selectedRole = Roles.CLIENT;
	searchQuery = '';
	roles = Object.values(Roles);

	@ViewChild(MatPaginator) paginator: MatPaginator | undefined;

	constructor(
		private userService: UserService,
		private authService: AuthenticationService,
		public dialog: MatDialog,
		private route: ActivatedRoute
	) {}

	ngOnInit(): void {
		this.fetchUsers();
	}

	fetchUsers(pageNumber: number = 1, pageSize: number = this.pageSize): void {
		const pagingParameters: PagingParameters = { pageNumber, pageSize };
		let hospitalId: number | undefined = this.getHospitalIdFromUrl();

		if (!this.authService.hasRole(Roles.MASTER_SUPERVISOR)) {
			let id = this.authService.getHospitalId();
			if (id === null) {
				throw new Error('Hospital ID is null');
			}

			hospitalId = parseInt(id);
		}

		if (this.selectedRole === Roles.CLIENT) {
			hospitalId = undefined;
		}

		this.userService
			.getAllByRole(
				pagingParameters,
				this.selectedRole,
				hospitalId,
				this.searchQuery
			)
			.subscribe({
				next: (response: HttpResponse<ApplicationUserDto[]>) => {
					this.dataSource.data = response.body!;
					const paginationData = JSON.parse(
						response.headers.get('X-Pagination')!
					);
					this.totalUsers = paginationData.totalCount;
					this.paginator!.length = paginationData.totalCount;
					this.paginator!.pageIndex = paginationData.currentPage - 1;
					this.paginator!.pageSize = paginationData.pageSize;
				},
			});
	}

	private getHospitalIdFromUrl(): number | undefined {
		if (!this.route.snapshot.queryParamMap.has('hospitalId')) {
			return undefined;
		}

		return parseInt(this.route.snapshot.queryParamMap.get('hospitalId')!);
	}

	onPageChange(event: any): void {
		this.fetchUsers(event.pageIndex + 1, event.pageSize);
	}

	openAssignDialog(user?: ApplicationUserDto): void {
		const dialogRef = this.dialog.open(AssignEmployeeDialogComponent, {
			width: '400px',
			data: user,
		});

		dialogRef.afterClosed().subscribe((result) => {
			if (result) {
				this.fetchUsers(
					this.paginator!.pageIndex,
					this.paginator!.pageSize
				);
			}
		});
	}

	openUpdateDialog(user: ApplicationUserDto): void {
		const dialogRef = this.dialog.open(UpdateUserDialogComponent, {
			width: '275px',
			data: user,
		});

		dialogRef.afterClosed().subscribe(() => {
			this.fetchUsers(
				this.paginator!.pageIndex,
				this.paginator!.pageSize
			);
		});
	}
}
