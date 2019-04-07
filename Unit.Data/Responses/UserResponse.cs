using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Drifting.JWT.Provider;
using Extensions.Reps;
using Extensions.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Unit.Data;
using Unit.Data.Countermeasure;
using Unit.DbModel;
using Unti.Storage;
using Z.EntityFramework.Plus;

namespace Unit.Data.Responses
{
    /// <summary>
    /// 对用户的其它属性的响应
    /// </summary>
    [WithNameSpace("Salient.Web.Controllers")]
    [WithClassRoute("/api/v1/[controller]")]
    [ControllerName("Account")]
    public partial class UserResponse : DataResponseBase
    {
        private readonly UserManager<SUser> _userManager;
        private readonly SignInManager<SUser> _signInManager;
        private readonly RoleManager<SRole> _roleManager;
        private readonly STokenProvider _tokenProvider;
        private readonly Globle _globle;
        private readonly HeadPortraitFolder _headPortraitFolder;
        public UserResponse(WDbContext dbContext,
            UserManager<SUser> userManager,
            SignInManager<SUser> signInManager,
            RoleManager<SRole> roleManager,
            STokenProvider tokenProvider,
            Globle globle)
            : base(dbContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _tokenProvider = tokenProvider;
            _globle = globle;
            _headPortraitFolder = _globle.GetGlobleFolder<HeadPortraitFolder>();
        }
        [HttpPostMethod]
        public async Task<EntityRep<TokenEntity, RepCodes>> LoginAsync(string name, string pwd)
        {
            EntityRep<TokenEntity, RepCodes> res = null;
            if (res == null)
            {
                var lres = await _signInManager.PasswordSignInAsync(name, pwd, false, true);
                if (lres.Succeeded)
                {
                    var user = await _signInManager.UserManager.FindByNameAsync(name);
                    var rcs = new Claim[] 
                    {
                        new Claim(SalientClaimTypes.Id, user.Id.ToString())
                    };
                    var te = _tokenProvider.GenerateToken(rcs);
                    res = new EntityRep<TokenEntity, RepCodes>()
                    {
                        Entity = te
                    };
                    
                }
                else
                {
                    res = new EntityRep<TokenEntity, RepCodes>()
                    {
                        InfoType = RepCodes.LoginError
                    };
                }

            }
            return res;
        }
        [HttpPostMethod]
        public async Task<EntityRep<bool, RepCodes>> RegisterAsync(string name, string pwd)
        {

            EntityRep<bool, RepCodes> res = null;
            try
            {
                var user = new SUser()
                {
                    UserName = name
                };
                var rres = await _userManager.CreateAsync(user, pwd);
                res = FromEntity(rres.Succeeded ? RepCodes.Succeed : RepCodes.RegisterError, rres.Succeeded);
            }
            catch (Exception ex)
            {
                res = FromEntity(RepCodes.Exception, false);
                res.Msg = ex.Message;
            }
            return res;
        }
        public async Task<Rep<RepCodes>> UploadHeadImgAsync(ulong uid,IFormFile himg)
        {
            try
            {
                var fex = himg.FileName.Split('.').Last();
                var fn = Guid.NewGuid().ToString() + "." + fex;
                var fp = Path.Combine(_headPortraitFolder.Directory.FullName, fn);
                using (var fs = File.Create(fp))
                {
                    await himg.CopyToAsync(fs);
                }
                var r = await DbContext.Users.AsNoTracking().UpdateAsync(u => new SUser { HeadImg = fn });
                return FromCode(r > 0 ? RepCodes.Succeed : RepCodes.FailUnknow);
            }
            catch (Exception ex)
            {
                return FromCode(RepCodes.Exception,ex.Message);
            }
        }
        public string GetHeadImgPath(string fn)
        {
            return Path.Combine(_headPortraitFolder.Directory.FullName, fn);
        }
        public async Task<EntityRep<UserInfo,RepCodes>> GetUserInfoAsync(ulong uid)
        {
            var info = await DbContext.Users.AsNoTracking().Where(u => u.Id == uid)
                .Select(u => new UserInfo { Id=u.Id,Name=u.UserName,Himg=u.HeadImg}).FirstOrDefaultAsync();
            return FromEntity( RepCodes.Succeed,info);
        }
    }
    public class UserInfo
    {
        public ulong Id { get; set; }
        public string Name { get; set; }
        public string Himg { get; set; }
    }
}
