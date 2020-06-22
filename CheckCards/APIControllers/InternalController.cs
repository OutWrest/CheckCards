using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CheckCards.APIControllers
{
    [Route("api/v0.999/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class InternalController : ControllerBase
    {
        public ActionResult Get()
        {
            string name = this.HttpContext.User.Identity.Name;
            return Ok();
        }
    }
}