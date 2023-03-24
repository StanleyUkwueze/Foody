﻿using Foody.DTOs;
using Foody.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Foody.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("sign-up")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            var result = await _authService.Register(model, Request.Scheme, Url);
            return Ok(result);
        }
    }
}