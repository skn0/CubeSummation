using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace CubeSummation.web.Controllers
{
    [Route("api/[controller]")]
    public class CubeController : Controller
    {
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            Models.Cube objMatrix = new Models.Cube(4, 4, 4);


            objMatrix.UpdateInCube(2, 2, 2, 4);
            var sum1 = objMatrix.QueryInCube(1, 1, 1, 3, 3, 3);
            objMatrix.UpdateInCube(1, 1, 1, 23);
            var sum2 = objMatrix.QueryInCube(2, 2, 2, 4, 4, 4);
            var sum3 = objMatrix.QueryInCube(1, 1, 1, 3, 3, 3);

            var total = sum1 + sum2 + sum3;
            //int sum = objMatrix.QueryInMatrix3D()
                        
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
