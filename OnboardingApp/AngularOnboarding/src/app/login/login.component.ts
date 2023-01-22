import { Component, OnInit } from '@angular/core';
import { SharedService } from 'src/app/shared.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  constructor(private service:SharedService,  private router: Router) { }

  Email:string | undefined;
  Password:number | undefined;

  ngOnInit(): void {
    }

    login(){
      if(localStorage.getItem("token") != null){
        this.service.logout();
      }
      var val = {Email:this.Email,
                Password:this.Password};
      this.service.Login(val).subscribe((res:any)=>{
        localStorage.setItem("token", res.token);
        localStorage.setItem("role", res.role);
        localStorage.setItem("username", res.username);
        localStorage.setItem("id", res.userId);
        this.service.sendAuthStateChangeNotification(res.isAuthSuccessful);
        this.router.navigate(["/project"]);
      },
      (error) => {
        alert("Failed logging in");
      })
    }

    
}