using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PDev.Auth.Api.Models
{
    public sealed class SendNewPasswordModel
    {
        public string Email { get; set; }
    }
}