using ErrorHandling_AspNetCore.Dtos;
using ErrorHandling_AspNetCore.Exceptions;
using ErrorHandling_AspNetCore.Interfaces;
using ErrorHandling_AspNetCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ErrorHandling_AspNetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriverController : ControllerBase
    {
        private readonly IDriverService _userService;

        public DriverController(IDriverService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(Driver driver)
        {
            var result =await _userService.Register(driver);
            //if (!result.IsSuccess)
            //{
            //    //return ValidationProblem(detail: result.Errors[0],instance:HttpContext.Request.Path,statusCode:400,title:"User already exists", type: "blank:html", modelStateDictionary:ModelState);
            //}
            return Ok(result);
        }
        //[Authorize]
        [HttpGet("get-driver")]
        public async Task<IActionResult> GetAsync([Required] int id)
        {
            var result = await _userService.GetDriver(id);
            if (result == null)
            {
                throw new NotFoundException("Driver",id);
            }
            return Ok(result);
        }
        [HttpGet("get-drivers")]
        public async Task<IActionResult> UpdateDriver()
        {
            var result = await _userService.GetDrivers();
            return Ok(result);
        }
        [HttpPut("update-driver")]
        public async Task<IActionResult> UpdateDriver(Driver driver)
        {
            var result = await _userService.Update(driver);
            return Ok(result);
        }
        [HttpDelete("delete-driver")]
        public async Task<IActionResult> DeleteDriver(int id)
        {
            var result = await _userService.DeleteDriver(id);
            return Ok(result);
        }
    }
}
