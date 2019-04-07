using Drifting.Jwt.Models;
using Drifting.JWT.Helpers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Unit.Web.Controllers
{
    public abstract class ApiController:Controller
    {
        [NonAction]
        protected TokenDec GetToken(string key)
        {
            return AuthorizationHelper.ParseTokenByAuthorization(Request.Headers["Authorization"], key);
        }
        [NonAction]
        protected ulong GetUserId()
        {
            var idc = User.FindFirst("Id");
            if (idc == null)
            {
                return 0;
            }
            if (ulong.TryParse(idc.Value, out var id))
            {
                return id;
            }
            return 0;
        }
    }
}
