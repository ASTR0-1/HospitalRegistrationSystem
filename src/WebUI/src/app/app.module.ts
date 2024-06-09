import { NgModule, LOCALE_ID } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { Routes, RouterModule } from '@angular/router';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations'; // Add this line

import { Roles } from './constants/role.constants';

import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { NotFoundComponent } from './not-found/not-found.component';

import { LoginComponent } from './authentication/login/login.component';
import { RegisterComponent } from './authentication/register/register.component';
import { UserPersonalPageComponent } from './user-personal-page/user-personal-page.component';
import { HospitalManagementComponent } from './hospital-management/hospital-management.component';
import { AddHospitalDialogComponent } from './hospital-management/add-hospital-dialog/add-hospital-dialog.component';
import { UserManagementComponent } from './user-management/user-management.component';
import { UpdateUserDialogComponent } from './user-management/update-user-dialog/update-user-dialog.component';
import { AssignEmployeeDialogComponent } from './user-management/assign-employee-dialog/assign-employee-dialog.component';
import { ScheduledAppointmentsComponent } from './scheduled-appointments/scheduled-appointments.component';
import { DoctorScheduleComponent } from './doctor-schedule/doctor-schedule.component';

import { AuthInterceptor } from './interceptors/auth.interceptor';
import { AuthGuard } from './guards/auth.guard';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatTableModule } from '@angular/material/table';
import { RoleGuard } from './guards/role.guard';
import { MatDialogModule } from '@angular/material/dialog';
import { MatSortModule } from '@angular/material/sort';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatSnackBarModule } from '@angular/material/snack-bar';

import { registerLocaleData } from '@angular/common';
import localeUk from '@angular/common/locales/uk';

registerLocaleData(localeUk);

const appRoutes: Routes = [
	{ path: '', component: HomeComponent },
	{ path: 'login', component: LoginComponent },
	{ path: 'register', component: RegisterComponent },
	{ path: 'personal-page', component: UserPersonalPageComponent, canActivate: [AuthGuard] },
	{ path: 'scheduled-appointments', component: ScheduledAppointmentsComponent, canActivate: [AuthGuard] },
	{ path: 'hospital-management', component: HospitalManagementComponent, canActivate: [AuthGuard, RoleGuard], data: { expectedRole: [Roles.MASTER_SUPERVISOR] } },
	{ path: 'user-management', component: UserManagementComponent, canActivate: [AuthGuard, RoleGuard], data: { expectedRole: [Roles.MASTER_SUPERVISOR, Roles.SUPERVISOR] } },
	{ path: 'doctor-schedule', component: DoctorScheduleComponent},
	{ path: '**', component: NotFoundComponent },
];

@NgModule({
	imports: [
		BrowserModule,
		BrowserAnimationsModule,
		FormsModule,
		ReactiveFormsModule,
		HttpClientModule,
		MatTableModule,
		MatDialogModule,
		MatDatepickerModule,
		MatNativeDateModule,
		MatSnackBarModule,
		MatInputModule,
		MatPaginatorModule,
		MatSortModule,
		MatProgressSpinnerModule,
		MatSelectModule,
		RouterModule.forRoot(appRoutes),
	],
	declarations: [
		AppComponent,
		HomeComponent,
		NotFoundComponent,

		LoginComponent,
		RegisterComponent,
		
		UserPersonalPageComponent,

		DoctorScheduleComponent,

		ScheduledAppointmentsComponent,
		
		HospitalManagementComponent,
		AddHospitalDialogComponent,
		
		UserManagementComponent,
		UpdateUserDialogComponent,
		AssignEmployeeDialogComponent
	],
	bootstrap: [AppComponent],
	providers: [
		{ provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },
		{ provide: LOCALE_ID, useValue: 'uk' }
	],
})
export class AppModule {}
