
export enum RepCodes
{
    /// <summary>
    /// 成功
    /// </summary>
    Succeed=0,
    /// <summary>
    /// 存在异常
    /// </summary>
    Exception,
    /// <summary>
    /// 失败了，但不知道发生了什么错误
    /// </summary>
    FailUnknow,
    ParamtersEmpty,
    NoSuchProject,
    UpdateProjectFail,
    AlreadyRequiredProject,
    NoSuchRequired,
    NoSuchProjectMember,
    NoSuchProjectPart,
    NoSuchProjectPlan,
    RegisterError,
    LoginError,
    NoSuchDialog,
    NoSuchFile,
    NoSuchRequiredJoin,
    NotCreator
}

export interface Rep{
    msg:string;
    isOk:boolean;
    infoType:RepCodes;
}
export interface EntityRep<TEntity> extends Rep{
    entity:TEntity;
}
export interface EnumerableRep<TEntity> extends Rep{
    entities:TEntity[];
}
export interface TokenEntity{
    accessToken:string;
    expiresIn:number;
}
export interface WordUnit{
    tip:string;
    remember:string;
}
export interface DicUnit{
    id:string;
    title:string;
    imgPath:string;
    words:Array<WordUnit>;
}
export interface ComplateUnit{
    offset:number;
    forgetCount:number;
}
export interface ComplateUnitRep{
    dicId:string;
    from:number;
    to:number;
    jsonResult:Array<ComplateUnit>;
}
export interface WorkBeginning{
    from:number;
    dicUnit:DicUnit;
}
export interface DicPackage{
    dic:DicUnit;
    larstIndex:number;
    totalDicCount:number;
}
export interface UserInfo{
    id:number;
    name:string;
    himg:string;
}