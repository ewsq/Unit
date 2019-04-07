import { Component, OnInit, Inject } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { HTTP_SERVICE_TOKEN, HttpService } from '../httpservice/httpservice.provider';
import { DicUnit, DicPackage } from '../httpservice/httpservice.model';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { AccountService } from '../account/account.service';

@Component({
  selector: 'app-dic',
  templateUrl: './dic.component.html',
  styleUrls: ['./dic.component.css']
})
export class DicComponent implements OnInit {
  did:string;
  dic:DicPackage;
  complatePex:number;
  bckImg:string;
  appear:boolean=true;
  constructor(
    @Inject(HTTP_SERVICE_TOKEN) private hs:HttpService,
    private router:Router,
    private ar:ActivatedRoute,
    private ac:AccountService) {
      if(!this.hs.isLogin){
        location.href='/login';
        return;
      }
      ar.params.subscribe(r=>{
        this.did=r["id"];
        console.log(this.did);
        if(this.did){
          hs.getDic(this.did).subscribe(o=>{
            console.log(o);
            if(o){
              this.dic=o;
              this.bckImg=hs.getBckFile(this.dic.dic.imgPath);
              if(this.dic.larstIndex==-1&&!this.ac.token){
                this.appear=false;
              }else{
                if(this.dic.larstIndex>=this.dic.totalDicCount){
                  this.complatePex=100;
                }else{
                  if(this.dic.larstIndex==-1){
                    this.dic.larstIndex=0;
                  }
                  this.complatePex=this.dic.larstIndex/this.dic.totalDicCount;
                  this.complatePex=Math.abs(this.complatePex);
                  this.complatePex=Number.parseFloat((this.complatePex*100).toFixed(2));
                }
              }
              
            }
          });
        }
      });
   }

  ngOnInit() {
  }
  goStudy(){
    this.router.navigate(['/study',{did:this.did}]);
  }
}
