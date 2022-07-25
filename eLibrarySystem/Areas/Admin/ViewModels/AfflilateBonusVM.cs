using eLibrary.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eLibrarySystem.Areas.Admin.ViewModels
{
    public class AfflilateBonusVM
    {
        public int Id { get; set; }
        public int? AfflilateUserID { get; set; }
        public string AfflilateUser { get; set; }
        public string afflilateEmail { get; set; }
        public decimal TotalAmount { get; set; }
        public string TotalAmountString { get; set; }
        public IList<Bonus> Bonus { get; set; }
    }
    public class Bonus
    {
        public string BonusType { get; set; }
        public decimal Amount { get; set; }
        public string AmountString { get; set; }
    }
}