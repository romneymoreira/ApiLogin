using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PDev.Auth.Api.Domain;

namespace PDev.Auth.Api.Models
{
    public class LoginResultModel
    {
        public LoginResultModel() { }

        public LoginResultModel(bool success, string message)
        {
            Success = success;
            Message = message;
        }

        public string Message { get; set; }
        public bool Success { get; set; }
        public User User { get; set; }
    }
}