using ApiTest.Authentication;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace ApiTest.Responses
{
    public class AuthenticationResponse
    {
        public string Status;
        public string Message;
    }

    public class LoginResponse
    {
        public string Token;
        public DateTime ExpirationDate;
        public string Id;
        public string Email;
        public string Username;
        public string[] Roles;
    }

    public class IsLoggedInResponse
    {
        public string Id;
        public string Email;
        public string Username;
        public string[] Roles;
    }

    public class GetUsersResponse
    {
        public IQueryable<ApplicationUser> Users;
    }
}
