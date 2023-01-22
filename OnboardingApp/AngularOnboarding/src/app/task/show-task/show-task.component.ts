import { Component, OnInit, TemplateRef } from '@angular/core';
import { SharedService } from 'src/app/shared.service';

import { BsModalService, ModalOptions } from 'ngx-bootstrap/modal';
import { BsModalRef } from 'ngx-bootstrap/modal/bs-modal-ref.service';

@Component({
  selector: 'app-show-task',
  templateUrl: './show-task.component.html',
  styleUrls: ['./show-task.component.css']
})
export class ShowTaskComponent implements OnInit {

  constructor(private service:SharedService, private modalService: BsModalService) { }

  TaskList:any=[];
  ProgrammersList:any=[];

  ModalTitle:string | undefined;
  ActivateAddEditTask:boolean=false;
  task:any;
  modalRef?: BsModalRef;
  taskStates:any;
  userRole:any;
  
  projectId:number | undefined;
  projectName:string| undefined;

  config: ModalOptions = {};
  
  ngOnInit(): void {
    this.projectId = this.service.projectId;
    this.projectName = this.service.projectName;
    this.userRole = localStorage.getItem("role");
    this.refreshTaskList();
  }

  refreshTaskList(){
    var taskStates = { todo: 1, inprogress: 2, finished: 3,
      [1]: "To Do", [2]: "In Progress", [3]: "Finished" }
    this.taskStates = taskStates;
    this.service.GetTasks(this.projectId).subscribe(data=>{
      this.TaskList=data
    });
    this.service.GetProgrammers().subscribe(data=>{
      this.ProgrammersList=data;
    });
  }

  addClick(template: TemplateRef<any>){
    this.task={
      id:0,
      projectid:this.projectId
    };
    this.ModalTitle="Add Task";
    this.ActivateAddEditTask=true;
    this.modalRef = this.modalService.show(template, this.config);
  }

  editClick(item: any, template: TemplateRef<any>){
    this.task=item;
    this.task.projectid = this.projectId;
    this.ModalTitle="Edit Task";
    this.ActivateAddEditTask=true;
    this.modalRef = this.modalService.show(template);
  }

  deleteClick(item: any){
    if(confirm("Are you sure?")){
      this.service.DeleteTask(item).subscribe(data=>{
          this.refreshTaskList();
        })
    }
  }

  closeClick(){
    this.ActivateAddEditTask=false;
    this.refreshTaskList();
  }
}
