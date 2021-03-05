using System;
using JinRuiHomeFurnishing.Model;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using JinRuiHomeFurnishing.Bll;
using System.Data;
using JinRuiHomeFurnishing.dto;
using JinRuiHomeFurnishingNetCoreMVC.Attributes;

namespace JinRuiHomeFurnishingNetCoreMVC.Controllers
{
    /// <summary>
    /// valuesMethod
    /// </summary>
    /// <response code="102">请您重新登陆</response>
    /// <response code="101">失败</response>
    /// <response code="100">成功</response>  
    [Route("product/[controller]/[action]")]
    [EnableCors("AllowAll")]
    [ApiController]
    [ProducesResponseType(102)]
    [ProducesResponseType(101)]
    [ProducesResponseType(100)]
    public class ValuesController : ControllerBase
    {
        /// <summary>
        /// 导管子
        /// </summary>
        /// <remarks>
        /// 备注:
        /// http://localhost:50810/product/Values/Get
        /// </remarks>
        /// <param name="a">参数1</param>
        /// <param name="b">参数2</param>
        /// <returns></returns>
        [HttpGet]
        [LoginCheck]
        public ActionResult Get(int a, int b)
        {
            ApiResult<object> result = new ApiResult<object>();

            UsersBll u = new UsersBll();
            DataSet ds = u.getUsersList();

            result.code = 1;
            result.message = "成功";
            result.data = ds;

            return new JsonResult(result);
        }

        // GET api/values/5
        [HttpGet("{id}&{name}")]
        [LoginCheck]
        public ActionResult Get(int id, string name)
        {
            return new JsonResult(1);
        }

        // GET api/values/5
        [HttpGet]
        public ActionResult GetUserInfo([FromBody]UsersDto u)  //传json
        {
            return new JsonResult(u);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

    }
}
