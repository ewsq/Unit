import { Component, OnInit, Inject } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { DicPackage, RepCodes, WorkBeginning, WordUnit } from '../httpservice/httpservice.model';
import { HTTP_SERVICE_TOKEN, HttpService } from '../httpservice/httpservice.provider';
import { NzMessageService } from 'ng-zorro-antd';
import { AccountService } from '../account/account.service';

@Component({
  selector: 'app-study',
  templateUrl: './study.component.html',
  styleUrls: ['./study.component.css']
})
export class StudyComponent implements OnInit {
  did:string;
  dpck:WorkBeginning;
  locword:WordUnit;
  words:WordUnit[]=[];
  appearDesc:boolean=false;
  isforget:boolean=false;
  complated:boolean=false;
  imgp:string;
  wordRes:{word:WordUnit,forgetCount:number,offset:number}[]=[];
  constructor(private router:Router,
    private ac:ActivatedRoute,
    @Inject(HTTP_SERVICE_TOKEN) private hs:HttpService,
    private ms:NzMessageService,
    private acc:AccountService) { 
      ac.params.subscribe(p=>{
        this.did=p['did'];
      })
      if(!this.acc.token){
        this.ms.warning("当前你没有登录，数据不会被保存!");
      }
      hs.get10DicWords(this.did).subscribe(r=>{
        if(r.infoType==RepCodes.Succeed){
          this.dpck=r.entity;
          this.imgp=this.hs.getBckFile(this.dpck.dicUnit.imgPath);
          if(this.dpck.dicUnit.words.length==0){
            this.ms.info("你已背完此单元");
            setTimeout(()=>history.back(),1000);
          }else{
            this.words=this.dpck.dicUnit.words.slice();
            let index=0;
            this.dpck.dicUnit.words.forEach(ele => {
              this.wordRes.push({
                word:ele,
                forgetCount:0,
                  offset:index+this.dpck.from
              });
              index++;
            });
            this.locword=this.words[0];
          }
          console.log(this.dpck);
        }else{
          this.ms.error("获取单词失败，错误信息:"+r.msg);
        }
      })
    }
  nextPart(){
    location.reload();
  }
  ngOnInit() {
  }
  remember(){
    let word=this.wordRes.find(w=>w.word==this.locword);
    this.appearDesc=true;
  }
  forget(){
    let word=this.wordRes.find(w=>w.word==this.locword);
    word.forgetCount++;
    this.appearDesc=true;
    this.isforget=true;
  }
  next(){
    if(this.isforget){
      this.randArray(this.words);
    }else{
      this.words=this.words.slice(1);
    }
    this.appearDesc=false;
    if(this.words.length==0){
      //完成
      this.wordRes=this.wordRes.sort((r,w)=>w.forgetCount-r.forgetCount);
      let datas:{offset:number,forgetCount:number}[]=[];
      this.wordRes.forEach(ele => {
        datas.push({
          offset:ele.offset,
          forgetCount:ele.forgetCount
        });
      });
      if(!this.acc.token){
        this.ms.error("保存数据失败");
        return;
      }
      this.hs.complate(this.dpck.from,this.did,JSON.stringify(datas)).subscribe(r=>{
        this.complated=true;
        if(r.infoType==RepCodes.Succeed){
          
            this.ms.success("保存数据成功");
          
        }
      });
      return;
    }
    this.locword=this.words[0];
    this.isforget=false;
  }
  randArray<T>(data:Array<T>):void{
      for (var i = data.length-1; i >=0; i--) {
        var randomIndex = Math.floor(Math.random()*(i+1));
        var itemAtIndex = data[randomIndex];
        data[randomIndex] = data[i]; data[i] = itemAtIndex;
      } 
  }
}
