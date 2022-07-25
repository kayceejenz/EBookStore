using eLibrary.DAL.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLibrary.DAL.DataConnection
{
    public class Seed
    {
        public static void DatabaseSeed(eLibraryDatabaseEntities context)
        {
            context.ApplicationSettings.AddOrUpdate(x => x.Id,
          new Entity.ApplicationSettings()
          {
              Id = 1,
              AppName = "eLibrary",
              MemberOvertimeDebt = 1000,
          });
            context.Roles.AddOrUpdate(x => x.Id,
         new Entity.Role()
         {
             Id = 1,
             Description = "Super administrator",
             IsDeleted = false,
             DateCreated = DateTime.Now
         });

            context.Users.AddOrUpdate(x => x.Id,
  new Entity.User()
  {
      Id = 1,
      Firstname = "Kosisochukwu",
      Lastname = "Jenz",
      Email = "kcokolo10@gmail.com",
      Address = "St. Theresa's parish",
      PhoneNumber = "09056893344",
      Username = "Administrator",
      Password = CustomEnrypt.Encrypt("Legendary"),
      PasswordSalt = CustomEnrypt.Encrypt("Legendary"),
      RoleID = 1,
      IsActive = true,
      IsDeleted = false,
      DateCreated = DateTime.Now
  });

            //Save all changes
            context.SaveChanges();
        }
    }
}
