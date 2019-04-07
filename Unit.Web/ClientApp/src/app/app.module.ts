import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppComponent } from './app.component';
import { NgZorroAntdModule, NZ_I18N, en_US } from 'ng-zorro-antd';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { registerLocaleData } from '@angular/common';
import en from '@angular/common/locales/en';
import { HttpServiceModule } from './httpservice/httpservice.module';
import { RouterModule } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { DicComponent } from './dic/dic.component';
import { AccountModule } from './account/account.module';
import { CunitComponent } from './cunit/cunit.component';
import { StudyComponent } from './study/study.component';

registerLocaleData(en);

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    LoginComponent,
    RegisterComponent,
    DicComponent,
    CunitComponent,
    StudyComponent
  ],
  imports: [
    BrowserModule,
    NgZorroAntdModule,
    NgbModule,
    FormsModule,
    HttpClientModule,
    ReactiveFormsModule,
    BrowserAnimationsModule,
    HttpServiceModule,
    AccountModule.forRoot(),
    RouterModule.forRoot([
      {path:'',component:HomeComponent,pathMatch:'full'},
      {path:'login',component:LoginComponent},
      {path:'register',component:RegisterComponent},
      {path:'createUnit',component:CunitComponent},
      {path:'study',component:StudyComponent,children:[
        {
          path:':did',
          component:StudyComponent
        }
      ]},
      {path:'dic',component:DicComponent,children:[
        {
          path:':id',
          component:DicComponent
        }
      ]}   
    ])
  ],
  providers: [{ provide: NZ_I18N, useValue: en_US }],
  bootstrap: [AppComponent]
})
export class AppModule { }
