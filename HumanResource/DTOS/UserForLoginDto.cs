using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HumanResource.DTOS
{
    public class UserForLoginDto
    {
        
        public string Username { get; set; }

        public string password { get; set; }
    }
}
