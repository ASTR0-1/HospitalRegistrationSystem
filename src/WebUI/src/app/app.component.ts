import { Component, OnInit } from '@angular/core';
import { map, share, Subscription, timer } from 'rxjs';

@Component({
	selector: 'app-root',
	templateUrl: './app.component.html',
	styleUrls: ['./app.component.css'],
})
export class AppComponent implements OnInit {
	rxTime = new Date();
	intervalId: any;
	subscription!: Subscription;

	ngOnInit() {
		this.subscription = timer(0, 1000)
			.pipe(
				map(() => new Date()),
				share()
			)
			.subscribe((time) => {
				this.rxTime = time;
			});
	}

	ngOnDestroy() {
		clearInterval(this.intervalId);
		if (this.subscription) {
			this.subscription.unsubscribe();
		}
	}
}
