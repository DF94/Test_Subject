import { Component, OnInit, TemplateRef} from '@angular/core';
import { SharedService } from 'src/app/shared.service';

import { BsModalService, ModalOptions } from 'ngx-bootstrap/modal';
import { BsModalRef } from 'ngx-bootstrap/modal/bs-modal-ref.service';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-show-project',
  templateUrl: './show-project.component.html',
  styleUrls: ['./show-project.component.css']
})
export class ShowProjectComponent implements OnInit {
  

  constructor(private service:SharedService, private modalService: BsModalService, private router: Router, public translate:TranslateService) { }

  ProjectList:any=[];

  ModalTitle:string | undefined;
  ActivateAddEditProject:boolean=false;
  project:any;
  modalRef?: BsModalRef;
  userRole: any;

  config: ModalOptions = { /** class: 'modal-lg' **/ };

  ngOnInit(): void {
    this.userRole = localStorage.getItem("role");
    this.refreshProjectList();
  }

  refreshProjectList(){
    this.service.GetProjects().subscribe(data=>{
      this.ProjectList=data
    });
  }

  addClick(template: TemplateRef<any>){
    this.project={
      id:0,
      name:"",
      money:0
    }
    this.ModalTitle="Add Project"
    this.ActivateAddEditProject=true;
    this.modalRef = this.modalService.show(template, this.config);
  }

  editClick(item: any, template: TemplateRef<any>){
    this.project=item;
    this.ModalTitle="Edit Project";
    this.ActivateAddEditProject=true;
    this.modalRef = this.modalService.show(template);
  }

  detailsClick(item: any){
    this.service.projectId = item.id;
    this.service.projectName = item.name;
    this.router.navigate(["/task"]);
  }

  deleteClick(item: any){
    if(confirm("Are you sure?")){
      this.service.DeleteProject(item).subscribe(data=>{
          this.refreshProjectList();
        })
    }
  }

  closeClick(){
    this.ActivateAddEditProject=false;
    this.refreshProjectList();
  }
}


