import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { DoctorScheduleDto } from '../entities/doctorSchedule/doctorScheduleDto';
import { DoctorScheduleForManipulationDto } from '../entities/doctorSchedule/doctorScheduleForManipulationDto';
import { DoctorScheduleParameters } from '../entities/doctorSchedule/doctorScheduleParameters';
import { AuthenticationService } from '../services/authentication.service';
import { Roles } from '../constants/role.constants';
import { DoctorScheduleService } from '../services/doctorSchedule.service';
import { MatDialog } from '@angular/material/dialog';
import { AppointmentService } from '../services/appointment.service';
import { AppointmentForCreationDto } from '../entities/appointment/appointmentForCreationDto';
import { CreateAppointmentDialogComponent } from './create-appointment-dialog/create-appointment-dialog.component';

@Component({
	selector: 'app-doctor-schedule',
	templateUrl: './doctor-schedule.component.html',
	styleUrls: ['./doctor-schedule.component.css'],
})
export class DoctorScheduleComponent implements OnInit {
	currentDate: Date = new Date();
	initialDate: Date = new Date();
	weekDays: Date[] = [];
	hours: number[] = Array.from({ length: 24 }, (_, i) => i);
	schedule: { [key: string]: number[] } = {};
	existingSchedules: { [key: string]: DoctorScheduleDto } = {};

	constructor(
		private doctorScheduleService: DoctorScheduleService,
		private authService: AuthenticationService,
		private route: ActivatedRoute,
		private dialog: MatDialog,
		private appointmentService: AppointmentService
	) {}

	ngOnInit(): void {
		this.updateWeekDays();
		this.loadSchedule();
	}

	updateWeekDays(): void {
		this.weekDays = [];
		for (let i = 0; i < 7; i++) {
			const date = new Date(this.currentDate);
			date.setDate(date.getDate() - date.getDay() + i);
			this.weekDays.push(date);
		}
	}

	loadSchedule(): void {
		const startOfWeek = this.weekDays[0].toISOString().split('T')[0];
		const endOfWeek = this.weekDays[6].toISOString().split('T')[0];
		const params: DoctorScheduleParameters = {
			from: startOfWeek,
			to: endOfWeek,
			pageNumber: 1,
			pageSize: 7,
		};

		let doctorId = this.route.snapshot.queryParams['doctorId'];

		this.doctorScheduleService
			.getDoctorSchedules(params, doctorId)
			.subscribe((response) => {
				response.body?.forEach((schedule: DoctorScheduleDto) => {
					const dateStr = schedule.date;
					this.schedule[dateStr] = schedule.workingHoursList;
					this.existingSchedules[dateStr] = schedule; // Store existing schedules
				});
			});
	}

	previousWeek(): void {
		if (!this.isInitialWeek()) {
			this.currentDate.setDate(this.currentDate.getDate() - 7);
			this.updateWeekDays();
			this.loadSchedule();
		}
	}

	nextWeek(): void {
		this.currentDate.setDate(this.currentDate.getDate() + 7);
		this.updateWeekDays();
		this.loadSchedule();
	}

	isInitialWeek(): boolean {
		const startOfWeek = new Date(this.currentDate);
		startOfWeek.setDate(startOfWeek.getDate() - startOfWeek.getDay());
		const startOfInitialWeek = new Date(this.initialDate);
		startOfInitialWeek.setDate(
			startOfInitialWeek.getDate() - startOfInitialWeek.getDay()
		);
		return startOfWeek <= startOfInitialWeek;
	}

	isScheduled(day: Date, hour: number): boolean {
		const dateStr = this.toLocalISOString(day).split('T')[0];
		return this.schedule[dateStr]?.includes(hour) || false;
	}

	isDoctor(): boolean {
		let doctorIdFromStorage = localStorage.getItem('userId');
		let doctorId = this.route.snapshot.queryParams['doctorId'];

		return (
			this.authService.hasRole(Roles.DOCTOR) &&
			doctorIdFromStorage?.toString() === doctorId.toString()
		);
	}

	toggleSchedule(day: Date, hour: number): void {
		const dateStr = this.toLocalISOString(day).split('T')[0];
		if (!this.schedule[dateStr]) {
			this.schedule[dateStr] = [];
		}
		const index = this.schedule[dateStr].indexOf(hour);
		if (index === -1) {
			this.schedule[dateStr].push(hour);
		} else {
			this.schedule[dateStr].splice(index, 1);
		}
	}

	openAppointmentDialog(day: Date, hour: number): void {
		if (this.isScheduled(day, hour)) {
			const dialogRef = this.dialog.open(
				CreateAppointmentDialogComponent,
				{
					data: { day, hour },
				}
			);

			dialogRef.afterClosed().subscribe((result) => {
				if (result) {
					const dateStr = this.toLocalISOString(day).split('T')[0];
					const doctorId = this.route.snapshot.queryParams['doctorId'];
					const clientId = +localStorage.getItem('userId')!;

					const visitTime = new Date(dateStr);
          			visitTime.setHours(hour, 0, 0, 0);

					const localVisitTime = new Date(visitTime.getTime() - visitTime.getTimezoneOffset() * 60000);

					const appointment: AppointmentForCreationDto = {
						visitTime: localVisitTime,
						doctorId: +doctorId,
						clientId: clientId,
					};

					this.appointmentService.addNew(appointment).subscribe(() => {
						this.loadSchedule();
					});
				}
			});
		}
	}

	saveSchedule(): void {
		for (const [date, hours] of Object.entries(this.schedule)) {
			if (this.existingSchedules[date]) {
				// Update existing schedule
				const scheduleDto: DoctorScheduleForManipulationDto = {
					id: this.existingSchedules[date].id,
					date,
					workingHoursList: hours,
				};
				this.doctorScheduleService
					.updateDoctorSchedule(scheduleDto)
					.subscribe();
			} else {
				// Create new schedule
				const scheduleDto: DoctorScheduleForManipulationDto = {
					id: 0,
					date,
					workingHoursList: hours,
				};
				this.doctorScheduleService
					.createDoctorSchedule(scheduleDto)
					.subscribe();
			}
		}
	}

	toLocalISOString(date: Date): string {
		const tzo = -date.getTimezoneOffset(),
			dif = tzo >= 0 ? '+' : '-',
			pad = function (num: number) {
				const norm = Math.floor(Math.abs(num));
				return (norm < 10 ? '0' : '') + norm;
			};
		return (
			date.getFullYear() +
			'-' +
			pad(date.getMonth() + 1) +
			'-' +
			pad(date.getDate()) +
			'T' +
			pad(date.getHours()) +
			':' +
			pad(date.getMinutes()) +
			':' +
			pad(date.getSeconds()) +
			dif +
			pad(tzo / 60) +
			':' +
			pad(tzo % 60)
		);
	}
}
