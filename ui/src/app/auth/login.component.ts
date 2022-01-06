import { Component, OnInit, NgZone } from '@angular/core';
import { Router } from '@angular/router';
import { Auth, Hub } from 'aws-amplify';
import { CognitoHostedUIIdentityProvider } from '@aws-amplify/auth';

@Component({
  selector: 'app-login',
  template: '<div></div>'
})
export class LoginComponent implements OnInit {

  constructor(
    private router: Router,
    private zone: NgZone) {


    // Used for listening to login events
    Hub.listen("auth", ({ payload: { event, data } }) => {
      if (event === "cognitoHostedUI" || event === "signedIn") {
        console.log(event);
        this.zone.run(() => this.router.navigate(['/dashboard']));
      } 
    });

    //currentAuthenticatedUser: when user comes to login page again
    Auth.currentAuthenticatedUser()
      .then(() => {
        this.router.navigate(['/dashboard'], { replaceUrl: true });
      }).catch((err) => {
        console.log(err);
      })

  }

  ngOnInit() { 
    Auth.federatedSignIn(
     // {provider: CognitoHostedUIIdentityProvider.Google}  --- with google sign in option
    );
  }
}