using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rootics.Core.Dtos
{
    public class RegisterModelDto
    {

        [Required, StringLength(50)]
        public string UserName { get; set; }

        [Required, StringLength(128), EmailAddress]
        public string Email { get; set; }

        [Required, StringLength(256)]
        public string Password { get; set; }

        [Required, StringLength(256), Compare("Password")]
        public string ConfirmPassword { get; set; } // Added confirmation password
    }
}
