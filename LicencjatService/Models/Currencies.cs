using Microsoft.Azure.Mobile.Server;
using Microsoft.Azure.Mobile.Server.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace KursyWalutService.Models
{
    [Table("tblCurrencies")]
    public class Currencies : EntityData
    {
        //public string Id { get; set; }
        public string UserID { get; set; }

        public string FirstCurrency { get; set; }
        public string FirstCurrencyValue { get; set; }
        public bool FirstCurrencyOver { get; set; }

        public string SecondCurrency { get; set; }
        public string SecondCurrencyValue { get; set; }
        public bool SecondCurrencyOver { get; set; }

        public string ThirdCurrency { get; set; }
        public string ThirdCurrencyValue { get; set; }
        public bool ThirdCurrencyOver { get; set; }

        public string FourthCurrency { get; set; }
        public string FourthCurrencyValue { get; set; }
        public bool FourthCurrencyOver { get; set; }

        public string FifthCurrency { get; set; }
        public string FifthCurrencyValue { get; set; }
        public bool FifthCurrencyOver { get; set; }
    }
}