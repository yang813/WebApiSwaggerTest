using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebApiSwaggerTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        /// <summary>
        ///  通过id获取值
        /// </summary>
        /// <remarks>
        /// 例子：
        /// Get api/values/5
        /// </remarks>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="201">返回value字符串</response>
        /// <response code="400">如果id为空</response>  
        // GET api/values/5
        [HttpGet("{id}")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public ActionResult<string> Get(int id)
        {
            return $"你请求的 id 是 {id}";
        }

        /// <remarks>
        /// Sample request:
        ///
        /// POST /Todo
        /// {
        /// "id": 1,
        /// "name": "Item1",
        /// "isComplete": true
        /// }
        ///
        /// </remarks>
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
