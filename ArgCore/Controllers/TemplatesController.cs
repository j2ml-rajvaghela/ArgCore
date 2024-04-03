using ArgCore.Attributes;
using ArgCore.Helpers;
using ArgCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ArgCore.Controllers
{
    [Authorize]
    [AuthorizeUser]
    public class TemplatesController : Controller
    {
        public IActionResult Index(string q)
        {
            try
            {
                //if (HttpContext.Session.GetString("IsSessionActive") == null)
                //{
                //    return RedirectToAction("LogIn", "Account");
                //}

                var templates = new Templates();
                templates.CommonObjects.TopHeading = "Templates";
                templates.CommonObjects.Heading = "Templates";
                templates.TemplatesList = new List<Arg.DataModels.Templates>();
                templates.TemplatesList = Common.Templates.GetTemplates(q);

                return View(templates);
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }
            return null;
        }

        [HttpGet]
        public IActionResult Save(int? templateId)
        {
            try
            {
                //if (HttpContext.Session.GetString("IsSessionActive") == null)
                //{
                //  return RedirectToAction("LogIn", "Account");
                //}

                var templates = new Templates();
                templates.CommonObjects.TopHeading = "Templates";

                var _templateId = Convert.ToInt32(templateId);
                templates.TemplateDetail = new Arg.DataModels.Templates();

                if (_templateId > 0)
                {
                    templates.CommonObjects.Heading = "Edit Template";
                    templates.TemplateDetail = Common.Templates.GetTemplate(_templateId, "");

                    if (templates.TemplateDetail == null || templates.TemplateDetail.TemplateId <= 0)
                    {
                        return RedirectToAction("Templates", new { m = "Template not found or deleted" });
                    }

                }
                else
                {
                    templates.CommonObjects.Heading = "Add Template";
                }
                templates.Categories = new SelectList(Common.TemplateCats.GetTemplateCats(0, ""), "CatId", "Name");

                return View(templates);
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }
            return null;
        }

        [HttpPost]
        public IActionResult Save(Templates template)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var templateNameExist = Common.Templates.GetTemplatesExist(template.TemplateDetail.Name, template.TemplateDetail.CatId, template.TemplateDetail.TemplateId);
                    if (templateNameExist.Count > 0)
                    {
                        template.Categories = new SelectList(Common.TemplateCats.GetTemplateCats(0, ""), "CatId", "Name");
                        template.ErrorMessage = "Template Name already exists with this Category";
                        return View(template);
                    }

                    template.TemplateDetail.AddedBy = Common.CurrentUserId;
                    Common.Templates.SaveTemplate(template.TemplateDetail);

                    if (template.TemplateDetail.TemplateId > 0)
                    {
                        Common.ActivityStats.SaveActivityStats(Arg.DataAccess.ActivityStatsImpl.EnumActions.Saved, 0, "Templates");
                        return RedirectToAction("Index", "Templates");
                    }
                }
                else
                {
                    template.Categories = new SelectList(Common.TemplateCats.GetTemplateCats(0, ""), "CatId", "Name");
                    return View(template);
                }
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }
            return RedirectToAction("Index", "Templates");
        }

        public IActionResult Delete(int templateId)
        {
            try
            {
                var result = Common.Templates.DeleteTemplate(templateId);
                if (result > 0)
                {
                    Common.ActivityStats.SaveActivityStats(Arg.DataAccess.ActivityStatsImpl.EnumActions.Deleted, 0, "Templates");
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
