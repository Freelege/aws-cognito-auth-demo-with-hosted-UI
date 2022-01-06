import { Component, AfterViewInit } from '@angular/core';
import { Auth } from 'aws-amplify';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements AfterViewInit {

  token: any;  
  email: string;
  userName: string;
  phone: string;

  constructor() {}

  ngAfterViewInit() {
    Auth.currentUserInfo()
    .then((auth) => {
        this.userName = auth.username;
        this.phone = auth.attributes.phone_number;
        this.email = auth.attributes.email;
    });

    Auth.currentSession()
    .then((auth) => {
        let jwt = auth.getIdToken();
        this.token = jwt.getJwtToken();
    });
  }

  onLogoutClick() {

    console.log("Logout Clicked");

    Auth.signOut({ global: true })
      .then(data => console.log(data))
      .catch(err => console.log(err));
  }
}