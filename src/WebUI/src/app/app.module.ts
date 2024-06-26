import { NgModule, LOCALE_ID } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { Routes, RouterModule } from '@angular/router';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

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
import { VisitedAppointmentsComponent } from './visited-appointments/visited-appointments.component';
import { DoctorsByHospitalComponent } from './doctors-by-hospital/doctors-by-hospital.component';
import { CreateAppointmentDialogComponent } from './doctor-schedule/create-appointment-dialog/create-appointment-dialog.component';
import { MissedAppointmentsComponent } from './missed-appointments/missed-appointments.component';
import { MarkAppointmentAsVisitedDialogComponent } from './missed-appointments/mark-appointment-as-visited-dialog/mark-appointment-as-visited-dialog.component';
import { FeedbackDialogComponent } from './visited-appointments/feedback-dialog/feedback-dialog.component';
import { FeedbackListDialogComponent } from './doctors-by-hospital/feedbacks-list-dialog/feedback-list-dialog.component';

import { AuthInterceptor } from './interceptors/auth.interceptor';
import { AuthGuard } from './guards/auth.guard';
import { RoleGuard } from './guards/role.guard';

import { MatPaginatorModule } from '@angular/material/paginator';
import { MatIconModule } from '@angular/material/icon';
import { MatTableModule } from '@angular/material/table';
import { MatDialogModule } from '@angular/material/dialog';
import { MatSortModule } from '@angular/material/sort';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatCardModule } from '@angular/material/card';
import { MatSnackBarModule } from '@angular/material/snack-bar';

import { registerLocaleData } from '@angular/common';
import localeUk from '@angular/common/locales/uk';

registerLocaleData(localeUk);

const appRoutes: Routes = [
	{ path: '', component: HomeComponent },
	{ path: 'login', component: LoginComponent },
	{ path: 'register', component: RegisterComponent },
	{
		path: 'personal-page',
		component: UserPersonalPageComponent,
		canActivate: [AuthGuard],
	},
	{
		path: 'scheduled-appointments',
		component: ScheduledAppointmentsComponent,
		canActivate: [AuthGuard],
	},
	{
		path: 'visited-appointments',
		component: VisitedAppointmentsComponent,
		canActivate: [AuthGuard],
	},
	{
		path: 'missed-appointments',
		component: MissedAppointmentsComponent,
		canActivate: [AuthGuard],
	},
	{
		path: 'hospital-management',
		component: HospitalManagementComponent,
		canActivate: [AuthGuard, RoleGuard],
		data: { expectedRole: [Roles.MASTER_SUPERVISOR] },
	},
	{
		path: 'user-management',
		component: UserManagementComponent,
		canActivate: [AuthGuard, RoleGuard],
		data: { expectedRole: [Roles.MASTER_SUPERVISOR, Roles.SUPERVISOR] },
	},
	{ path: 'doctors-by-hospital/:id', component: DoctorsByHospitalComponent },
	{ path: 'doctor-schedule', component: DoctorScheduleComponent },
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
		MatCardModule,
		MatIconModule,
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
		DoctorsByHospitalComponent,
		LoginComponent,
		RegisterComponent,
		UserPersonalPageComponent,
		DoctorScheduleComponent,
		CreateAppointmentDialogComponent,
		ScheduledAppointmentsComponent,
		VisitedAppointmentsComponent,
		FeedbackDialogComponent,
		FeedbackListDialogComponent,
		MarkAppointmentAsVisitedDialogComponent,
		MissedAppointmentsComponent,
		HospitalManagementComponent,
		AddHospitalDialogComponent,
		UserManagementComponent,
		UpdateUserDialogComponent,
		AssignEmployeeDialogComponent,
	],
	bootstrap: [AppComponent],
	providers: [
		{ provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },
		{ provide: LOCALE_ID, useValue: 'uk' },
	],
})
export class AppModule {}
