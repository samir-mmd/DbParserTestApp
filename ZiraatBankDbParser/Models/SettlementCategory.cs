using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZiraatBankDbParser.Models
{
    public class SettlementCategory
    {
        public int ID { get; set; }
        public int TransactionID { get; set; }
        public string CatName { get; set; }
        public decimal TransAmountCredit { get; set; }
        public decimal ReconAmountCredit { get; set; }
        public decimal FeeAmountCredit { get; set; }
        public decimal TransAmountDebit { get; set; }
        public decimal ReconAmountDebit { get; set; }
        public decimal FeeAmountDebit { get; set; }
        public int CountTotal { get; set; }
        public decimal NetValue { get; set; }

        public Transaction Transaction { get; set; }
    }
}
