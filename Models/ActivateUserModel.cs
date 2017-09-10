using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PDev.Auth.Api.Models
{
    public sealed class ActivateUserModel
    {
        public string NewPassword { get; set; }
        public string Token { get; set; }
        public int UserId { get; set; }
    }
}