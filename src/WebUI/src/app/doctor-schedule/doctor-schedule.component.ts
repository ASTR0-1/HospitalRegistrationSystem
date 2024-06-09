import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { DoctorScheduleDto } from '../entities/doctorSchedule/doctorScheduleDto';
import { DoctorScheduleForManipulationDto } from '../entities/doctorSchedule/doctorScheduleForManipulationDto';
import { DoctorScheduleParameters } from '../entities/doctorSchedule/doctorScheduleParameters';
import { AuthenticationService } from '../services/authentication.service';
import { Roles } from '../constants/role.constants';
import { DoctorScheduleService } from '../services/doctorSchedule.service';

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
    private route: ActivatedRoute
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
    if (this.authService.hasRole(Roles.DOCTOR)) {
      doctorId = localStorage.getItem('userId');
    }
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
    const dateStr = day.toISOString().split('T')[0];
    return this.schedule[dateStr]?.includes(hour) || false;
  }

  isDoctor(): boolean {
    return this.authService.hasRole(Roles.DOCTOR);
  }

  toggleSchedule(day: Date, hour: number): void {
    const dateStr = day.toISOString().split('T')[0];
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

  saveSchedule(): void {
    for (const [date, hours] of Object.entries(this.schedule)) {
      if (this.existingSchedules[date]) {
        // Update existing schedule
        const scheduleDto: DoctorScheduleForManipulationDto = {
          id: this.existingSchedules[date].id,
          date,
          workingHoursList: hours,
        };
        this.doctorScheduleService.updateDoctorSchedule(scheduleDto).subscribe();
      } else {
        // Create new schedule
        const scheduleDto: DoctorScheduleForManipulationDto = {
          id: 0,
          date,
          workingHoursList: hours,
        };
        this.doctorScheduleService.createDoctorSchedule(scheduleDto).subscribe();
      }
    }
  }
}
