using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZiraatBankDbParser.Models;

namespace ZiraatBankDbParser.Data
{
    public static class DbInitializer
    {
        public static void Initialize(MainContext context)
        {
            context.Database.EnsureCreated();
          
            if (context.Transactions.Any())
            {
                return;  
            }
        }
    }
}
