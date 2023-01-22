import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { BsModalService } from 'ngx-bootstrap/modal';
import { SharedService } from 'src/app/shared.service';

@Component({
  selector: 'app-add-edit-task',
  templateUrl: './add-edit-task.component.html',
  styleUrls: ['./add-edit-task.component.css']
})
export class AddEditTaskComponent implements OnInit {

  constructor(private service:SharedService, private modalService: BsModalService) { }

  @Input() task:any; 
  @Input() ProgrammersList:any=[];
  @Output() someEvent = new EventEmitter<any>();
  Id:number | undefined;
  Name:string | undefined;
  State:number | undefined;
  LimitDate:Date | undefined;
  ProjectId:number | undefined;
  placeholderName:string| undefined;
  placeholderDate:Date| undefined;
  taskStates:any;
  ProgrammerList:any=[];
  Programmer:string | undefined | null;
  userRole:any;
  
  ngOnInit(): void {
    this.userRole = localStorage.getItem("role");
    this.Id=this.task.id;
    this.Name=this.task.name;
    this.State=this.task.state;
    this.LimitDate=this.task.limitDate;
    this.ProjectId=this.task.projectid;
    this.ProgrammerList = this.ProgrammersList;
    if(this.userRole == "Programmer"){
      this.Programmer = localStorage.getItem("id");
    }
    var taskStates = { todo: 1, inprogress: 2, finished: 3,
      [1]: "To Do", [2]: "In Progress", [3]: "Finished" }
    this.taskStates = taskStates;
    if(this.Id == 0){
      this.placeholderName = "";
      this.placeholderDate = new Date(), 'dd/MM/yyyy', this.LimitDate;
    }
    else{
      this.placeholderName = this.Name;
      this.placeholderDate = new Date(), 'dd/MM/yyyy', this.LimitDate;
      //alert(this.placeholderDate);
    }
  }

  addTask(){
    var val = {Name:this.Name,
      State:Number(this.State),
      LimitDate:this.LimitDate,
      Programmer:Number(this.Programmer),
      Project:this.ProjectId
    };
    this.service.CreateTask(val).subscribe(res=>{
    this.modalService.hide();
    this.someEvent.next('');

    },(error) => {
    alert("Failed at creating task");
    })
  }

  updateTask(){
    var val = {Name:this.Name,
      State:Number(this.State),
      LimitDate:this.LimitDate,
      Programmer:Number(this.Programmer),
      Project:this.ProjectId
    };
    this.service.UpdateTask(val, this.Id).subscribe(res=>{
      this.modalService.hide();
      this.someEvent.next('');
    },(error) => {
      alert("Failed at updating task");
    })
  }
}

