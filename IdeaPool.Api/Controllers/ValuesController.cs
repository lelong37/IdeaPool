#region

using System.Collections.Generic;
using IdeaPool.Domain.Models;
using Microsoft.AspNetCore.Mvc;

#endregion

namespace IdeaPool.Api.Controllers
{
    public class ValuesController: BaseController
    {
        public ValuesController(IdeaPoolContext dataContext): base(dataContext) {}

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id) {}

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get() => new[] { "value1", "value2" };

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id) => "value";

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value) {}

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value) {}
    }
}