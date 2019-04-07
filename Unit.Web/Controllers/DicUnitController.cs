using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unit.Data.Responses;

namespace Unit.Web.Controllers
{
    [EnableCors]
    [Authorize]
    [Route("api/v1/[controller]/[action]")]
    public class DicUnitController : ApiController
    {
        private readonly DicUnitResponse dicUnitResponse;
        private readonly WordsBrench wordsBrench;
        public DicUnitController(DicUnitResponse dicUnitResponse,WordsBrench wordsBrench)
        {
            this.dicUnitResponse = dicUnitResponse;
            this.wordsBrench = wordsBrench;
        }

        [HttpPost]
        [Consumes("multipart/form-data", "application/octet-stream")]
        public async Task<IActionResult> InsertDic(string title, IFormFile bckFile, string words)
        {
            return Ok(await dicUnitResponse.InsertDicAsync(title, bckFile, words));
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetDicPck([FromQuery]string oid)
        {
            ulong? id = GetUserId();
            if (id == 0)
            {
                id = null;
            }
            return Ok(await dicUnitResponse.GetDicAsync(oid, id));
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetRandomDicPackage()
        {
            ulong? id = GetUserId();
            if (id==0)
            {
                id = null;
            }
            return Ok(await dicUnitResponse.GetRandomDicPackageAsync(id));
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> FindWord([FromQuery]string key)
        {
            return Ok(await wordsBrench.GetAsync(key)); 

        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetDics()
        {
            return Ok(await dicUnitResponse.GetDicsAsync());
        }
        [HttpGet]
        public async Task<IActionResult> Get10DicWords([FromQuery]string oid)
        {
            var id = GetUserId();
            return Ok(await dicUnitResponse.Get10DicWordsAsync(id, oid));
        }
        [HttpPost]
        public async Task<IActionResult> Complate(int from, string oid, string info)
        {
            var id = GetUserId();
            return Ok(await dicUnitResponse.ComplateAsync(id,from,oid,info));
        }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetBckFile([FromQuery]string fn)
        {
            return PhysicalFile(dicUnitResponse.GetBckFile(fn), "image/png");
        }
    }
}
