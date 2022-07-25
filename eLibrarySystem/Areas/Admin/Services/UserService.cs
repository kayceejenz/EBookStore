using eLibrary.DAL.DataConnection;
using eLibrary.DAL.Helpers;
using eLibrary.DAL.Entity;
using eLibrarySystem.Areas.Admin.Interfaces;
using eLibrarySystem.Areas.Admin.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;

namespace eLibrarySystem.Areas.Admin.Services
{
    public class UserService : IUserService
    {
        /* Instancation of the database context model
         * and injecting some buisness layer services
          */
        #region Instanciation
        readonly eLibraryDatabaseEntities _db;
        public UserService()
        {
            _db = new eLibraryDatabaseEntities();
        }
        public UserService(eLibraryDatabaseEntities db)
        {
            _db = db;
        }
        #endregion

        /* **************************************************************** */
        //User

        // Fetching users
        public List<UserVM> GetUsers()
        {
            var model = _db.Users.Where(x => x.IsDeleted == false).Select(b => new UserVM()
            {
                Id = b.Id,
                Firstname = b.Firstname,
                Lastname = b.Lastname,
                Email = b.Email,
                Address = b.Address,
                PhoneNumber = b.PhoneNumber,
                Role = b.Role.Description,
                Username = b.Username,
                DateCreated = b.DateCreated,
                IsActive = b.IsActive
            }).ToList();
            return model;
        }

        // Creating users
        public bool CreateUser(UserVM vmodel)
        {
            bool HasSaved = false;
            User model = new User()
            {
                Username = vmodel.Username,
                Firstname = vmodel.Firstname,
                Lastname = vmodel.Lastname,
                Email = vmodel.Email,
                PhoneNumber = vmodel.PhoneNumber,
                RoleID = vmodel.RoleID,
                Address = vmodel.Address,
                IsActive = true,
                IsDeleted = false,
                Password = CustomEnrypt.Encrypt(vmodel.Password),
                PasswordSalt = CustomEnrypt.Encrypt(vmodel.PasswordSalt),
                DateCreated = DateTime.Now,
            };
            _db.Users.Add(model);
            _db.SaveChanges();
            HasSaved = true;
            return HasSaved;
        }

        // Get user
        public UserVM GetUser(int ID)
        {
            var model = _db.Users.Where(x => x.Id == ID).Select(b => new UserVM()
            {
                Id = b.Id,
                Firstname = b.Firstname,
                Lastname = b.Lastname,
                Email = b.Email,
                Username = b.Username,
                PhoneNumber = b.PhoneNumber,
                Address = b.Address,
                RoleID = b.RoleID,
            }).FirstOrDefault();
            return model;
        }

        // Editting and updating users
        public bool EditUser(UserVM vmodel)
        {
            bool HasSaved = false;
            var model = _db.Users.FirstOrDefault(x => x.Id == vmodel.Id);
            model.Firstname = vmodel.Firstname;
            model.Lastname = vmodel.Lastname;
            model.Email = vmodel.Email;
            model.PhoneNumber = vmodel.PhoneNumber;
            model.Address = vmodel.Address;
            model.RoleID = vmodel.RoleID;
            model.DateModified = DateTime.Now;
            _db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();
            HasSaved = true;
            return HasSaved;
        }

        // Deleting users
        public bool DeleteUser(int ID)
        {
            bool HasDeleted = false;
            var model = _db.Users.FirstOrDefault(x => x.Id == ID);
            model.IsDeleted = true;
            _db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();
            HasDeleted = true;
            return HasDeleted;
        }

        // Deactivating users
        public bool DeactivateUser(int ID)
        {
            bool HasDeleted = false;
            var model = _db.Users.FirstOrDefault(x => x.Id == ID);
            model.IsActive = false;
            _db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();
            HasDeleted = true;
            return HasDeleted;
        }

        // Activating users
        public bool ActivateUser(int ID)
        {
            bool HasDeleted = false;
            var model = _db.Users.FirstOrDefault(x => x.Id == ID);
            model.IsActive = true;
            _db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();
            HasDeleted = true;
            return HasDeleted;
        }


        /* *************************************************************************** */
        //Role

        // Fetching system roles
        public List<RoleVM> GetRoles()
        {
            var model = _db.Roles.Where(x => x.IsDeleted == false).Select(b => new RoleVM()
            {
                Id = b.Id,
                Description = b.Description
            }).ToList();
            return model;
        }

        // Creating system roles
        public bool CreateRole(RoleVM vmodel)
        {
            bool HasSaved = false;
            Role role = new Role()
            {
                Description = vmodel.Description,
                IsDeleted = false,
                DateCreated = DateTime.Now,
            };
            _db.Roles.Add(role);
            _db.SaveChanges();
            HasSaved = true;
            return HasSaved;
        }

