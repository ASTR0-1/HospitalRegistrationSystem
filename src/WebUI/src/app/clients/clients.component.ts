import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { Client } from '../entities/client';
import { ClientService } from '../services/client.service';

@Component({
	selector: 'app-clients',
	templateUrl: './clients.component.html',
	styleUrls: ['./clients.component.css'],
	providers: [ClientService],
})
export class ClientsComponent implements OnInit {
	clients: Client[] = [];

	hasPrevious: boolean = false;
	hasNext: boolean = false;

	pageNumber: number = 1;

	searchString: string | null = null;

	constructor(
		private clientService: ClientService,
		private route: ActivatedRoute
	) {}

	ngOnInit(): void {
		this.searchString =
			this.route.snapshot.queryParamMap.get('SearchString');

		if (this.searchString) {
			this.searchClients(this.searchString);
		} else {
			this.loadClients();
		}
	}

	getEmojiGender(client: Client) {
		if (client.gender === 'Male') {
			return '👨';
		} else if (client.gender === 'Female') {
			return '👩';
		} else {
			return '❔';
		}
	}

    getClientFullName(client: Client): string {
        return client.firstName + ' ' + client.middleName + ' ' + client.lastName;
    }

	prevPage() {
		if (this.hasPrevious && this.pageNumber > 1 && this.searchString) {
			this.pageNumber--;
			this.searchClients(this.searchString);
		} else if (this.hasPrevious && this.pageNumber > 1) {
			this.pageNumber--;
			this.loadClients();
		}
	}

	nextPage() {
		if (this.hasNext && this.searchString) {
			this.pageNumber++;
			this.searchClients(this.searchString);
		} else if (this.hasNext) {
			this.pageNumber++;
			this.loadClients();
		}
	}

	private loadClients() {
		this.clientService.getClients(this.pageNumber).subscribe((resp) => {
			var pagingParams = JSON.parse(
				resp.headers.get('x-pagination') || '{}'
			);

			this.hasPrevious = pagingParams.HasPrevious;
			this.hasNext = pagingParams.HasNext;

			this.clients = <Client[]>resp.body;
		});
	}

	private searchClients(searchString: string) {
		this.clientService.searchClients(this.pageNumber, searchString)
			.subscribe((resp) => {
				var pagingParams = JSON.parse(
					resp.headers.get('x-pagination') || '{}'
				);

				this.hasPrevious = pagingParams.HasPrevious;
				this.hasNext = pagingParams.HasNext;

				this.clients = <Client[]>resp.body;
			});
	}
}
