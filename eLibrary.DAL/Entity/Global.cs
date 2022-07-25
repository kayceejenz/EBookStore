using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace eLibrary.DAL.Entity
{
    public class Global
    {
        public static int AuthenticatedUserID { get; set; }
        public static int AuthenticatedMemberID { get; set; }
        public static int SubCategoryID { get; set; }
        public static int CategoryID { get; set; }
        public static  string ErrorMessage { get; set; }
        public static string ReturnUrl { get; set; }
    }
    public enum BookFormat
    {
        Soft_Copy = 1,
        Hard_Copy
    }
    public enum ContentFormat
    {
        Audio = 1,
        Video,
        Text,
        Picture
    }
    public enum BonusType
    {
        Book = 1,
        Article,
        Author
    }
    public enum StandsCategory
    {
        Featured = 1,
        BestRated,
        MostVisted,
        Recommended
    }
    public enum QuestionType
    {
        [EnumDisplayName(DisplayName = "MULTI CHOICE")]
        Multi_Choice = 1,
        [EnumDisplayName(DisplayName = "FILL IN THE BLANK")]
        Fill_in_the_blank
    }
    public class EnumDisplayNameAttribute : Attribute
    {
        private string _displayName;
        public string DisplayName
        {
            get
            {
                return _displayName;
            }
            set
            {
                _displayName = value;
            }
        }
    }

    public static class EnumExtensions
    {

        public static string DisplayName(this Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());

            EnumDisplayNameAttribute attribute
                = Attribute.GetCustomAttribute(field, typeof(EnumDisplayNameAttribute))
            as EnumDisplayNameAttribute;

            return attribute == null ? value.ToString() : attribute.DisplayName;
        }
    }

}


