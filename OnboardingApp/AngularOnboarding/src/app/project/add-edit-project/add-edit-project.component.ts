import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { SharedService } from 'src/app/shared.service';

@Component({
  selector: 'app-add-edit-project',
  templateUrl: './add-edit-project.component.html',
  styleUrls: ['./add-edit-project.component.css']
})
export class AddEditProjectComponent implements OnInit {
  

  constructor(private service:SharedService, private modalService: BsModalService) { }


  @Input() project:any;
  @Output() someEvent = new EventEmitter<any>();
  Id:number| undefined;
  Name:string | undefined;
  Money:number | undefined;
  placeholderName:string| undefined;
  placeholderMoney:number| undefined;



  ngOnInit(): void {
    this.Id=this.project.id;
    this.Name=this.project.name;
    this.Money=this.project.money;
    if(this.Id == 0){
      this.placeholderName = "";
      this.placeholderMoney = 0;
    }
    else{
      this.placeholderName = this.Name;
      this.placeholderMoney = this.Money;
    }
  }

  addProject(){
    var val = {Name:this.Name,
              Money:this.Money};
    this.service.CreateProject(val).subscribe(res=>{
      this.modalService.hide();
      this.someEvent.next('');
    },(error) => {
      alert("Failed at creating project");
    })
  }

  updateProject(){
    var val = {Name:this.Name,
              Money:this.Money};
    this.service.UpdateProject(val, this.Id).subscribe(res=>{
      this.modalService.hide();
      this.someEvent.next('');
    },(error) => {
      alert("Failed at updating project");
    })
  }
}
