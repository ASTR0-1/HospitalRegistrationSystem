import { Component } from '@angular/core';
import { Location } from '@angular/common';

import { ClientForCreation } from 'src/app/entities/clientForCreation';
import { ClientService } from 'src/app/services/client.service';

@Component({
	selector: 'app-clients',
	templateUrl: './add.client.component.html',
	styleUrls: ['./add.client.component.css'],
	providers: [ClientService],
})
export class AddClientComponent {
	clientToAdd: ClientForCreation = new ClientForCreation();

	constructor(private clientService: ClientService, private location: Location) {}

	addClient() {
		this.clientService.postClient(this.clientToAdd).subscribe({
			error: (error) => console.log(error),
		});

        this.location.back();
	}
}
