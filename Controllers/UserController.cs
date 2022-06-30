using ApiTest.Context;
using ApiTest.Models;
using ApiTest.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ApiTest.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private UserContext _userContext;

        public UserController(UserContext userContext)
        {
            _userContext = userContext;
        }

        [HttpGet]
        [Route("getUsers")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserlistResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(UserTextResponse))]
        public IActionResult GetUsers()
        {
            DbSet<User> userList = _userContext.Users;
            if(userList == null)
            {
                UserTextResponse res = new UserTextResponse
                {
                    output = "Could not retrieve users list"
                };

                return NotFound(res);
            }

            UserlistResponse response = new UserlistResponse
            {
                userList = userList,
            };
            return Ok(response);
        }

        [HttpGet("{id}/getuser")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(UserTextResponse))]
        public IActionResult GetUser(int id)
        {
            User u = _userContext.Users.FirstOrDefault(u => u.Id == id);
            if(u == null)
            {
                UserTextResponse res = new UserTextResponse
                {
                    output = "User with id: " + id + " could not be found"
                };

                return NotFound(res);
            }

            UserResponse response = new UserResponse
            {
                user = u,
            };
            return Ok(response);
        }

        [HttpGet("{id}/{password}/authenticate")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserAuthenticateResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(UserTextResponse))]
        public IActionResult AuthenticateUser(int id, string password)
        {
            User u = _userContext.Users.FirstOrDefault(u => u.Id == id);
            if (u == null)
            {
                UserTextResponse res = new UserTextResponse
                {
                    output = "User with id: " + id + " could not be found"
                };

                return NotFound(res);
            }

            bool verified = BCrypt.Net.BCrypt.Verify(password, u.Password);

            UserAuthenticateResponse response = new UserAuthenticateResponse
            {
                succes = verified,
            };
            return Ok(response);
        }

        // POST api/<EmployeeController>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserTextResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(UserTextResponse))]
        public IActionResult Post([FromBody] User value)
        {
            if(hasUsername(value.Username))
            {
                UserTextResponse res = new UserTextResponse
                {
                    output = "Could not add user, username allready exists"
                };

                return NotFound(res);
            }
            value.Password = BCrypt.Net.BCrypt.HashPassword(value.Password);
            _userContext.Users.Add(value);

            _userContext.SaveChanges();
            string output = "User: " + value.Username + " succesfully added to the database";
            UserTextResponse response = new UserTextResponse
            {
                output = output,
            };
            return Ok(response);
        }

        private bool hasUsername(string username)
        {
            return _userContext.Users.Any(u => u.Username == username);
        }

        // PUT api/<EmployeeController>/5
        [HttpPut("{id}/put")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserTextResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(UserTextResponse))]
        public IActionResult Put(int id, [FromBody] User value)
        {
            var user = _userContext.Users.FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                if(value.Password != user.Password)
                {
                    value.Password = BCrypt.Net.BCrypt.HashPassword(value.Password);
                }

                _userContext.Entry<User>(user).CurrentValues.SetValues(value);
                _userContext.SaveChanges();

                UserTextResponse response = new UserTextResponse
                {
                    output = "Succesfully updated values for user with id: " + id,
                };

                return Ok(response);
            }

            UserTextResponse res = new UserTextResponse
            {
                output = "Could not find user with id: " + id,
            };

            return NotFound(res);
        }

        // DELETE api/<EmployeeController>/5
        [Authorize]
        [HttpDelete("{id}/delete")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserTextResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(UserTextResponse))]
        public IActionResult Delete(int id)
        {
            var user = _userContext.Users.FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                _userContext.Users.Remove(user);
                _userContext.SaveChanges();

                UserTextResponse response = new UserTextResponse
                {
                    output = "Succesfully deleted user with id: " + id,
                };

                return Ok(response);
            }

            UserTextResponse res = new UserTextResponse
            {
                output = "Could not find user with id: " + id,
            };

            return NotFound(res);
        }
    }
}
