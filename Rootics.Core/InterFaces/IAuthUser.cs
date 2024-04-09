using Rootics.Core.Dtos;
using Rootics.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rootics.Core.InterFaces
{
    public interface IAuthUser
    {
        Task<rooticsUser> RegisterAsync(RegisterModelDto registerModelDto);
        Task<rooticsUser> GetTokenAsync(GetTokenRequstDto getTokenRequstDto);
        Task<string> AddRoleAsync(RoleModel roleModel);

    }
}
