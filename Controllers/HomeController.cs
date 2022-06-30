using APItest.Models;
using APItest.Responses.HomeResponses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace APItest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {

        public HomeController()
        {


        }


        [HttpGet("first_func")]
        [ProducesResponseType(typeof(TestResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<TestResponse> Get1()
        {
            var rng = new Random();
            return new TestResponse
            {
                output = new TestObject
                {
                    name = "Testoutput",
                    id = rng.Next()
                }
            };
        }

        [HttpGet("{input}/second_func")]
        [ProducesResponseType(typeof(TestResponse2), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<TestResponse2> test2(string input)
        {
            var rng = new Random();
            Debug.WriteLine("TEST!");
            return new TestResponse2
            {
                testString = input,
                testId = 123
            };
        }
    }
}
