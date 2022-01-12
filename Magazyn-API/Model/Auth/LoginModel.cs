using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Magazyn_API.Model.Auth
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Email jest obowiązkowy!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Hasło jest obowiązkowe!")]
        public string Password { get; set; }
    }
}
