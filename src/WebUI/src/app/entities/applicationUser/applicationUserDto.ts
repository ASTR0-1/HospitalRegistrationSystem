export interface ApplicationUserDto {
	id: number;
	firstName: string;
	middleName: string;
	lastName: string;
	gender: string;
	specialty?: string;
	visitCost?: number;
	totalServiceCost?: number;
	hospitalId?: number;
	profilePhotoUrl?: string;
}
