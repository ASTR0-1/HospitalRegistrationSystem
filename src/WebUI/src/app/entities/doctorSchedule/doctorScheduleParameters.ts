import { PagingParameters } from '../utility/pagingParameters';

export interface DoctorScheduleParameters extends PagingParameters {
	from: string; // DateOnly in C#
	to: string; // DateOnly in C#
}
