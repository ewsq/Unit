import { Component, OnInit, Inject } from '@angular/core';
import { md5 } from '../httpservice/http.baidu'
import { HttpService, HTTP_SERVICE_TOKEN } from '../httpservice/httpservice.provider';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
  styles:[
    `
    nz-avatar {
      margin-top: 16px;
      margin-right: 16px;
    }
    `
  ]
})
export class HomeComponent implements OnInit {
  inputValue:string;
  options:Array<{item1:string,item2:string}>=[];
  constructor(@Inject(HTTP_SERVICE_TOKEN) private hs:HttpService) {

   }

  ngOnInit() {
  }
  onInput(){
    this.hs.findWord(this.inputValue).subscribe(r=>{
      this.options=r;
    })
  }
}
