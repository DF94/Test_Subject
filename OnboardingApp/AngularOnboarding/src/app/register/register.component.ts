import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { SharedService } from '../shared.service';

interface Role {
  value: string;
}

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})

export class RegisterComponent implements OnInit {

  constructor(private service:SharedService, private router: Router) { }

  Name:string | undefined;
  Email:string | undefined;
  Password:string | undefined;
  Role:string | undefined;
  

  roles: Role[] = [
    {value: 'Manager'},
    {value: 'Programmer'},
  ];

  ngOnInit(): void {
  }

  addUser(){
    var val = {Name:this.Name,
              Email:this.Email,
              Password:this.Password,
              Role:this.Role};
    this.service.SignUp(val).subscribe((res:any)=>{
      this.router.navigate(["/login"]);
    },(error) => {
      alert("Failed at creating user");
    })
  }
}