        // Getting system roles
        public RoleVM GetRole(int ID)
        {
            var model = _db.Roles.Where(x => x.Id == ID).Select(b => new RoleVM()
            {
                Id = b.Id,
                Description = b.Description
            }).FirstOrDefault();
            return model;
        }

        // Editting and updating system roles
        public bool EditRole(RoleVM vmodel)
        {
            bool HasSaved = false;
            var model = _db.Roles.Where(x => x.Id == vmodel.Id).FirstOrDefault();
            model.Description = vmodel.Description;
            model.DateModified = DateTime.Now;

            _db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();
            HasSaved = true;
            return HasSaved;
        }

        // Deleting system roles
        public bool DeleteRole(int ID)
        {
            bool HasDeleted = false;
            var model = _db.Roles.Where(x => x.Id == ID).FirstOrDefault();
            model.IsDeleted = true;

            _db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();
            HasDeleted = true;
            return HasDeleted;
        }

        /* ************************************************************************ */
        //Account

        // Checking to verify login creditials for system users
        public List<User> CheckCreditials(UserVM userVM)
        {
            var ecryptedPassword = CustomEnrypt.Encrypt(userVM.Password);
            var user = _db.Users.Where(x => x.Username == userVM.Username && x.Password == ecryptedPassword && x.IsDeleted == false && x.IsActive == true).ToList();
            return user;
        }

        // Checking to verify login creditials for members
        public List<Member> CheckMemberCreditials(UserVM userVM)
        {
            var ecryptedPassword = CustomEnrypt.Encrypt(userVM.Password);
            var user = _db.Members.Where(x => x.Email == userVM.Username && x.Password == ecryptedPassword && x.IsDeleted == false && x.IsActive == true).ToList();
            return user;
        }

        // Fetching all the bonus accumulated by an afflilate user
        public List<AfflilateBonusVM> GetAfflilate_Bouns()
        {
            var afflilates = _db.Users.Where(x => x.Role.Description == "Afflilate" && x.IsActive == true && x.IsDeleted == false).Select(b => new AfflilateBonusVM()
            {
                AfflilateUserID = b.Id,
                AfflilateUser = b.Firstname + " " + b.Lastname,
                afflilateEmail = b.Email
            }).ToList();

            foreach (var afflilate in afflilates)
            {
                var afflilatetotalbonus = _db.AfflilateBonusManagers.Where(x => x.AfflilateUserID == afflilate.AfflilateUserID).ToList();
                if (afflilatetotalbonus.Count() > 0)
                    afflilate.TotalAmount = afflilatetotalbonus.Sum(x => x.Amount);
                else
                    afflilate.TotalAmount = 0.00m;

                NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
                afflilate.TotalAmountString = afflilate.TotalAmount.ToString("N", nfi);

                var afflilateBonuses = new List<Bonus>();
                var bonusTypes = Enum.GetValues(typeof(BonusType)).Cast<BonusType>().Select(value => value.ToString()).ToList();
                foreach(var bonusType in bonusTypes)
                {
                    var bonusval = (BonusType)Enum.Parse(typeof(BonusType), bonusType);
                    var afflilatebonus = _db.AfflilateBonusManagers.FirstOrDefault(x => x.AfflilateUserID == afflilate.AfflilateUserID && x.BonusType == bonusval);
                    var bonus = new Bonus();
                    bonus.BonusType = bonusType;
                    bonus.Amount = afflilatebonus == null ? 0 : afflilatebonus.Amount;
                    bonus.AmountString = bonus.Amount.ToString("N", nfi);

                    afflilateBonuses.Add(bonus);
                }
                afflilate.Bonus = afflilateBonuses;
            }
            return afflilates;
        }
        public List<BookVM> GetBooksAddedByAfflilate(int AfflilateUserID)
        {
            var model = _db.Books.Where(x => x.CreatedByID == AfflilateUserID && x.IsDeleted == false).Select(b => new BookVM()
            {
                Id = b.Id,
                Name = b.Name,
                Author = b.Author,
                Edition = b.Edition,
                Categories = b.Categories.Description,
                DateCreated = b.DateCreated
            }).OrderByDescending(x=>x.DateCreated).ToList();
            return model;
        }
        public List<ArticleVM> GetArticleAddedByAfflilate(int AfflilateUserID)
        {
            byte[] empty = { 4, 3 };
            var model = _db.Articles.Where(x => x.CreatedByID == AfflilateUserID && x.IsDeleted == false).Select(b => new ArticleVM()
            {
                Id = b.Id,
                Description = b.Description,
                Image = b.Image == null ? empty : b.Image,
                DateCreated = b.DateCreated,
            }).OrderByDescending(x => x.DateCreated).ToList();
            return model;
        }
        public List<AuthorVM> GetAuthorAddedByAfflilate(int AfflilateUserID)
        {
            byte[] empty = { 4, 3 };
            var model = _db.Authors.Where(x => x.CreatedByID == AfflilateUserID && x.IsDeleted == false).Select(b => new AuthorVM()
            {
                Id = b.Id,
                Name = b.Name,
                Photo = b.Photo == null ? empty : b.Photo,
                DateCreated = b.DateCreated,
            }).OrderByDescending(x => x.DateCreated).ToList();
            return model;
        }


