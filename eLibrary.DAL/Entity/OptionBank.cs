using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLibrary.DAL.Entity
{
    public class OptionBank
    {
        public int Id { get; set; }
        public int? QuestionID { get; set; }
        public string OptionText { get; set; }
        public bool IsDeleted { get; set; }

        [ForeignKey("QuestionID")]
        public QuestionBank Question { get; set; }
    }
}
