import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { SharedService } from 'src/app/shared.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{
  
  title = 'AngularOnboarding';
  userName: string | null | undefined;
  public isUserAuthenticated: boolean | undefined;

  constructor(private service:SharedService, private router: Router, public translate:TranslateService) { 
    translate.addLangs(['en', 'pt']);
    translate.setDefaultLang('en');
  }

  switchLanguage(lang: string){
    this.translate.use(lang);
  }

  ngOnInit(): void {
    //this.router.navigate(["/login"]);
    this.service.authChanged
    .subscribe(res => {
      this.isUserAuthenticated = res;
      this.userName = localStorage.getItem("username");
    })
    if(localStorage.getItem("id")){
      this.service.sendAuthStateChangeNotification(true);
      this.router.navigate(["/project"]);
    }
    else{
      this.router.navigate(["/login"]);
    }
  }

  public logout = () => {
    this.service.logout();
    this.router.navigate(["/login"]);
  }
  
}