        // Fetching member
        public List<MemberVM> GetMembers()
        {
            var model = _db.Members.Where(x => x.IsDeleted == false).Select(b => new MemberVM()
            {
                Id = b.Id,
                Name = b.Name,
                Email = b.Email,
                IsActive = b.IsActive
            }).ToList();
            return model;
        }
        
        // Creating membership
        public bool CreateMember(MemberVM vmodel)
        {
            var model = new Member()
            {
                Name = vmodel.Name,
                Email = vmodel.Email,
                Password = CustomEnrypt.Encrypt(vmodel.Password),
                ConfirmPassword = CustomEnrypt.Encrypt(vmodel.ConfirmPassword),
                IsActive = true,
                IsDeleted = false,
                DateCreated = DateTime.Now
            };
            _db.Members.Add(model);
            _db.SaveChanges();

            return true;
        }
        
        // Deactivating membership
        public bool DeactivateMember(int Id)
        {
            var model = _db.Members.FirstOrDefault(x => x.Id == Id);
            model.IsActive = false;
            _db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();
            return true;
        }

        // Get Member wishlist
        public List<MemberWishListVM> GetWishList(int memberID)
        {
            var wishlist = _db.MemberWishLists.Where(x => x.IsDeleted == false && x.MemberID == memberID).Select(b => new MemberWishListVM()
            {
                Id = b.Id,
                BookName = b.Book.Name,
                Category = b.Book.Categories.Description,
                Author = b.Book.Author
            }).ToList();

            return wishlist;
        }

        // Add To WishList
        public bool AddToWishList(MemberWishListVM vmodel)
        {
            bool hasAdded;
            var model = new MemberWishList()
            {
                BookID = vmodel.BookID,
                MemberID = vmodel.MemberID,
                DateAdded = DateTime.Now,
                IsDeleted = false
            };

            try
            {
                _db.MemberWishLists.Add(model);
                _db.SaveChanges();
                hasAdded = true;
            }
            catch(Exception e)
            {
                throw e;
            }

            return hasAdded;
        }

        // Deactivate From WishList
        public bool DeactivateFromWishList(int wishlistID)
        {
            var model = _db.MemberWishLists.FirstOrDefault(x => x.Id == wishlistID);
            model.IsDeleted = true;

            _db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();
            return true;
        }

        // Activate From Wishlist
        public bool ActivateFromWishList(int wishlistID)
        {
            var model = _db.MemberWishLists.FirstOrDefault(x => x.Id == wishlistID);
            model.IsDeleted = false;

            _db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();
            return true;
        }

        // Check if book exist in wishlist
        public bool CheckIfExistInWishList(MemberWishListVM vmmodel)
        {
            var count = _db.MemberWishLists.Count(x =>  x.MemberID == vmmodel.MemberID && x.BookID == vmmodel.BookID);
            if (count > 0)
                return true;
            else
                return false;
        }

        public MemberWishListVM GetWishList(MemberWishListVM vmodel)
        {
            var wishlist = _db.MemberWishLists.Where(x =>x.MemberID == vmodel.MemberID && x.BookID == vmodel.BookID).Select(b=> new MemberWishListVM() {
            Id = b.Id,
            BookID = b.BookID,
            MemberID = b.MemberID
            }).FirstOrDefault();
            return wishlist;
        }

        // Get Member wishlist
        public List<MemberCartVM> GetCartList(int memberID)
        {
            var cartList = _db.MemberCarts.Where(x => x.IsDeleted == false && x.MemberID == memberID).Select(b => new MemberCartVM()
            {
                Id = b.Id,
                BookName = b.Book.Name,
                Category = b.Book.Categories.Description,
                Author = b.Book.Author
            }).ToList();

            return cartList;
        }

        // Add To Cart List
        public bool AddToCartList(MemberCartVM vmodel)
        {
            bool hasAdded;
            var model = new MemberCart()
            {
                BookID = vmodel.BookID,
                MemberID = vmodel.MemberID,
                DateAdded = DateTime.Now,
                IsDeleted = false
            };

            try
            {
                _db.MemberCarts.Add(model);
                _db.SaveChanges();
                hasAdded = true;
            }
            catch (Exception e)
            {
                throw e;
            }

            return hasAdded;
        }

        // Delete From Cartlist
        public bool DeleteFromCartList(int wishlistID)
        {
            var model = _db.MemberCarts.FirstOrDefault(x => x.Id == wishlistID);
            model.IsDeleted = true;

            _db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();
            return true;
        }
    }
}