import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { Routes, RouterModule } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';

import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { NotFoundComponent } from './not-found/not-found.component';

import { ClientsComponent } from './clients/clients.component';
import { AddClientComponent } from './clients/add-client/add.client.component';

import { DoctorsComponent } from './doctors/doctors.component';
import { AddDoctorComponent } from './doctors/add-doctor/add.doctor.component';

import { ClientAppointmentsComponent } from './appointments/client-appointments/client.appointments.component';
import { ClientVisitedAppointmentsComponent } from './appointments/client-visited-appointments/client.appointments.visited.component';
import { DoctorAppointmentsComponent } from './appointments/doctor-appointments/doctor.appointments.component';
import { MarkAppointmentComponent } from './appointments/mark-appointment/mark.appointment.component';
import { AddAppointmentComponent } from './appointments/add-appointment/add.appointment.component';

const appRoutes: Routes = [
	{ path: '', component: HomeComponent },
	{ path: 'clients', component: ClientsComponent },
	{ path: 'clients/new', component: AddClientComponent },
	{
		path: 'clients/:clientId/appointments',
		component: ClientAppointmentsComponent,
	},
	{
		path: 'clients/:clientId/appointments/visited',
		component: ClientVisitedAppointmentsComponent,
	},
	{
		path: 'clients/:clientId/appointments/markAsVisited',
		component: MarkAppointmentComponent,
	},
	{ path: 'doctors', component: DoctorsComponent },
	{ path: 'doctors/new', component: AddDoctorComponent },
	{
		path: 'doctors/:doctorId/appointments',
		component: DoctorAppointmentsComponent,
	},
	{
		path: 'doctors/:doctorId/appointments/markAsVisited',
		component: MarkAppointmentComponent,
	},
	{
		path: 'doctors/:doctorId/appointments/new',
		component: AddAppointmentComponent,
	},
	{ path: '**', component: NotFoundComponent },
];

@NgModule({
	imports: [
		BrowserModule,
		FormsModule,
		HttpClientModule,
		RouterModule.forRoot(appRoutes),
	],
	declarations: [
		AppComponent,
		HomeComponent,
		NotFoundComponent,
		ClientsComponent,
		AddClientComponent,
		DoctorsComponent,
		AddDoctorComponent,
		ClientAppointmentsComponent,
		ClientVisitedAppointmentsComponent,
		DoctorAppointmentsComponent,
		MarkAppointmentComponent,
		AddAppointmentComponent,
	],
	bootstrap: [AppComponent],
	providers: [],
})
export class AppModule {}
