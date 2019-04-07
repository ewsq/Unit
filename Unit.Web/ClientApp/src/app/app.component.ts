import { Component, Inject } from '@angular/core';
import { trigger, state, style, transition, animate, animateChild, group, query } from '@angular/animations'
import { HttpService, HTTP_SERVICE_TOKEN } from './httpservice/httpservice.provider';
import { DicUnit, RepCodes } from './httpservice/httpservice.model';
import { AccountService } from './account/account.service';
import { UploadFile, NzMessageService } from 'ng-zorro-antd';
import { RouterOutlet } from '@angular/router';
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  animations:[
    trigger('routeAnimations', [
      transition('HomePage <=> AboutPage', [
        style({ position: 'relative' }),
        query(':enter, :leave', [
          style({
            position: 'absolute',
            top: 0,
            left: 0,
            width: '100%'
          })
        ]),
        query(':enter', [
          style({ left: '-100%'})
        ]),
        query(':leave', animateChild()),
        group([
          query(':leave', [
            animate('300ms ease-out', style({ left: '100%'}))
          ]),
          query(':enter', [
            animate('300ms ease-out', style({ left: '0%'}))
          ])
        ]),
        query(':enter', animateChild()),
      ]),
      transition('* <=> FilterPage', [
        style({ position: 'relative' }),
        query(':enter, :leave', [
          style({
            position: 'absolute',
            top: 0,
            left: 0,
            width: '100%'
          })
        ]),
        query(':enter', [
          style({ left: '-100%'})
        ]),
        query(':leave', animateChild()),
        group([
          query(':leave', [
            animate('200ms ease-out', style({ left: '100%'}))
          ]),
          query(':enter', [
            animate('300ms ease-out', style({ left: '0%'}))
          ])
        ]),
        query(':enter', animateChild()),
      ])
    ])
  ]
})
export class AppComponent {
  title = 'Angular';
  visible = false;
  uploading = false;
  fileList: UploadFile[] = [];
  datas:DicUnit[]=[];
  constructor(@Inject(HTTP_SERVICE_TOKEN) public hs:HttpService,
  public ac:AccountService,
  public ms:NzMessageService){
    this.updateDics();
  }
  updateDics(){
    this.hs.getDics().subscribe(r=>{
      this.datas=r;
    });
  }
  logout(){
   this.hs.logout();
   this.ac.token=null; 
  }
  beforeUpload = (file: UploadFile): boolean => {
    this.fileList = this.fileList.concat(file);
    return false;
  };
  prepareRoute(outlet: RouterOutlet) {
    return outlet && outlet.activatedRouteData && outlet.activatedRouteData['animation'];
  }
  handleUpload(): void {
    this.hs.uploadHeadImg(this.fileList[0]).subscribe(r=>{
      if(r.infoType==RepCodes.Succeed){
        this.ms.success("上传成功");
        this.hs.getUserInfo().subscribe(i=>{
          this.ac.headImgPath=this.hs.getHeadImg(i.entity.himg);
        })
      }else{
        this.ms.error("上传失败，原因是:"+r.msg);
      }
    });
  }
}
