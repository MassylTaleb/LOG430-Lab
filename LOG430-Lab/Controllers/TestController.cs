using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace LOG430_Lab.Controllers
{
    public class TestController : Controller
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok();
        }


        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            return Ok();
        }

    }
}
