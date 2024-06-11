export interface DoctorScheduleForManipulationDto {
	id: number;
	date: string; // DateOnly in C#
	workingHoursList: number[];
}
