import { Component, OnInit, Inject } from '@angular/core';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { HttpService, HTTP_SERVICE_TOKEN } from '../httpservice/httpservice.provider';
import { inject } from '@angular/core/testing';
import { AccountService } from '../account/account.service';
import { RepCodes } from '../httpservice/httpservice.model';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
  styles:[
    `.login-form {
      max-width: 500px;
    }
    `
  ]
})
export class LoginComponent implements OnInit {
  validateForm: FormGroup;
  loginning:boolean=false;
  async submitForm() {
    this.loginning=true;
    for (const i in this.validateForm.controls) {
      this.validateForm.controls[i].markAsDirty();
      this.validateForm.controls[i].updateValueAndValidity();
    }
    var un=this.validateForm.get("userName").value;
    var pwd=this.validateForm.get("password").value;
    let r=await this.hs.login(un,pwd);
    if(r.infoType==RepCodes.Succeed){
      this.ac.token=r.entity.accessToken;
      this.hs.getUserInfo().subscribe(u=>{
        if(u.infoType==RepCodes.Succeed){
          if(u.entity.himg!=null){
            this.ac.headImgPath=this.hs.getHeadImg(u.entity.himg);
          }
          this.ac.name=u.entity.name;
          this.ac.id=u.entity.id;
          this.router.navigate(['/']);
        }
        this.loginning=false;
      });
    }
    this.loginning=false;
  }

  constructor(private fb: FormBuilder,
    @Inject(HTTP_SERVICE_TOKEN) private hs:HttpService,
    private ac:AccountService,
    private router:Router) {}

  ngOnInit(): void {
    this.validateForm = this.fb.group({
      userName: [null, [Validators.required]],
      password: [null, [Validators.required]]
    });
  }
}
