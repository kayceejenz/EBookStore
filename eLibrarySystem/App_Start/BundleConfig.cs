using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace eLibrarySystem.App_Start
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundle)
        {
            BundleTable.EnableOptimizations = true;
            // Style bundles
            bundle.Add(new StyleBundle("~/Content/main")
                .Include("~/Contents/Styles/main.d810cf0ae7f39f28f336.css"));

            bundle.Add(new StyleBundle("~/Content/homestyle")
                .Include("~/Contents/Styles/HomeStyle.css"));

            bundle.Add(new StyleBundle("~/Content/jstree")
               .Include("~/Scripts/jsTree3/dist/themes/default/style.min.css"));

            bundle.Add(new StyleBundle("~/Content/clientStyle")
            .Include("~/Contents/Styles/ClientsStyle.css"));

            bundle.Add(new StyleBundle("~/Content/animate")
           .Include("~/Contents/Styles/OwlCarousel2-2.2.1/aminate.css"));

            bundle.Add(new StyleBundle("~/Content/owl-carousel")
          .Include("~/Contents/Styles/OwlCarousel2-2.2.1/owl.carousel.css"));

            bundle.Add(new StyleBundle("~/Content/owl-theme")
          .Include("~/Contents/Styles/OwlCarousel2-2.2.1/owl.theme.default.css"));

            bundle.Add(new StyleBundle("~/Content/responsive")
           .Include("~/Contents/Styles/responsive.css"));

            bundle.Add(new StyleBundle("~/Content/slick")
          .Include("~/Contents/Styles/slick-1.8.0/slick.css"));

            bundle.Add(new StyleBundle("~/Content/bookstore")
        .Include("~/Contents/Styles/shop_styles.css"));

            bundle.Add(new StyleBundle("~/Content/jquery-ui")
.Include("~/Contents/Styles/jquery-ui/jquery-ui.css"));

            bundle.Add(new StyleBundle("~/Content/bookstore-responive")
        .Include("~/Contents/Styles/shop_responsive.css"));

            bundle.Add(new StyleBundle("~/Content/BookView_styles")
       .Include("~/Contents/Styles/product_styles.css"));

            bundle.Add(new StyleBundle("~/Content/contact-responive")
        .Include("~/Contents/Styles/contact_responsive.css"));


            bundle.Add(new StyleBundle("~/Content/contact-style")
      .Include("~/Contents/Styles/contact_styles.css"));


            bundle.Add(new StyleBundle("~/Content/BookView_responsive")
        .Include("~/Contents/Styles/product_responsive.css"));
            /* ***************************************************************************************** */

            // Script bundles

            bundle.Add(new ScriptBundle("~/bundles/jquery-unobtrusive-ajax")
.Include("~/Scripts/jquery.unobtrusive-ajax.min.js"));

            bundle.Add(new ScriptBundle("~/bundles/jquery")
                .Include("~/Scripts/jquery-{version}.js"));

            bundle.Add(new ScriptBundle("~/bundles/jquery-ajax")
            .Include("~/Scripts/Ajax.jquery.min.js"));

            bundle.Add(new ScriptBundle("~/bundles/main")
               .Include("~/Scripts/main.d810cf0ae7f39f28f336.js"));

            bundle.Add(new ScriptBundle("~/bundles/sweet-alert")
               .Include("~/Scripts/SweetAlert.js"));

            bundle.Add(new ScriptBundle("~/bundles/global")
               .Include("~/Scripts/Global.js"));

            bundle.Add(new ScriptBundle("~/bundles/jstree").Include(
"~/Scripts/jsTree3/dist/jstree.min.js"));

            bundle.Add(new ScriptBundle("~/bundles/jstree-custom")
.Include("~/Scripts/jsTree3/Custom.js"));

            bundle.Add(new ScriptBundle("~/bundles/afflilate")
                .Include("~/Areas/Admin/Scripts/Afflilate.js"));

            bundle.Add(new ScriptBundle("~/bundles/article")
               .Include("~/Areas/Admin/Scripts/Article.js"));

            bundle.Add(new ScriptBundle("~/bundles/articlecontent")
               .Include("~/Areas/Admin/Scripts/ArticleContent.js"));

            bundle.Add(new ScriptBundle("~/bundles/author")
               .Include("~/Areas/Admin/Scripts/Author.js"));

            bundle.Add(new ScriptBundle("~/bundles/book")
               .Include("~/Areas/Admin/Scripts/Book.js"));

            bundle.Add(new ScriptBundle("~/bundles/category")
               .Include("~/Areas/Admin/Scripts/Category.js"));

            bundle.Add(new ScriptBundle("~/bundles/role")
               .Include("~/Areas/Admin/Scripts/Role.js"));

            bundle.Add(new ScriptBundle("~/bundles/user")
               .Include("~/Areas/Admin/Scripts/User.js"));

            bundle.Add(new ScriptBundle("~/bundles/localsearch")
              .Include("~/Scripts/JsLocalSearch.js"));

            bundle.Add(new ScriptBundle("~/bundles/validation")
             .Include("~/Scripts/Validation.js"));

            bundle.Add(new ScriptBundle("~/bundles/editor")
           .Include("~/Scripts/Editor.js"));

            bundle.Add(new ScriptBundle("~/bundles/addquestion")
           .Include("~/Scripts/AddQuestion.js"));

            bundle.Add(new ScriptBundle("~/bundles/timymc")
          .Include("~/Scripts/Tinymc.js"));

            bundle.Add(new ScriptBundle("~/bundles/previewer")
          .Include("~/Scripts/Previewer.js"));

            bundle.Add(new ScriptBundle("~/bundles/moment")
        .Include("~/Scripts/moment.js"));

            bundle.Add(new ScriptBundle("~/bundles/homeutil")
        .Include("~/Scripts/HomeUtils.js"));

            bundle.Add(new ScriptBundle("~/bundles/TweenMax")
      .Include("~/Scripts/greensock/TweenMax.min.js"));

            bundle.Add(new ScriptBundle("~/bundles/TimeLineMax")
    .Include("~/Scripts/greensock/Timeline.min.js"));

            bundle.Add(new ScriptBundle("~/bundles/ScrollTo")
    .Include("~/Scripts/greensock/ScrollToPlugin.min.js"));

            bundle.Add(new ScriptBundle("~/bundles/animation")
    .Include("~/Scripts/greensock/animate.gasp.min.js"));


            bundle.Add(new ScriptBundle("~/bundles/isotope")
    .Include("~/Scripts/isotope/isotope.pkgd.min.js"));

            bundle.Add(new ScriptBundle("~/bundles/parallex")
 .Include("~/Scripts/parallex-js-master/parallex.min.js"));

            bundle.Add(new ScriptBundle("~/bundles/slick")
.Include("~/Contents/Styles/slick-1.8.0/slick.js"));

            bundle.Add(new ScriptBundle("~/bundles/easing")
   .Include("~/Scripts/easing/easing.js"));

            bundle.Add(new ScriptBundle("~/bundles/jquery-ui")
.Include("~/Contents/Styles/jquery-ui/jquery-ui.js"));

            bundle.Add(new ScriptBundle("~/bundles/owl-carsousel")
  .Include("~/Contents/Styles/OwlCarousel2-2.2.1/owl.carousel.js"));

            bundle.Add(new ScriptBundle("~/bundles/custom")
.Include("~/Scripts/custom.js"));

            bundle.Add(new ScriptBundle("~/bundles/bookstore-custom")
.Include("~/Scripts/shop_custom.js"));

            bundle.Add(new ScriptBundle("~/bundles/book-custom")
.Include("~/Scripts/product_custom.js"));

            bundle.Add(new ScriptBundle("~/bundles/contact-custom")
.Include("~/Scripts/contact_custom.js"));
        }
    }
}