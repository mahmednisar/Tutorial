using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tutorial.Dto;
using Tutorial.Services.Infrastructure;
using Tutorial.Utils;

namespace Tutorial.API.Controllers
{

    public class AuthController : BaseController
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        
        [HttpPost]
        public TResponse LoginUser(LoginDto loginDto)
        {
            return _authService.Login(loginDto);
        }
    }
}
