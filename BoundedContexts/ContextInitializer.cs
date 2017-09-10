using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace PDev.Auth.Api.BoundedContexts
{
    public class ContextInitializer<T> : IDatabaseInitializer<T> where T : DbContext
    {
        public void InitializeDatabase(T context)
        {
            // Do nothing, thats the sense of it!
        }
    }
}