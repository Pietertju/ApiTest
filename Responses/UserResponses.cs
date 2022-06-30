using ApiTest.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiTest.Responses
{
    public class UserResponses
    {
    }

    public class UserTextResponse
    {
        public string output;
    }

    public class UserResponse
    {
        public User user;
    }

    public class UserlistResponse
    {
        public DbSet<User> userList;
    }

    public class UserAuthenticateResponse
    {
        public bool succes;
    }

}
