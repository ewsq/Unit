using Microsoft.AspNetCore.Mvc;
using Unit.Data.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.Extensions.Options;
using Drifting.Jwt.Models;
using Drifting.JWT.Helpers;

namespace Unit.Web.Controllers
{
    [EnableCors]
    [Route("api/v1/[controller]/[action]")]
    public class AccountController : ApiController
    {
        private readonly UserResponse _userResponse;
        private readonly IOptions<WebApiSettings> apiSettings;

        public AccountController(UserResponse userResponse, IOptions<WebApiSettings> apiSettings)
        {
            _userResponse = userResponse;
            this.apiSettings = apiSettings;
        }
        [HttpGet]
        public async Task<IActionResult> IsLogin(string token)
        {
            return Ok(await Task.FromResult(AuthorizationHelper.ParseToken(token, apiSettings.Value.SecretKey)));
        }
        [HttpPost]
        public async Task<IActionResult> Login([FromForm]string userName,string pwd)
        {
            return Ok(await _userResponse.LoginAsync(userName, pwd));
        }
        [HttpPost]
        public async Task<IActionResult> Register([FromForm]string userName, string pwd)
        {
            return Ok(await _userResponse.RegisterAsync(userName, pwd));
        }
        [HttpGet]
        public IActionResult GetHeadImg([FromQuery]string fn)
        {
            return PhysicalFile(_userResponse.GetHeadImgPath(fn), "image/png");
        }
        [HttpGet]
        public async Task<IActionResult> GetUserInfo()
        {
            var id = GetUserId();
            var res = await _userResponse.GetUserInfoAsync(id);
            return Ok(res);
        }
        [HttpPost]
        [Authorize]
        [Consumes("multipart/form-data", "application/octet-stream")]
        public async Task<IActionResult> UploadHeadImg(IFormFile hfn)
        {
            var id = GetUserId();
            return Ok(await _userResponse.UploadHeadImgAsync(id, hfn));
        }
    }
}
