using ArgCore.Attributes;
using ArgCore.Helpers;
using ArgCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ArgCore.Controllers
{
    [Authorize]
    public class TemplateCatsController : Controller
    {
        public IActionResult Index(string q)
        {
            try
            {
                //if (HttpContext.Session.GetString("IsSessionActive") == null)
                //{
                //    return RedirectToAction("LogIn", "Account");
                //}

                var templateCats = new TemplateCats();
                templateCats.CommonObjects.TopHeading = "Template Categories";
                templateCats.CommonObjects.Heading = "Template Categories";
                templateCats.TemplateCatsList = new List<Arg.DataModels.TemplateCats>();
                templateCats.TemplateCatsList = Common.TemplateCats.GetTemplateCats(0, q);

                return View(templateCats);
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }
            return null;
        }

        [HttpGet]
        public IActionResult Save(int? catId)
        {
            try
            {
                //if (HttpContext.Session.GetString("IsSessionActive") == null)
                //{
                //     return RedirectToAction("LogIn", "Account");
                //}

                var templateCats = new TemplateCats();
                templateCats.CommonObjects.TopHeading = "Template Categories";
                var _tempId = Convert.ToInt32(catId);
                templateCats.TemplateCatDetail = new Arg.DataModels.TemplateCats();
                if (_tempId > 0)
                {
                    templateCats.CommonObjects.Heading = "Edit Template Category";
                    templateCats.TemplateCatDetail = Common.TemplateCats.GetTemplateCat(_tempId, "");

                    if (templateCats.TemplateCatDetail == null || templateCats.TemplateCatDetail.CatId <= 0)
                    {
                        return RedirectToAction("Index", new { m = "Template not found or deleted" });
                    }

                }
                else
                {
                    templateCats.CommonObjects.Heading = "Add Template Category";
                }

                return View(templateCats);
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }
            return null;
        }

        [HttpPost]
        public IActionResult Save(TemplateCats templateCats)
        {
            try
            {
                var templateCatNameExist = Common.TemplateCats.TemplateCatsExist(templateCats.TemplateCatDetail.Name, templateCats.TemplateCatDetail.CatId);
                if (templateCatNameExist.Count > 0)
                {
                    templateCats.ErrorMessage = "Template category name already exists with this Category";
                    return View(templateCats);
                }

                templateCats.TemplateCatDetail.AddedBy = Common.CurrentUserId;
                Common.TemplateCats.SaveTemplateCat(templateCats.TemplateCatDetail);

                if (templateCats.TemplateCatDetail.CatId > 0)
                {
                    Common.ActivityStats.SaveActivityStats(Arg.DataAccess.ActivityStatsImpl.EnumActions.Saved, 0, "Template Categories");
                    return RedirectToAction("Index", "TemplateCats");
                }
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }
            return RedirectToAction("Index", "TemplateCats"); 
        }

        [AuthorizeUser]
        public IActionResult Delete(int catId)
        {
            try
            {
                var result = Common.TemplateCats.DeleteTemplateCat(catId);
                if (result > 0)
                {
                    Common.ActivityStats.SaveActivityStats(Arg.DataAccess.ActivityStatsImpl.EnumActions.Deleted, 0, "Template Cats");
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
    }
}
