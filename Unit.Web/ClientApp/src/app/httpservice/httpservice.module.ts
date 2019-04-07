import { NgModule } from '@angular/core';
import { ModuleWithProviders } from '@angular/compiler/src/core';
import { HTTP_SERVICE_TOKEN, HttpService } from './httpservice.provider';


@NgModule({
    providers:[
        {provide:HTTP_SERVICE_TOKEN,useClass:HttpService}
    ]
})
export class HttpServiceModule {
}