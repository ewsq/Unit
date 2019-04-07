import { NgModule } from '@angular/core';
import { ModuleWithProviders } from '@angular/compiler/src/core';
import { AccountService } from './account.service';



@NgModule()
export class AccountModule{
    static forRoot():ModuleWithProviders{
        return {
            ngModule:AccountModule,
            providers:[
                AccountService
            ]
        }
    }
}