import { ApplicationUserDto } from '../applicationUser/applicationUserDto';

export interface AppointmentDto {
	id: number;
	visitTime: Date;
	diagnosis: string;
	isVisited: boolean;
	doctor: ApplicationUserDto;
	client: ApplicationUserDto;
}
