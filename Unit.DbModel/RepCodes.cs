using System;
using System.Collections.Generic;
using System.Text;

namespace Unit.DbModel
{
    /// <summary>
    /// 响应代码
    /// </summary>
    public enum RepCodes
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
}
