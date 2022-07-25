using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eLibrarySystem.Areas.Admin.ViewModels
{
    public class AuthorVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Biography { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string CountryOfOrigin { get; set; }
        public string StateOfOrigin { get; set; }
        public string ContactAddress { get; set; }
        public string ImageString { get; set; }
        public byte[] Photo { get; set; }
        public DateTime DateCreated { get; set; }
    }
}