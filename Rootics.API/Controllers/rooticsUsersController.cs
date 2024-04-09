using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rootics.Core.Dtos;
using Rootics.Core.InterFaces;
using Rootics.Core.Models;
using Rootics.EF.Repsotories;
using System.Text.RegularExpressions;

namespace Rootics.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class rooticsUsersController : ControllerBase
    {
        private readonly IAuthUser _authUser;
        private readonly IUnitOfWork _unitOfWork;

        public rooticsUsersController(IAuthUser authUser, IUnitOfWork unitOfWork)
        {
            _authUser = authUser;
            _unitOfWork = unitOfWork;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(RegisterModelDto registerModelDto)
        {
            // Check if the model state is valid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Perform specific validation rules
            if (string.IsNullOrEmpty(registerModelDto.UserName))
            {
                ModelState.AddModelError("Username", "Username is required.");
            }

            if (string.IsNullOrEmpty(registerModelDto.Password))
            {
                ModelState.AddModelError("Password", "Password is required.");
            }
            else if (registerModelDto.Password.Length < 6)
            {
                ModelState.AddModelError("Password", "Password must be at least 6 characters long.");
            }

            if (!IsValidEmail(registerModelDto.Email))
            {
                ModelState.AddModelError("Email", "Please enter a valid email address.");
            }
            // Check if there are any validation errors
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // If all validation passes, proceed with user registration
            var registerUser = await _authUser.RegisterAsync(registerModelDto);

            if (!registerUser.IsAuthenticated)
            {
                return BadRequest(registerUser.Message);
            }
            // Return the anonymous object within an Ok result
            return Ok(registerUser);
        }
        [HttpPost("LogIn")]
        public async Task<IActionResult> getTokenAsync(GetTokenRequstDto getTokenRequstDto)
        {
            // Check if the model state is valid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (string.IsNullOrEmpty(getTokenRequstDto.Passward))
            {
                ModelState.AddModelError("Password", "Password is required.");
            }
            else if (getTokenRequstDto.Passward.Length < 6)
            {
                ModelState.AddModelError("Password", "Password must be at least 6 characters long.");
            }

            if (!IsValidEmail(getTokenRequstDto.Email))
            {
                ModelState.AddModelError("Email", "Please enter a valid email address.");
            }
            // Check if there are any validation errors
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // If all validation passes, proceed with user registration
            var registerUser = await _authUser.GetTokenAsync(getTokenRequstDto);

            if (!registerUser.IsAuthenticated)
            {
                return BadRequest(registerUser.Message);
            }
            // Return the anonymous object within an Ok result
            return Ok(registerUser);
        }

        [HttpPost("AddRole")]
        public async Task<IActionResult> AddRoleAsync([FromBody]RoleModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authUser.AddRoleAsync(model);

            if (!string.IsNullOrEmpty(result))
                return BadRequest(result);

            return Ok(model);
        }

        [HttpGet("GetAllAsync/{page}/{pageSize}")]
        public async Task<IActionResult> GetAllAsync(int page = 1, int pageSize = 10)
        {
            if (page <= 0 || pageSize <= 0)
            {
                return BadRequest("Invalid page or pageSize parameters.");
            }

            var ApplicationUsers = await _unitOfWork.ApplicationUsers.GetAllAsync(page, pageSize);

            return Ok(ApplicationUsers);
        }


        [HttpGet("IdentityUser/{id}")]
        public async Task<IActionResult> IdentityUser(int id)
        {
            var identityUser = await _unitOfWork.ApplicationUsers.GetByIdAsync(id);
            return Ok(identityUser);
        }    
        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return false;
            }
            // Regular expression for a more comprehensive email validation
            // This regex pattern covers various valid email formats.
            string emailRegexPattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
                + @"([-a-z0-9!.%]+@(?:[-a-z0-9]+\.)+[a-z]{2,})|"
                + @"([a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@"
                + @"(?:[a-z0-9](?:[-a-z0-9]*[a-z0-9])?\.)+[a-z0-9](?:[-a-z0-9]*[a-z0-9])?))$";

            Regex regex = new Regex(emailRegexPattern, RegexOptions.IgnoreCase);
            return regex.IsMatch(email);
        }

        
    }
}
