import { NgModule, inject } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { Routes, RouterModule } from '@angular/router';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';

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

const appRoutes: Routes = [
	{ path: '', component: HomeComponent },
	{ path: 'login', component: LoginComponent },
	{ path: 'register', component: RegisterComponent },
	{ path: 'personal-page', component: UserPersonalPageComponent, canActivate: [AuthGuard] },
	{ path: '**', component: NotFoundComponent },
];

@NgModule({
	imports: [
		BrowserModule,
		FormsModule,
		HttpClientModule,
		MatFormFieldModule,
		RouterModule.forRoot(appRoutes),
	],
	declarations: [
		AppComponent,
		HomeComponent,
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
