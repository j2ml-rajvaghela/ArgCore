using ArgCore.Helpers;
using ArgCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArgCore.Controllers
{
    [Authorize]
    public class CompaniesController : Controller
    {
        public PartialViewResult Index(string userId)
        {
            var model = new Companies();
            try
            {
                model.CommonObjects.TopHeading = "Manage Permissions";
                model.UserId = Convert.ToString(userId);

                var companies = Common.ArgClients.GetArgClients();
                if (companies != null && companies.Any())
                {
                    model.CompaniesList = companies;
                }

                var assignComp = Common.ArgClients.GetRelatedUserCompanies(userId);
                if (assignComp != null && assignComp.Any())
                {
                    model.AssignedCompanies = assignComp;
                    model.CompaniesList.RemoveAll(item => model.AssignedCompanies.Exists(y => y.CompanyId == item.CompanyId));
                }
                //foreach (var comp in data.CompaniesList)
                //{
                //    var exists = assignComp.Exists(x => x.CompanyId == comp.CompanyId);
                //    if (exists)
                //        data.CompaniesList.Remove(comp);
                //}
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex.ToString());
            }
            return PartialView(model);
        }

        [HttpPost]
        public void AssignCompany(string userId, int companyId)
        {
            try
            {
                Common.UserCompanyRels.AssignCompany(userId, companyId);
                Common.ActivityStats.SaveActivityStats(Arg.DataAccess.ActivityStatsImpl.EnumActions.Saved, companyId, "Assigned Company in UserCompanyRels");
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }
        }

        [HttpPost]
        public void RemoveCompany(string userId, int companyId)
        {
            try
            {
                Common.UserCompanyRels.RemoveAssignedComp(userId, companyId);
                Common.ActivityStats.SaveActivityStats(Arg.DataAccess.ActivityStatsImpl.EnumActions.Deleted, companyId, "Removed Company from UserCompanyRels");
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }
        }
    }
}
