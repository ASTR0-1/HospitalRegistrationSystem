import { Component, OnInit } from '@angular/core';
import { Router } from "@angular/router";

@Component({
	selector: 'app-home',
	templateUrl: './home.component.html',
	styleUrls: ['./home.component.css'],
})
export class HomeComponent implements OnInit {
	constructor(private router: Router) {}

	ngOnInit(): void {}

    searchClients(searchString: string) {
        this.router.navigate(['clients'], {
			queryParams: { SearchString: searchString },
		});
    }

    searchDoctors(searchString: string) {
        this.router.navigate(['doctors'], {
			queryParams: { SearchString: searchString },
		});
    }
}
