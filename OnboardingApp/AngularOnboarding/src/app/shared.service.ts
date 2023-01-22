import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders} from '@angular/common/http';
import { Observable, Subject } from 'rxjs';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';

@Injectable({ providedIn: 'root'})

export class SharedService {
readonly APIUrl = "http://localhost:5000/api"

  constructor(private http:HttpClient, private _jwtHelper: JwtHelperService,  private router: Router,  public translate:TranslateService) {
    
  }

  public projectId: number | undefined;
  public projectName: string | undefined;

  private _authChangeSub = new Subject<boolean>()
  public authChanged = this._authChangeSub.asObservable();

  public sendAuthStateChangeNotification = (isAuthenticated: boolean) => {
    this._authChangeSub.next(isAuthenticated);
  }

  public logout = () => {
    localStorage.removeItem("token");
    localStorage.removeItem("role");
    localStorage.removeItem("username");
    localStorage.removeItem("id");
    this.sendAuthStateChangeNotification(false);
  }

  public isUserAuthenticated = (): boolean => {
    const token = localStorage.getItem("token");
    if(token != null){
      if(this._jwtHelper.isTokenExpired(token)){
        return false;
      }
      else{
        return true;
      }
    }
    else
      return false;
  }
  
  public TimeOut = () => {
    this.logout();
    this.router.navigate(["/login"]);
  }

  GetProgrammers():Observable<any[]>{
    /**
    if(localStorage.getItem("token") != null){}
    let header = new HttpHeaders().set(
      "Authorization",
      "Bearer "+localStorage.getItem("token") || ""
    );
    **/
    return this.http.get<any>(this.APIUrl+'/Tasks/GetProgrammers'/** , {headers:header}**/);
  }

  GetProjects():Observable<any[]>{
    return this.http.get<any>(this.APIUrl+'/Projects/GetProjects');
  }

  GetTasks(val:any):Observable<any[]>{
    return this.http.post<any>(this.APIUrl+'/Tasks/GetTasks',val);
  }

  CreateProject(val:any){
    return this.http.post(this.APIUrl+'/Projects/CreateProjectAsync', val);
  }

  UpdateProject(val:any, val2:any){
    var data = {'model': val, 'projectId': val2 };

    return this.http.post(this.APIUrl+'/Projects/EditProjectAsync', data);
  }

  DeleteProject(val:any){
    return this.http.post(this.APIUrl+'/Projects/DeleteProjectAsync', val);
  }

  CreateTask(val:any){
    return this.http.post(this.APIUrl+'/Tasks/CreateTaskAsync', val);
  }

  UpdateTask(val:any, val2:any){
    var data = {'model': val, 'taskId': val2 };
    return this.http.post(this.APIUrl+'/Tasks/EditTaskAsync', data);
  }

  DeleteTask(val:any){
    return this.http.post(this.APIUrl+'/Tasks/DeleteTaskAsync', val);
  }

  Login(val:any){
    return this.http.post(this.APIUrl+'/Users/Login', val, {headers:{skip:"true"}});
  }

  SignUp(val:any){
    return this.http.post(this.APIUrl+'/Users/Register', val, {headers:{skip:"true"}});
  }
}
