using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZiraatBankDbParser.Models
{
    public class Transaction
    {
        public int ID { get; set; }
        public string FinInst { get; set; }
        public DateTime FxSetDate { get; set; }
        public string RecFileId { get; set; }
        public string TransCurrency { get; set; }
        public string ReconCurrency { get; set; }

        public decimal TransAmountCreditTotal { get; set; }
        public decimal ReconAmountCreditTotal { get; set; }
        public decimal FeeAmountCreditTotal { get; set; }
        public decimal TransAmountDebitTotal { get; set; }
        public decimal ReconAmountDebitTotal { get; set; }
        public decimal FeeAmountDebitTotal { get; set; }
        public int CountTotalTotal { get; set; }
        public decimal NetValueTotal { get; set; }

        public ICollection<SettlementCategory> SettlementCategories { get; set; }
    }

}

