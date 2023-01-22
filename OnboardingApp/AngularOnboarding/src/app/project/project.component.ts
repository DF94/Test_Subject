import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { SharedService } from '../shared.service';

@Component({
  selector: 'app-project',
  templateUrl: './project.component.html',
  styleUrls: ['./project.component.css']
})
export class ProjectComponent implements OnInit {

  constructor(public translate:TranslateService) { }

  ngOnInit(): void {
  }

}
