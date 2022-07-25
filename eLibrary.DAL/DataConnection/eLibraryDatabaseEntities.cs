using eLibrary.DAL.Entity;
using eLibrary.DAL.Migrations;
using System.Data.Entity;

namespace eLibrary.DAL.DataConnection
{
    public class eLibraryDatabaseEntities : DbContext
    {
        public eLibraryDatabaseEntities() : base("name=eLibraryDatabaseEntities")
        {
           Database.SetInitializer(new MigrateDatabaseToLatestVersion<eLibraryDatabaseEntities, Configuration>());
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
        public virtual DbSet<ApplicationSettings> ApplicationSettings { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Permission> Permissions { get; set; }
        public virtual DbSet<RolePermission> RolePermissions { get; set; }
        public virtual DbSet<Categories> Categories { get; set; }
        public virtual DbSet<Article> Articles { get; set; }
        public virtual DbSet<ArticleContent> ArticleContents { get; set; }
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<Author> Authors { get; set; }
        public virtual DbSet<BookSubCategories> BookSubCategories { get; set; }
        public virtual DbSet<AfflilateBonusManager> AfflilateBonusManagers { get; set; }
        public virtual DbSet<QuestionBank> QuestionBanks { get; set; }
        public virtual DbSet<OptionBank> OptionBanks { get; set; }
        public virtual DbSet<AnswerBank> AnswerBanks { get; set; }
        public virtual DbSet<ArticleContentQuestionMapping> ArticleContentQuestionMappings { get; set; }
        public virtual DbSet<Member> Members { get; set; }
        public virtual DbSet<TestXPaper> TestXPapers { get; set; }
        public virtual DbSet<MemberCart> MemberCarts { get; set; }
        public virtual DbSet<MemberWishList> MemberWishLists { get; set; }
    }
}
