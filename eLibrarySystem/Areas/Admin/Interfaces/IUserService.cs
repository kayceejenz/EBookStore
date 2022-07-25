using eLibrary.DAL.Entity;
using eLibrarySystem.Areas.Admin.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLibrarySystem.Areas.Admin.Interfaces
{
    interface IUserService
    {
        // Users
        List<UserVM> GetUsers();
        bool CreateUser(UserVM vmodel);
        UserVM GetUser(int ID);
        bool EditUser(UserVM vmodel);
        bool DeleteUser(int ID);
        bool DeactivateUser(int ID);
        bool ActivateUser(int ID);

        //Roles
        List<RoleVM> GetRoles();
        bool CreateRole(RoleVM vmodel);
        RoleVM GetRole(int ID);
        bool EditRole(RoleVM vmodel);
        bool DeleteRole(int ID);

        List<User> CheckCreditials(UserVM userVM);
        List<AfflilateBonusVM> GetAfflilate_Bouns();
        List<BookVM> GetBooksAddedByAfflilate(int AfflilateUserID);
        List<ArticleVM> GetArticleAddedByAfflilate(int AfflilateUserID);
        List<AuthorVM> GetAuthorAddedByAfflilate(int AfflilateUserID);

        List<MemberVM> GetMembers();
        bool CreateMember(MemberVM vmodel);
        bool DeactivateMember(int Id);
        List<Member> CheckMemberCreditials(UserVM userVM);

        List<MemberWishListVM> GetWishList(int memberID);
        bool AddToWishList(MemberWishListVM vmodel);
        bool DeactivateFromWishList(int wishlistID);
        bool ActivateFromWishList(int wishlistID);
        bool CheckIfExistInWishList(MemberWishListVM vmmodel);
        MemberWishListVM GetWishList(MemberWishListVM vmodel);
        List<MemberCartVM> GetCartList(int memberID);
        bool AddToCartList(MemberCartVM vmodel);
        bool DeleteFromCartList(int wishlistID);

    }
}
