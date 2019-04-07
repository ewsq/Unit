import { Component, OnInit, Inject } from '@angular/core';
import { HttpRequest } from '@angular/common/http';
import { UploadFile, UploadFilter, NzMessageService } from 'ng-zorro-antd';
import { HTTP_SERVICE_TOKEN, HttpService } from '../httpservice/httpservice.provider';
import { FormBuilder, Validators, FormGroup, FormControl, AbstractControl } from '@angular/forms';
import { WordUnit, DicUnit, RepCodes } from '../httpservice/httpservice.model';
import { Observable } from 'rxjs';
import { Router } from '@angular/router';

@Component({
  selector: 'app-cunit',
  templateUrl: './cunit.component.html',
  styleUrls: ['./cunit.component.css']
})
export class CunitComponent{
  
  uploading = false;
  fileList: UploadFile[]=[];
  indexer:number=0;
  controlArray: Array<{ idt: number,idr:number, tip:string,remember:string}> = [];
  fg:FormGroup;
  rev:boolean=false;
  filters:UploadFilter[]=[
  ];
  constructor(@Inject(HTTP_SERVICE_TOKEN) private hs:HttpService,
    private fb:FormBuilder,
    private ms:NzMessageService,
    private router:Router){
      if(!this.hs.isLogin){
        location.href='/login';
        return;
      }
      this.fg=fb.group({
        dname:[null,[Validators.required]]
      });
      this.addField();
      if(!  hs.isLogin){
        ms.error("请登录!");
        router.navigate(["/login"]);
      }
  }
  exchange():void{
    this.rev=!this.rev;
  }
  beforeUpload = (file: UploadFile): boolean => {
    this.fileList = this.fileList.concat(file);
    return false;
  };
  handleUpload(): void {
  }
  submitForm(){
    for (const i in this.fg.controls) {
      this.fg.controls[i].markAsDirty();
      this.fg.controls[i].updateValueAndValidity();
    }
    let ws:WordUnit[]=[];
    for (let index = 0; index < this.controlArray.length; index++) {
      const element = this.controlArray[index];
      element.tip=this.fg.get(element.idt.toString()).value;
      element.remember=this.fg.get(element.idr.toString()).value;
      if(this.rev){
        let x=element.remember;
        element.remember=element.tip;
        element.tip=x;
      }
      ws.push({
        tip:element.tip,
        remember:element.remember
      });
    }
    if(this.fileList.length<0){
      this.ms.error("请选择背景图");
      return;
    }
    const dn=this.fg.get("dname").value;
    if(dn==null||dn==''){
      return;
    }
    this.hs.insertDic(dn,this.fileList[0],ws).subscribe(r=>{
      if(r.infoType==RepCodes.Succeed){
        this.ms.success("上传数据成功");
      }
    });
  }
  addField(e?: MouseEvent): void {
    if (e) {
      e.preventDefault();
    }
    //const id = this.controlArray.length > 0 ? this.controlArray[this.controlArray.length - 1].id + 1 : 0;
    const newer={idt:this.indexer,idr:this.indexer+1,tip:null,remember:null};
    let locIndex=this.indexer;
    const index = this.controlArray.push(newer);
    this.fg.addControl(
      newer.idt.toString(),
      new FormControl(null, Validators.required)
    );
    this.fg.addControl(
      newer.idr.toString(),
      new FormControl(null, Validators.required)
    );
    this.indexer+=2;
  }

  removeField(i:{ idt: number,idr:number, tip:string,remember:string}, e: MouseEvent): void {
    e.preventDefault();
    if (this.controlArray.length > 1) {
      const index = this.controlArray.indexOf(i);
      this.controlArray.splice(index, 1);
      this.fg.removeControl(i.idt.toString());
      this.fg.removeControl(i.idr.toString());
    }
  }
  getFormControl(name: string): AbstractControl {
    return this.fg.controls[name];
  }
}
