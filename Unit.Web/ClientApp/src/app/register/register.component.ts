import { Component, OnInit, Inject } from '@angular/core';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { HttpService, HTTP_SERVICE_TOKEN } from '../httpservice/httpservice.provider';
import { Router, ActivatedRoute } from '@angular/router';
import { RepCodes } from '../httpservice/httpservice.model';
import { AccountService } from '../account/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
  styles:[
    `
    `
  ]
})
export class RegisterComponent implements OnInit {

  validateForm: FormGroup;
  registering:boolean=false;
  submitForm() {
    this.registering=true;
    for (const i in this.validateForm.controls) {
      this.validateForm.controls[i].markAsDirty();
      this.validateForm.controls[i].updateValueAndValidity();
    }
    var un=this.validateForm.get("userName").value;
    var pwd=this.validateForm.get("password").value;
    console.log(pwd);
    this.hs.register(un,pwd).subscribe(r=>{
      if(r.infoType==RepCodes.Succeed){
        this.router.navigate(["/login"]);
      }
      this.registering=true;
    });
  }

  constructor(private fb: FormBuilder,
    @Inject(HTTP_SERVICE_TOKEN) private hs:HttpService,
    private router:Router) {
      
    }

  ngOnInit(): void {
    this.validateForm = this.fb.group({
      userName: [null, [Validators.required]],
      password: [null, [Validators.required]]
    });
  }
}
