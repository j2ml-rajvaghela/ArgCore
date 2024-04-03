using ArgCore.Helpers;
using ArgCore.Models;
using Microsoft.AspNetCore.Mvc;

namespace ArgCore.Controllers
{
    public class CurrencyConversionRatesController : Controller
    {
        public IActionResult Index()
        {
            try
            {
                //if (HttpContext.Session.GetString("IsSessionActive") == null)
                //{
                //    return RedirectToAction("LogIn", "Account");
                //}

                var currencyConversionRates = new CurrencyConversionRates();
                currencyConversionRates.CommonObjects.TopHeading = "Currency Conversion Rates";
                currencyConversionRates.CommonObjects.Heading = "Currency Conversion ates";
                currencyConversionRates.CurrencyConversionRatesList = new List<Arg.DataModels.CurrencyConversionRates>();
                currencyConversionRates.CurrencyConversionRatesList = Common.CurrencyConversionRates.GetCurrencyConversionRates();

                return View(currencyConversionRates);
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }
            return null;
        }

        [HttpGet]
        public IActionResult Save(int? currencyId)
        {
            try
            {
                //if (HttpContext.Session.GetString("IsSessionActive") == null)
                //{
                //    return RedirectToAction("LogIn", "Account");
                //}

                var currencyConversionRates = new CurrencyConversionRates();
                currencyConversionRates.CommonObjects.TopHeading = "Currency Conversion Rates";

                var _currencyId = Convert.ToInt32(currencyId);

                currencyConversionRates.CurrencyConversionRatesDetail = new Arg.DataModels.CurrencyConversionRates();
                if (_currencyId > 0)
                {
                    currencyConversionRates.CommonObjects.Heading = "Edit Currency Rate Conversion";
                    currencyConversionRates.CurrencyConversionRatesDetail = Common.CurrencyConversionRates.GetCurrencyConversionRates(_currencyId).FirstOrDefault();

                    if (currencyConversionRates.CurrencyConversionRatesDetail == null || currencyConversionRates.CurrencyConversionRatesDetail.CurrencyId <= 0)
                    {
                        return RedirectToAction("CurrencyConversionRates", new { m = "Currency Conversion Rate not found or deleted" });
                    }
                }
                else
                {
                    currencyConversionRates.CommonObjects.Heading = "Add Currency Conversion Rates";
                }
                return View(currencyConversionRates);
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }
            return null;
        }

        [HttpPost]
        public IActionResult Save(CurrencyConversionRates currencyConversionRate)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    Common.CurrencyConversionRates.SaveCurrencyConversionRate(currencyConversionRate.CurrencyConversionRatesDetail);

                    if (currencyConversionRate.CurrencyConversionRatesDetail.CurrencyId > 0)
                    {
                        Common.ActivityStats.SaveActivityStats(Arg.DataAccess.ActivityStatsImpl.EnumActions.Saved, 0, "Currency Conversion Rate");
                        return RedirectToAction("Index", "CurrencyConversionRates");
                    }
                }
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }
            return RedirectToAction("Index", "CurrencyConversionRates");
        }

    }


}
