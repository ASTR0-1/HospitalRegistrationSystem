export interface DoctorScheduleDto {
	id: number;
	doctorId: number;
	workingHoursList: number[];
	date: string; // DateOnly in C#
}
