import { NgModule, inject } from '@angular/core';
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

import { UserPersonalPageComponent } from './user-personal-page/user-personal-page.component';
import { ClientAppointmentsComponent } from './appointments/client-appointments/client.appointments.component';
import { ClientVisitedAppointmentsComponent } from './appointments/client-visited-appointments/client.appointments.visited.component';
import { DoctorAppointmentsComponent } from './appointments/doctor-appointments/doctor.appointments.component';
import { MarkAppointmentComponent } from './appointments/mark-appointment/mark.appointment.component';
import { AddAppointmentComponent } from './appointments/add-appointment/add.appointment.component';
import { LoginComponent } from './authentication/login/login.component';
import { RegisterComponent } from './authentication/register/register.component';

import { AuthInterceptor } from './interceptors/auth.interceptor';
import { AuthGuard } from './guards/auth.guard';
import { MatPaginatorModule } from '@angular/material/paginator';
import { HospitalManagementComponent } from './hospital-management/hospital-management.component';
import { MatTableModule } from '@angular/material/table';
import { RoleGuard } from './guards/role.guard';
import { AddHospitalDialogComponent } from './hospital-management/add-hospital-dialog/add-hospital-dialog.component';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule } from '@angular/material/dialog';
import { MatSortModule } from '@angular/material/sort';
import { MatSelectModule } from '@angular/material/select';

const appRoutes: Routes = [
	{ path: '', component: HomeComponent },
	{ path: 'login', component: LoginComponent },
	{ path: 'register', component: RegisterComponent },
	{ path: 'personal-page', component: UserPersonalPageComponent, canActivate: [AuthGuard] },
	{ path: 'hospital-management', component: HospitalManagementComponent, canActivate: [AuthGuard, RoleGuard], data: { expectedRole: Roles.MASTER_SUPERVISOR } },
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
		HospitalManagementComponent,
		AddHospitalDialogComponent,
		NotFoundComponent,
		ClientAppointmentsComponent,
		ClientVisitedAppointmentsComponent,
		DoctorAppointmentsComponent,
		MarkAppointmentComponent,
		AddAppointmentComponent,
		LoginComponent,
		RegisterComponent,
		UserPersonalPageComponent
	],
	bootstrap: [AppComponent],
	providers: [
		{ provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },
	],
})
export class AppModule {}
