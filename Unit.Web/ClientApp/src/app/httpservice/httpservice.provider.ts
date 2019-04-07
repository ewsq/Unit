import { Injectable, InjectionToken } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { RepCodes, EntityRep, TokenEntity, Rep, WordUnit, DicPackage, DicUnit, WorkBeginning, UserInfo } from './httpservice.model';
import { AccountService } from '../account/account.service';

export const HTTP_SERVICE_TOKEN=new InjectionToken<HttpService>('the http service');
class HttpServiceBase{
    public static readonly tokenKey:string='gdToken';
    public static readonly baseUrl:string='http://47.102.142.251/api/v1/';
    public get token() : string {
        return sessionStorage.getItem(HttpServiceBase.tokenKey);
    }
    public logout():void{
        sessionStorage.setItem(HttpServiceBase.tokenKey,null);
    }
    constructor(private http:HttpClient,private ac:AccountService){
       if(this.ac){
        this.ac.token=this.token;
       }
    }
    public get isLogin() : boolean {
        let t= this.token;
        return t!==undefined&&t!=null;
    }
    protected get authHead() : string {
        return "Bearer "+this.token;
    }
    protected post<T>(urlPaths:string[],d:{k:string,v:any}[],headers:HttpHeaders=null):Observable<T>{
        let fd=new FormData();
        d.forEach(ele => {
            fd.append(ele.k,ele.v);
        });
        return this.http.post<T>(this.generalUrl(HttpServiceBase.baseUrl,urlPaths),fd,{
            withCredentials:true,
            headers:headers
        });
    }
    protected get<T>(urlPaths:string[],d:{k:string,v:any}[]=null,headers:HttpHeaders=null):Observable<T>{
        let par='';
        if(d!=null&&d.length>0){
            par='?'+d[0].k+"="+d[0].v;
            for (let index = 1; index < d.length; index++) {
                const element = d[index];
                par+=`&${element.k}=${element.v}`;
            }
        }
        return this.http.get<T>(this.generalUrl(HttpServiceBase.baseUrl,urlPaths)+par,{
            withCredentials:true,
            headers:headers
        });
    }
    
    protected generalUrl(base:string,strs:string[]):string{
        return base+strs.join("/");
    }
}
@Injectable()
export class HttpService extends HttpServiceBase{
    readonly dicPath='dicUnit';
    readonly accountPath='account';
    constructor(http:HttpClient,ac:AccountService){
        super(http,ac);
    }
    public async login(name:string,pwd:string):Promise<EntityRep<TokenEntity>> {
        let res=this.post<EntityRep<TokenEntity>>([this.accountPath,"login"],[
            {k:"userName",v:name},
            {k:"pwd",v:pwd}
        ]);
        let r=await res.toPromise();
        if(r.infoType===RepCodes.Succeed&&r.isOk){
            sessionStorage.setItem(HttpServiceBase.tokenKey,r.entity.accessToken);
        }
        return r;
    }
    public register(userName:string,pwd:string):Observable<Rep>{
        return this.post([this.accountPath,"register"],[
            {k:"userName",v:userName},
            {k:"pwd",v:pwd}
        ]);
    }
    public getHeadImg(fn:string):string{
        return `/api/v1/${this.accountPath}/GetHeadImg?fn=${fn}`;
    }
    public uploadHeadImg(hfn:any):Observable<Rep>{
        /// TODO:上传文件
        return this.post([this.accountPath,"UploadHeadImg"],[
            {k:"hfn",v:hfn}
        ],new HttpHeaders({"Authorization":this.authHead}));
    }
    public insertDic(title:string,bckFile:any,words:Array<WordUnit>):Observable<Rep>{
        /// TODO:上传文件
        return this.post([this.dicPath,"InsertDic"],[
            {k:"title",v:title},
            {k:"bckFile",v:bckFile},
            {k:"words",v:JSON.stringify(words)}
        ],new HttpHeaders({
            "Authorization":this.authHead
        }));
    }
    public getRandomDicPackage():Observable<EntityRep<DicPackage>>{
        /// TODO:上传文件
        return this.get([this.dicPath,"GetRandomDicPackage"],[],new HttpHeaders({"Authorization":this.authHead}));
    }
    public getDics():Observable<Array<DicUnit>>{
        /// TODO:上传文件
        return this.get([this.dicPath,"GetDics"],[]);
    }
    public findWord(key:string):Observable<Array<{item1:string,item2:string}>>{
        /// TODO:上传文件
        return this.get([this.dicPath,"FindWord"],[
            {k:"key",v:key}
        ]);
    }
    public getDic(oid:string):Observable<DicPackage>{
        return this.get([this.dicPath,"GetDicPck"],[
            {k:"oid",v:oid}
        ],new HttpHeaders({"Authorization":this.authHead}));
    }
    public get10DicWords(oid:string):Observable<EntityRep<WorkBeginning>>{
        /// TODO:上传文件
        return this.get([this.dicPath,"Get10DicWords"],[
            {k:"oid",v:oid}
        ],new HttpHeaders({"Authorization":this.authHead}));
    }
    public complate(from:number,oid:string,info:string):Observable<Rep>{
        /// TODO:上传文件
        return this.post([this.dicPath,"Complate"],[
            {k:"from",v:from},
            {k:"oid",v:oid},
            {k:"info",v:info},
        ],new HttpHeaders({"Authorization":this.authHead}));
    }
    public getBckFile(fn:string):string{
        return `/api/v1/${this.dicPath}/GetBckFile?fn=${fn}`;
    }
    public getUserInfo():Observable<EntityRep<UserInfo>>{
        return this.get([this.accountPath,"GetUserInfo"],[
        ],new HttpHeaders({"Authorization":this.authHead}));
    }
}
