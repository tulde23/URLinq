using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UnitTests.Models;

namespace UnitTests.Controllers
{
    [Route("/api/v1/[controller]")]
    public class UnitTestController : ControllerBase
    {
        [HttpGet]
        public Task<string> GetString([FromQuery] string id)
        {
            return Task.FromResult(Guid.NewGuid().ToString());
        }
        [HttpGet("SubRoute")]
        public Task<string> GetStringSubRoute([FromQuery] string id)
        {
            return Task.FromResult(Guid.NewGuid().ToString());
        }
        [HttpGet("{myId}")]
        public Task<string> GetStringWithRouteParameter([FromRoute] string myId)
        {
            return Task.FromResult(Guid.NewGuid().ToString());
        }
        [HttpPost]
        public Task<string> PostComplexModel([FromBody]ComplexModel complexModel)
        {
            return Task.FromResult(Guid.NewGuid().ToString());
        }
        [HttpPost("{myId}")]
        public Task<string> PostComplexModelWithId([FromBody]ComplexModel complexModel)
        {
            return Task.FromResult(Guid.NewGuid().ToString());
        }
    }
}
