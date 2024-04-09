using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Rootics.API.Helpers;
using Rootics.Core.Dtos;
using Rootics.Core.InterFaces;
using Rootics.Core.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Rootics.EF.Repsotories
{
    public class AuthUser : IAuthUser
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthUser(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JWT _jwt;

        public AuthUser(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IOptions<JWT> jwt)
        {
            _userManager = userManager;
            _roleManager=roleManager;
            _jwt = jwt.Value;
        }
        public async Task<rooticsUser> RegisterAsync(RegisterModelDto registerModelDto)
        {
            if(await _userManager.FindByEmailAsync(registerModelDto.Email) != null)
            {
                return new rooticsUser { Message = "Email Is Already Register!" };
            }
            if (await _userManager.FindByEmailAsync(registerModelDto.UserName) != null)
            {
                return new rooticsUser { Message = "UserName Is Already Register!" };
            }
            var User = new ApplicationUser
            {
                Email = registerModelDto.Email,
                UserName = registerModelDto.UserName,
            };
            var Result = await _userManager.CreateAsync(User, registerModelDto.Password);
            if (!Result.Succeeded)
            {
                var errors = string.Empty;
                foreach (var error in Result.Errors)
                {
                    errors += $"{error.Description},";
                }
                return new rooticsUser { Message = errors };
            }
            await _userManager.AddToRoleAsync(User,"User");
            var jwtSecurityToken = await CreateJwtToken(User);
            return new rooticsUser
            {
                Email=User.Email,
                ExpiresOn=jwtSecurityToken.ValidTo,
                IsAuthenticated = true,
                Roles = new List<string> { "User" },
                Token=new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                UserName = User.UserName,
            };

        }

        public async Task<rooticsUser> GetTokenAsync(GetTokenRequstDto getTokenRequstDto)
        {
            var authUser = new rooticsUser();

            var User = await _userManager.FindByEmailAsync(getTokenRequstDto.Email);
            if (User == null || !await _userManager.CheckPasswordAsync(User, getTokenRequstDto.Passward))
            {
                return new rooticsUser { Message = "Email or Passward is incorrect" };
            }
            var jwtSecurityToken = await CreateJwtToken(User);
            var rolesList = await _userManager.GetRolesAsync(User);


            authUser.IsAuthenticated = true;
            authUser.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            authUser.Email = User.Email;
            authUser.ExpiresOn = jwtSecurityToken.ValidTo;
            authUser.UserName = User.UserName;
            authUser.Roles = rolesList.ToList();

            return authUser;
        }


        public async Task<string> AddRoleAsync(RoleModel roleModel)
        {
            var user = await _userManager.FindByIdAsync(roleModel.UserId);

            if (user is null || !await _roleManager.RoleExistsAsync(roleModel.Role))
                return "Invalid user ID or Role";

            if (await _userManager.IsInRoleAsync(user, roleModel.Role))
                return "User already assigned to this role";

            var result = await _userManager.AddToRoleAsync(user, roleModel.Role);

            return result.Succeeded ? string.Empty : "Sonething went wrong";
        }

        private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.Add(new Claim("roles", role));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.Now.AddDays(_jwt.DurationInDays),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }
    }
}
