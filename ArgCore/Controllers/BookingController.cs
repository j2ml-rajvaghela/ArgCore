using Arg.DataModels;
using ArgCore.Attributes;
using ArgCore.Helpers;
using ArgCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using X.PagedList;

namespace ArgCore.Controllers
{
    public class BookingController : Controller
    {
        public IActionResult Client()
        {
            var bolAuditing = new BOLAuditing();

            try
            {
                //if (HttpContext.Session.GetString("IsSessionActive") == null)
                //{
                //    return RedirectToAction("LogIn", "Account");
                //}

                bolAuditing.CommonObjects.TopHeading = "Atlas Auditing";
                bolAuditing.CommonObjects.Heading = "BOL Auditing";

                var companies = Common.ArgClients.GetBOLClients(Common.CurrentUserId);
                bolAuditing.Companies = new SelectList(companies, "CompanyId", "Name");

                if (Arg.DataAccess.ActiveClient.Info.CompanyId > 0)
                {
                    bolAuditing.CompanyId = Arg.DataAccess.ActiveClient.Info.CompanyId;
                }
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }
            return View(bolAuditing);
        }

        [HttpPost]
        public IActionResult Client(BOLAuditing info)
        {
            try
            {
                if (Arg.DataAccess.ActiveClient.Info.CompanyId > 0)
                {
                    info.CompanyId = Arg.DataAccess.ActiveClient.Info.CompanyId;
                    if (Arg.DataAccess.ActiveClient.Info.BolBilltype && info.InvoiceType == "Per Diem Invoice")
                    {
                        return RedirectToAction("Index_v1");
                    }
                    else
                    {
                        return RedirectToAction("Index");
                    }
                }
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }
            return RedirectToAction("Client");
        }

        [Authorize]
        public JsonResult SetActiveClient(int companyId)
        {
            try
            {
                if (companyId > 0)
                {
                    var clientInfo = Arg.DataAccess.ActiveClient.Set(companyId);
                    if (clientInfo != null && clientInfo.CompanyId > 0)
                    {
                        return Json(clientInfo);
                    }
                }
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
                return Json("Error");
            }
            return null;
        }

        public IActionResult Index(BOLAuditing bolModel, string ButtonType, List<string> CustId, List<string> orig, List<string> dest, List<string> POL, int? queryId)
        {
            var bOLAuditing = new BOLAuditing();

            try
            {
                //if (HttpContext.Session.GetString("IsSessionActive") == null)
                //{
                //    return RedirectToAction("LogIn", "Account");
                //}

                bOLAuditing.CommonObjects.TopHeading = "Atlas Auditing";
                bOLAuditing.CommonObjects.Heading = "Auditing & Analysis ";
                bOLAuditing.Operators = Common.ChargeCodeOperators.Select((r, index) => new SelectListItem { Text = r, Value = r });
                bOLAuditing.CompanyId = Arg.DataAccess.ActiveClient.Info.CompanyId;

                if (bolModel.SearchOptionsCeva != null)
                {
                    Common.CheckActiveClient();
                    bolModel.SearchOptionsCeva.CompanyId = bOLAuditing.CompanyId;
                    var q = Common.QueryResults.SaveQueryResults(bolModel.SearchOptionsCeva);
                    var type = "";
                    var clients = Common.ArgClients.GetArgClients(Common.CurrentUserId);

                    if (clients.Count <= 0)
                    {
                        type = "Message";
                    }
                    else if (ButtonType == "Stats")
                    {
                        type = "Stats";
                    }
                    else if (ButtonType == "Playbook")
                    {
                        type = "Playbook";
                    }
                    else
                    {
                        type = "Results";
                    }
                    return Json(new AjaxResult { QueryId = q.QueryId, Type = type, ResultTableFormat = bolModel.SearchOptionsCeva.ResultTableFormat });
                }
                else
                {
                    var companies = Common.ArgClients.GetBOLClients(Common.CurrentUserId);
                    bOLAuditing.Companies = new SelectList(companies, "CompanyId", "Name");

                    if (bOLAuditing.CompanyId > 0)
                    {
                        var users = Common.AspNetUsers.GetAspNetUsers(true);
                        bOLAuditing.Users = new SelectList(users.OrderBy(x => x.UserName), "Id", "UserName");

                        var Regions = Common.BookingHeaders.GetRegions();
                        bOLAuditing.Regions = new SelectList(Regions, "Region", "Region");

                        var mode = Common.Mode.OrderBy(i => i.Text).ToList();
                        bOLAuditing.Mode = new SelectList(mode, "Value", "Text");

                        var Branch = Common.BookingHeaders.GetBookingBranch();
                        bOLAuditing.Branch = new SelectList(Branch, "Branch", "Branch");

                        var OriginCountryCode = Common.BookingHeaders.GetOriginCountryCode();
                        bOLAuditing.OriginCountryCode = new SelectList(OriginCountryCode, "OriginCountryCode", "OriginCountryName");

                        var pol = Common.BookingHeaders.GetDistinctPOL();
                        bOLAuditing.PortOfLoading = new SelectList(pol, "POLCode", "POL");

                        var pod = Common.BookingHeaders.GetDistinctPOD();
                        bOLAuditing.PortOfDischarge = new SelectList(pod, "PODCode", "POD");

                        var DestinationCountryCode = Common.BookingHeaders.GetDestinationCountryCode();
                        bOLAuditing.DestinationCountryCode = new SelectList(DestinationCountryCode, "DestinationCountryCode", "DestinationCountryName");

                        var OceanServiceType = Common.BookingHeaders.GetOceanServiceType();
                        bOLAuditing.OceanServiceType = new SelectList(OceanServiceType, "ONCSERV", "ONCSERV");

                        var customer = Common.BookingHeaders.GetCustomer();
                        Arg.Ceva.DataAccess.BookingHeader.BookingHeaderImp nullcust = new Arg.Ceva.DataAccess.BookingHeader.BookingHeaderImp();

                        nullcust.DEBTOR = "null";
                        nullcust.Customer = "No Invoice (null)";

                        customer.Insert(0, nullcust);
                        bOLAuditing.Customer = new SelectList(customer, "DEBTOR", "Customer");

                        var shipper = Common.BookingHeaders.GetShipper();
                        bOLAuditing.Shipper = new SelectList(shipper, "SHPRNO", "Consignee");

                        var consgn = Common.BookingHeaders.GetDistinctConsignee();
                        bOLAuditing.Consignee = new SelectList(consgn, "CSEENO", "Consignee");

                        var packageType = Common.BookingHeaders.GetPackageType();
                        bOLAuditing.PackageType = new SelectList(packageType, "PCKGSCODE", "PackageType");

                        var oceanCarrier = Common.BookingHeaders.GetOceanCarrier();
                        bOLAuditing.OceanCarrier = new SelectList(oceanCarrier, "CARRCODE", "OceanCompanyName");

                        var airCarrier = Common.BookingHeaders.GetAirCarrier();
                        bOLAuditing.AirCarrier = new SelectList(airCarrier, "CARR", "AirCompanyName");

                        var airServiceLevels = Common.XrefAirServiceLevels.GetXrefAirServiceLevels();
                        bOLAuditing.AirServiceLevels = new SelectList(airServiceLevels, "SERVLEVEL", "AirServiceLevels");

                        var airServiceLeveldetail = Common.XrefAirServiceLevelsDetails.GetXrefAirServiceLevelDetail();
                        bOLAuditing.AirServiceLeveldetail = new SelectList(airServiceLeveldetail, "SERVLVL", "AirServiceLevelDetail");

                        var airServiceType = Common.BookingHeaders.GetAirServiceType();
                        bOLAuditing.AirServiceType = new SelectList(airServiceType, "SERVTYPE", "SERVTYPE");

                        var containerDetails = Common.ContainerDetails.GetContainerDetails();
                        bOLAuditing.ContainerDetails = new SelectList(containerDetails, "CNTRTYPE", "CNTRTYPE");

                        var bookingType = Common.BookingHeaders.GetBookingType();
                        bOLAuditing.Booking = new SelectList(bookingType, "TYPE", "BookingType");

                        var ImageType = Common.DocImages.GetAllDocumentImage();
                        bOLAuditing.ImagedTypes = new SelectList(ImageType, "Type", "Type");

                    }
                    string[,] sortCols = { { "Customer", "Customer" }, { "Shipper name", "ParticipantName" }, { "Origin port", "b.MATRCLOCA" }, { "Destination port", "b.MATRDLOCA" }, { "Origin country", "b.CNTRYCODE" }, { "Departure date", "b.MATRCDATE" } };

                    bOLAuditing.SearchOptionsCeva = new Arg.Ceva.DataAccess.SearchOptions
                    {
                        SortOptions = new List<Arg.Ceva.DataAccess.SortOptionsObj>()
                    };
                    var sortOptions = new List<Arg.Ceva.DataAccess.SortOptionsObj>();

                    if (sortCols != null)
                    {
                        var count = (sortCols.Length / 2) - 1;
                        for (var i = 0; i <= count; i++)
                        {
                            //sortOptionsObj = data.SearchOptions.SortOptions.Select(sortOpt => new SortOptionsObj { ColumnName = col, Idx = 0, IsDesc = false, IsSelected = true }).ToList();
                            var si = new Arg.Ceva.DataAccess.SortOptionsObj
                            {
                                DisplayName = sortCols[i, 0],
                                ColumnName = sortCols[i, 1],
                                Idx = 0,
                                IsDesc = false,
                                IsSelected = true
                            };
                            sortOptions.Add(si);
                        }

                        if ((CustId != null) || (orig != null) || (dest != null) || queryId != null)
                        {
                            var _queryId = Convert.ToInt32(queryId);
                            var qr = new Arg.DataAccess.QueryResultsImpl().GetQueryResults(_queryId);
                            if (qr != null)
                            {
                                var searchOptions = Newtonsoft.Json.JsonConvert.DeserializeObject<Arg.Ceva.DataAccess.SearchOptions>(qr.QueryJson);
                                bOLAuditing.SearchOptionsCeva = searchOptions;
                                if (CustId != null)
                                {
                                    //if (CustId.Contains("null") || CustId.Contains(""))
                                    //    data.SearchOptionsCeva.CSORNO = new string {"null"};
                                    //else
                                    bOLAuditing.SearchOptionsCeva.CSORNO = CustId;
                                }
                                if (orig != null)
                                {
                                    bOLAuditing.SearchOptionsCeva.POL = orig;
                                }
                                  
                                if (dest != null)
                                {
                                    bOLAuditing.SearchOptionsCeva.POD = dest;
                                }
                                    
                                bOLAuditing.SearchOptionsCeva.EstimatedStartDate = searchOptions.EstimatedStartDate;
                                bOLAuditing.SearchOptionsCeva.EstimatedEndDate = searchOptions.EstimatedEndDate;
                            }

                            bOLAuditing.SearchOptionsCeva.SortOptions = new List<Arg.Ceva.DataAccess.SortOptionsObj>();
                        }


                        if (sortOptions != null)
                        {
                            bOLAuditing.SearchOptionsCeva.SortOptions = sortOptions;
                        }
                    }
                    return View(bOLAuditing);
                }
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
                bOLAuditing.Message = ex.ToString();
            }
            return View(bOLAuditing);
        }

        [Authorize]
        public IActionResult MsgViewer(string msgFileFullPath)
        {
            var msgReader = new MsgReader.Reader();


            if (!System.IO.File.Exists(msgFileFullPath))
            {
                ViewBag.Content = "File not found: " + msgFileFullPath;
                return View();
            }

            using (var stream = System.IO.File.OpenRead(msgFileFullPath))
            {
                try
                {
                    var content = msgReader.ExtractMsgEmailBody(stream, MsgReader.ReaderHyperLinks.None, "text/html; charset=utf-8");
                    ViewBag.Content = content;
                }
                catch (Exception ex)
                {
                    Trace.TraceError(ex.ToString());
                    var url = msgFileFullPath.Replace(Arg.Core.Settings.FilesServerDrive, Arg.Core.Settings.FilePath);
                    //url = url.Replace(@"\",@"/");
                    ViewBag.Content = "MsgReader format is not valid to open, please <a class='btn btn-primary' target='_blank' href='" + url + "'>click here</a> to download";
                }
            }
            return View();
        }

        public IActionResult AuditingResults(int? queryId, int? idx, string bolNo, int? companyId, string customerId, string region, DateTime? bolExecDate, List<string> CustId, List<string> orig, List<string> dest, List<string> POL, string stats)
        {
            var result = new BookingAuditingResult();

            try
            {
                if (queryId > 0 && !Common.IsUserLoggedIn())
                {
                    Common.GoToLogin();
                }
                var _companyId = 0;
                if (companyId > 0)
                {
                    _companyId = Convert.ToInt32(companyId);
                    Arg.DataAccess.ActiveClient.Set(_companyId);
                }
                else
                {
                    _companyId = Arg.DataAccess.ActiveClient.Info.CompanyId;
                }

                Arg.Ceva.DataAccess.BookingHeader.BookingHeaderImp bolAuditingResults = null;
                var _queryId = Convert.ToInt32(queryId);
                var _idx = Convert.ToInt32(idx);

                if (((CustId != null) || (orig != null) || (dest != null) || (POL != null)) && (stats == "stats"))
                {
                    queryId = SaveQueryJson(CustId, orig, dest, POL, _queryId);
                    _queryId = Convert.ToInt32(queryId);
                }

                var note = @"BOL#: <a href='" + Common.MyRoot + "/Booking/AuditingResults?queryId=" + queryId + "' target='_blank'>" + bolNo + "</a>";
                result.ShowNavigation = true;

                if (queryId != null && queryId > 0)
                {
                    var qr = Common.QueryResults.GetQueryResults(_queryId);
                    result.QueryId = _queryId;
                    var avc = Thread.CurrentThread.CurrentCulture;

                    if (qr.SqlQuery == null)
                    {
                        bolAuditingResults = Common.BookingHeaders.GetResult(_queryId, _idx);
                    }
                    else
                    {
                        bolAuditingResults = Common.BookingHeaders.GetResults(qr.SqlQuery, _idx);
                    }

                    if (bolAuditingResults == null || bolAuditingResults.ResultCount == 0)
                    {
                        result.Message = "No results found related to  your search!";
                        return View(result);
                    }
                    else
                    {
                        result.TotalResultCount = bolAuditingResults.ResultCount;
                        result.BOLAuditResults = bolAuditingResults;//.Take(50).ToList();
                    }

                    result.Idx = _idx;
                    result.ShowNavigation = true;

                    //if (HttpContext.Session.GetString("IsSessionActive") == null)
                    //{
                    //    return RedirectToAction("LogIn", "Account");
                    //}
                }

                if (!string.IsNullOrWhiteSpace(bolNo))
                {
                    result.BOLAuditResults = new Arg.Ceva.DataAccess.BookingHeader.BookingHeaderImp();
                    result.BOLAuditResults = Common.BookingHeaders.GetBookingHeader(bolNo);
                }
                else
                {
                    bolNo = (string.IsNullOrWhiteSpace(result.BOLAuditResults.HBLNO) ? result.BOLAuditResults.HAWBNO : result.BOLAuditResults.HBLNO);
                }

                if (Convert.ToDecimal(result.BOLAuditResults.CHRGWGHT) > 0)
                {
                    var invoiceGrossRate = Common.InvoiceCharges.GetGrossRate(!string.IsNullOrWhiteSpace(result.BOLAuditResults.HBLNO) ? result.BOLAuditResults.HBLNO : "", result.BOLAuditResults.HAWBNO);
                    result.BOLAuditResults.GrossRate = invoiceGrossRate.GrossRate / Convert.ToDecimal(result.BOLAuditResults.CHRGWGHT);

                    var invoiceNetRate = Common.InvoiceCharges.GetNetRate(!string.IsNullOrWhiteSpace(result.BOLAuditResults.HBLNO) ? result.BOLAuditResults.HBLNO : "", result.BOLAuditResults.HAWBNO);
                    result.BOLAuditResults.NetRate = invoiceNetRate.NetRate / Convert.ToDecimal(result.BOLAuditResults.CHRGWGHT);
                }
                if (!string.IsNullOrWhiteSpace(result.BOLAuditResults.TYPE))
                {
                    var BookingTypeDescription = Common.XrefBookingTypes.GetBookingType(result.BOLAuditResults.TYPE);
                    if (BookingTypeDescription != null)
                    {
                        result.BOLAuditResults.BookingTypeDescription = BookingTypeDescription.Description ?? result.BOLAuditResults.TYPE;
                    }
                    else
                    {
                        result.BOLAuditResults.BookingTypeDescription = result.BOLAuditResults.TYPE;
                    }
                }
                if (!string.IsNullOrWhiteSpace(result.BOLAuditResults.SHPRNO))
                {
                    var ShipperDetail = Common.Participant.GetShipper(result.BOLAuditResults.SHPRNO);
                    if (ShipperDetail != null)
                    {
                        result.BOLAuditResults.Shipper = ShipperDetail.Shipper ?? result.BOLAuditResults.SHPRNO;
                    }
                    else
                    {
                        result.BOLAuditResults.Shipper = result.BOLAuditResults.SHPRNO;
                    }
                }

                if (!string.IsNullOrWhiteSpace(result.BOLAuditResults.CSORNO))
                {
                    var PayorDetail = Common.Participant.GetPayor(result.BOLAuditResults.CSORNO);
                    if (PayorDetail != null)
                    {
                        result.BOLAuditResults.Payor = PayorDetail.Payor ?? result.BOLAuditResults.CSORNO;
                    }
                    else
                    {
                        result.BOLAuditResults.Payor = result.BOLAuditResults.CSORNO;
                    }
                }

                if (!string.IsNullOrWhiteSpace(result.BOLAuditResults.CSEENO))
                {
                    var ConsigneeDetail = Common.Participant.GetConsignee(result.BOLAuditResults.CSEENO);
                    if (ConsigneeDetail != null)
                    {
                        result.BOLAuditResults.PartConsignee = ConsigneeDetail.Consignee ?? result.BOLAuditResults.CSEENO;
                    }
                    else
                    {
                        result.BOLAuditResults.PartConsignee = result.BOLAuditResults.CSEENO;
                    }
                }

                if (!string.IsNullOrWhiteSpace(result.BOLAuditResults.GOODSTYPE))
                {
                    var goodsType = Common.XrefGoodsTypes.GetGoodsType(result.BOLAuditResults.GOODSTYPE);
                    if (goodsType != null)
                    {
                        result.BOLAuditResults.GoodType = goodsType.Description ?? result.BOLAuditResults.GOODSTYPE;
                    }
                    else
                    {
                        result.BOLAuditResults.GoodType = result.BOLAuditResults.GOODSTYPE;
                    }
                }

                if (!string.IsNullOrWhiteSpace(result.BOLAuditResults.PCKGSCODE))
                {
                    var PackagingCode = Common.XrefPackagingCodes.GetPackagingCode(result.BOLAuditResults.PCKGSCODE);
                    if (PackagingCode != null)
                    {
                        result.BOLAuditResults.PackagingCode = PackagingCode.DESCRIPTION ?? PackagingCode.DESCRIPTION;
                    }
                    else
                    {
                        result.BOLAuditResults.PackagingCode = result.BOLAuditResults.PCKGSCODE;
                    }
                }

                if (!string.IsNullOrWhiteSpace(result.BOLAuditResults.CARR))
                {
                    var IATACodeDescription = Common.XrefAirCarriers.GetIATACode(result.BOLAuditResults.CARR);
                    if (IATACodeDescription != null)
                    {
                        result.BOLAuditResults.IATACodeDescription = IATACodeDescription.AirCompanyName ?? result.BOLAuditResults.CARR;
                    }
                    else
                    {
                        result.BOLAuditResults.IATACodeDescription = result.BOLAuditResults.CARR;
                    }
                }

                if (!string.IsNullOrWhiteSpace(result.BOLAuditResults.CARRCODE))
                {
                    var SCACCDescription = Common.XrefOceanCarriers.GetOceanCarriersCompanyName(result.BOLAuditResults.CARRCODE);
                    if (SCACCDescription != null)
                    {
                        result.BOLAuditResults.SCACCompanyName = SCACCDescription.SCACCompanyName ?? result.BOLAuditResults.CARRCODE;
                    }
                    else
                    {
                        result.BOLAuditResults.SCACCompanyName = result.BOLAuditResults.CARRCODE;
                    }
                }

                if (!string.IsNullOrWhiteSpace(result.BOLAuditResults.SERVLEVEL))
                {
                    var SERVLEVELDescription = Common.XrefAirServiceLevels.GetSERVLEVELDescription(result.BOLAuditResults.SERVLEVEL);
                    if (SERVLEVELDescription != null)
                    {
                        result.BOLAuditResults.SERVLEVELDescription = SERVLEVELDescription.AirServiceLevels ?? result.BOLAuditResults.SERVLEVEL;
                    }
                    else
                    {
                        result.BOLAuditResults.SERVLEVELDescription = result.BOLAuditResults.SERVLEVEL;
                    }
                }

                if (!string.IsNullOrWhiteSpace(result.BOLAuditResults.SERVLVL))
                {
                    var SERVLVLDescription = Common.XrefAirServiceLevelsDetails.GetSERVLVLDescription(result.BOLAuditResults.SERVLVL);
                    if (SERVLVLDescription != null)
                    {
                        result.BOLAuditResults.SERVLVLDescription = SERVLVLDescription.AirServiceLevelDetail ?? result.BOLAuditResults.SERVLVL;
                    }
                    else
                    {
                        result.BOLAuditResults.SERVLVLDescription = result.BOLAuditResults.SERVLVL;
                    }
                }

                if (!string.IsNullOrWhiteSpace(result.BOLAuditResults.PCCLOCA))
                {
                    var LocationName = Common.Location.GetLocation(result.BOLAuditResults.PCCLOCA);
                    if (LocationName != null)
                    {
                        result.BOLAuditResults.PickLocationName = LocationName.LocationName ?? result.BOLAuditResults.PCCLOCA;
                    }
                    else
                    {
                        result.BOLAuditResults.PickLocationName = result.BOLAuditResults.PCCLOCA;
                    }
                }

                if (!string.IsNullOrWhiteSpace(result.BOLAuditResults.ONCDLOCA))
                {
                    var LocationName = Common.Location.GetLocation(result.BOLAuditResults.ONCDLOCA);
                    if (LocationName != null)
                    {
                        result.BOLAuditResults.DelLocationName = LocationName.LocationName ?? result.BOLAuditResults.ONCDLOCA;
                    }
                    else
                    {
                        result.BOLAuditResults.DelLocationName = result.BOLAuditResults.ONCDLOCA;
                    }
                }

                if (!string.IsNullOrWhiteSpace(result.BOLAuditResults.CNTRYCODE))
                {
                    var CountryName = Common.XrefCountries.GetCountryName(result.BOLAuditResults.CNTRYCODE);
                    if (CountryName != null)
                    {
                        result.BOLAuditResults.CountryName = CountryName.Name ?? result.BOLAuditResults.CNTRYCODE;
                    }
                    else
                    {
                        result.BOLAuditResults.CountryName = result.BOLAuditResults.CNTRYCODE;
                    }
                }

                if (!string.IsNullOrWhiteSpace(result.BOLAuditResults.CNTRYCOD01))
                {
                    var CountryName = Common.XrefCountries.GetCountryName(result.BOLAuditResults.CNTRYCOD01);
                    if (CountryName != null)
                    {
                        result.BOLAuditResults.CountryName01 = CountryName.Name ?? result.BOLAuditResults.CNTRYCOD01;
                    }
                    else
                    {
                        result.BOLAuditResults.CountryName01 = CountryName.Name ?? result.BOLAuditResults.CNTRYCOD01;
                    }
                }

                if (!string.IsNullOrWhiteSpace(result.BOLAuditResults.MATRCLOCA))
                {
                    var pOLDetail = Common.Location.GetPOL(result.BOLAuditResults.MATRCLOCA);
                    if (pOLDetail != null)
                    {
                        result.BOLAuditResults.POLDetail = pOLDetail.POLDetail ?? result.BOLAuditResults.MATRCLOCA;
                    }
                    else
                    {
                        result.BOLAuditResults.POLDetail = result.BOLAuditResults.MATRCLOCA;
                    }
                }
                if (!string.IsNullOrWhiteSpace(result.BOLAuditResults.MATRDLOCA))
                {
                    var pODDetail = Common.Location.GetPOD(result.BOLAuditResults.MATRDLOCA);
                    if (pODDetail != null)
                    {
                        result.BOLAuditResults.PODDetail = pODDetail.PODDetail ?? result.BOLAuditResults.MATRDLOCA;
                    }
                    else
                    {
                        result.BOLAuditResults.PODDetail = result.BOLAuditResults.MATRDLOCA;
                    }
                }

                if (!string.IsNullOrWhiteSpace(result.BOLAuditResults.HBLNO) || !string.IsNullOrWhiteSpace(result.BOLAuditResults.HAWBNO))
                {
                    var pdfFile = Common.DocImages.GetDocumentImage(string.IsNullOrWhiteSpace(result.BOLAuditResults.HBLNO) ? result.BOLAuditResults.HAWBNO : result.BOLAuditResults.HBLNO);
                    if (pdfFile != null && pdfFile.Any())
                    {
                        result.PDFFile = pdfFile;
                        result.FileNames = JavaScript.Serialize(result.PDFFile.Select(x => x.fileName));
                    }
                }

                if (!string.IsNullOrWhiteSpace(result.BOLAuditResults.BOKPRT))

                {
                    var invoiceHeader = Common.InvoiceCharges.GetInvoicedChargesDetail(result.BOLAuditResults.BOKPRT);
                    //var invoiceHeader = Common.InvoiceCharges.GetInvoicedChargesDetail(string.IsNullOrWhiteSpace(data.BOLAuditResults.HBLNO) ? data.BOLAuditResults.HAWBNO : data.BOLAuditResults.HBLNO);
                    if (invoiceHeader != null)
                    {
                        result.BOLInvoiceHeader = invoiceHeader;
                    }

                    result.TotalBOLInvoiceHeader = Convert.ToDecimal(result.BOLInvoiceHeader.Sum(i => i.INVCURAMT));
                    result.BOLNetInvoiceHeader = result.BOLInvoiceHeader.GroupBy(x => new { x.DEBTOR, x.INVOICENO }).Select(g => new Arg.Ceva.DataAccess.InvoiceCharges.InvoiceCharge
                    {
                        DEBTOR = g.Key.DEBTOR,
                        INVOICENO = g.Key.INVOICENO,
                        INVCURAMT = g.Sum(ri => ri.INVCURAMT)
                    }).ToList();
                }

                if (!string.IsNullOrWhiteSpace(result.BOLAuditResults.HBLNO))
                {
                    var containerDetails = Common.ContainerDetails.GetContainerDetail(result.BOLAuditResults.HBLNO);
                    if (containerDetails != null)
                    {
                        result.BOLContainerDetail = containerDetails;
                    }
                }

                if (!string.IsNullOrWhiteSpace(bolNo))
                {
                    var balDue = Common.BalanceDues.GetARGBalanceDue(bolNo, _companyId, customerId, region, Convert.ToDateTime(bolExecDate));
                    //data.BOLAuditResults = new Arg.Ceva.DataAccess.BookingHeader.BookingHeaderImp();
                    //data.BOLAuditResults.BOLNo = bolNo;
                    var bdPaymentAmount = Common.BalanceDues.GetBDPaymentAmount(bolNo, _companyId);
                    result.BDPaymentAmount = bdPaymentAmount;
                    //if (balDue != null && balDue.BalanceId > 0)
                    if (balDue != null)

                    {
                        result.BalanceDueDetails = balDue;
                        //    note = @"BOL#: <a href='" + Common.MyRoot + "/Booking/AuditingResults?bolNo=" + bolNo +
                        //"&companyId=" + _companyId + "&region=" + balDue.Region + "&customerId=" + balDue.CustomerId +
                        //"&bolExecDate=" + balDue.BolExecutionDate.ToString("yyyy-MM-dd") + "' target='_blank'>" + bolNo + "</a>";
                    }
                    note = @"BOL#: <a href='" + Common.MyRoot + "/booking/AuditingResults?bolNo=" + bolNo +
                           "&companyId=" + _companyId + "' target='_blank'>" + bolNo + "</a>";
                }

                result.BolNo = bolNo;
                result.EnablePullForResearchBtn = false;
                var resItems = Common.ResearchItems.GetAuditingResearchItemsCeva("", _companyId, bolNo);
                if (resItems.Count <= 0)
                {
                    result.EnablePullForResearchBtn = true;
                }

                Common.ActivityStats.SaveActivityStats(Arg.DataAccess.ActivityStatsImpl.EnumActions.BOLViewed, _companyId, note, bolNo);

                var actStatsByEvent = Common.ActivityStats.GetActivityStatsByEventType(Arg.DataAccess.ActivityStatsImpl.EnumActions.BOLViewed.ToString(), bolNo, _companyId, Common.CurrentUserId);
                if (actStatsByEvent != null && actStatsByEvent.Any())
                {
                    result.ActStatsByEvent = actStatsByEvent;
                }
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }
            return View(result);

        }

        public IActionResult ViewAuditingResultTableFormat(int? queryId, int? idx, int? pageNo, List<string> CustId, List<string> orig, List<string> dest, List<string> POL, string stats)
        {
            var data = new BookingAuditingResult();
            try
            {
                int pageSize = Common.PageSize;
                int pageIndex = 1;
                data.BolAuditResultTableFormat = new List<Arg.Ceva.DataAccess.BookingHeader.BookingHeaderImp>().ToPagedList(pageIndex, pageSize);
                //List<Arg.DataModels.BOLHeader> bolAuditingtableformatresult = null;
                var bolAuditingtableformatresult = new List<Arg.Ceva.DataAccess.BookingHeader.BookingHeaderImp>();
                bolAuditingtableformatresult.ToPagedList(pageIndex, pageSize);
                pageIndex = pageNo.HasValue ? Convert.ToInt32(pageNo) : 1;
                var searchOptions = new Arg.Ceva.DataAccess.SearchOptions();

                var _queryId = Convert.ToInt32(queryId);
                var _idx = Convert.ToInt32(idx);
                if (((CustId != null) || (orig != null) || (dest != null) || (POL != null)) && (stats == "stats"))
                {
                    queryId = SaveQueryJson(CustId, orig, dest, POL, _queryId);
                    _queryId = Convert.ToInt32(queryId);
                }
                var qr = new Arg.DataAccess.QueryResultsImpl().GetQueryResults(_queryId);
                if (qr.SqlQuery == null)
                {
                    searchOptions = Newtonsoft.Json.JsonConvert.DeserializeObject<Arg.Ceva.DataAccess.SearchOptions>(qr.QueryJson);
                    bolAuditingtableformatresult = Common.BookingHeaders.GetResults(_queryId, "table");
                    //data.InvoiceBillType = searchOptions.BillType;
                }
                else
                {
                    bolAuditingtableformatresult = Common.BookingHeaders.GetResults(_queryId, "table");
                }

                if (bolAuditingtableformatresult == null || !bolAuditingtableformatresult.Any())
                {
                    data.Message = "No results found related to  your search!";
                    return View(data);
                }
                else
                {
                    data.CompanyId = Arg.DataAccess.ActiveClient.Info.CompanyId;
                    data.BolAuditResultTableFormat = bolAuditingtableformatresult.ToPagedList(pageIndex, pageSize);
                    //data.BolAuditResultTableFormat = bolAuditingtableformatresult;

                    data.SearchOptions = searchOptions;
                }
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex.ToString());
            }
            return View(data);
        }

        public JsonResult BOLViewed(string bolNo, int companyId)
        {
            try
            {
                if (companyId <= 0)
                {
                    companyId = Arg.DataAccess.ActiveClient.Info.CompanyId;
                }
                   
                var note = @"BOL#: <a href='" + Common.MyRoot + "/Booking/AuditingResults?bolNo=" + bolNo +
                    "&companyId=" + companyId + "' target='_blank'>" + bolNo + "</a>";

                Common.ActivityStats.SaveActivityStats(Arg.DataAccess.ActivityStatsImpl.EnumActions.BOLViewed, companyId, note, bolNo);
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex.ToString());
                return Json(ex.ToString());
            }
            return Json("Success");
        }

        public int SaveQueryJson(List<string> CustId, List<string> orig, List<string> dest, List<string> POL, int queryId)
        {
            int _queryId = 0;
            var bOLAuditing = new BOLAuditing();

            try
            {
                if ((CustId != null) || (orig != null) || (dest != null) || POL != null)
                {
                    bOLAuditing.SearchOptionsCeva = new Arg.Ceva.DataAccess.SearchOptions();
                    var qr = Common.QueryResults.GetQueryResults(queryId);
                    bOLAuditing.SearchOptionsCeva = Newtonsoft.Json.JsonConvert.DeserializeObject<Arg.Ceva.DataAccess.SearchOptions>(qr.QueryJson);
                    if (CustId != null)
                    {
                        bOLAuditing.SearchOptionsCeva.CSORNO = CustId;
                    }
                       
                    if (orig != null)
                    {
                        bOLAuditing.SearchOptionsCeva.POL = orig;
                    }
                       
                    if (dest != null)
                    {
                        bOLAuditing.SearchOptionsCeva.POD = dest;
                    }
                       
                    bOLAuditing.CompanyId = Arg.DataAccess.ActiveClient.Info.CompanyId;
                    bOLAuditing.SearchOptionsCeva.CompanyId = bOLAuditing.CompanyId;
                    var q = Common.QueryResults.SaveQueryResults(bOLAuditing.SearchOptionsCeva);
                    if (q.QueryId > 0)
                    {
                        _queryId = q.QueryId;
                    } 
                    return _queryId;
                }
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex.ToString());
            }

            return _queryId;
        }

        public IActionResult PullForResearch(string bolNo)
        {
            var pullForResearch = new PullForResearch();
            try
            {
                var bolDetails = Common.BookingHeaders.GetBookingHeader(bolNo);
                if (bolDetails != null)
                {
                    pullForResearch.BookingDetails = bolDetails;
                }
                var reasonCodes = Common.RSReasonCodes.GetDistinctReasonCodes();
                pullForResearch.ReasonCodes = new SelectList(reasonCodes, "ReasonCode", "ReasonCode");
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }
            return View(pullForResearch);
        }

        [Authorize]
        [HttpPost]
        public JsonResult SaveResearchDetails(PullForResearch pullForResearch)
        {
            try
            {
                //var balDue = Common.BalanceDues.GetBalanceDue(0, pullForResearch.BOLNo);
                var bolHeaderDetails = Common.BookingHeaders.GetBookingHeader(pullForResearch.BOLNo) ?? new Arg.Ceva.DataAccess.BookingHeader.BookingHeaderImp();
                //var region = "";
                //var regionInfo = ARG.Common.Regions.GetRegion(0, ARG.Common.GetActiveClientId());
                //if (regionInfo != null && regionInfo.RegionId > 0)
                //    region = regionInfo.Region;
                var resItem = new Arg.DataModels.ResearchItems
                {
                    Region = bolHeaderDetails.Region,
                    BOL = pullForResearch.BOLNo,
                    ResearchReasonCode = pullForResearch.PullReasonCode,
                    Status = "Open",
                    Comments = pullForResearch.ResearchComments,
                    LastModified = DateTime.Now,
                    CompanyId = Arg.DataAccess.ActiveClient.Info.CompanyId,
                    //BookingId = bolHeaderDetails.BookingID,
                    BookingId = "0",
                    BolExecutionDate = Convert.ToDateTime(bolHeaderDetails.MATRCDATE),
                    RevenueAnalystAuditor = Common.CurrentUserId,
                    LastModifiedBy = Common.CurrentUserId
                };
                Common.ResearchItems.SaveResearchItem(resItem);
                if (resItem.ResearchId > 0)
                {
                    Common.ActivityStats.SaveActivityStats(Arg.DataAccess.ActivityStatsImpl.EnumActions.Saved, resItem.CompanyId, "Research Items; BOL#: " + resItem.BOL + " ; Client: " + Arg.DataAccess.ActiveClient.Info.Name);
                }
                return Json("Research Item saved!");
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
                return Json(ex.ToString());
            }
        }

        [CheckActiveClient]
        public IActionResult AddBalanceDue(string bolNo, int balanceId = 0, bool IsNewBal = false)
        {
            var data = new AddBalanceDue();

            try
            {
                data.Message = Arg.Core.ActiveObjects.BolIsBeingEditedInfo(bolNo);
                Arg.Core.ActiveObjects.SetBolBeingEdited(bolNo, "", Common.CurrentUserName);
                data.BolNo = bolNo;
                data.CommonObjects.TopHeading = "Add Balance Due";
                var companyId = Arg.DataAccess.ActiveClient.Info.CompanyId;

                // Crrecncies
                var currencies = Common.InvoiceCharges.GetDistinctCurrency();
                data.Currencies = new SelectList(currencies, "CURR", "CURR");

                //BolCharges
                var bolChargeInfo = Common.InvoiceCharges.GetBOLCharge(bolNo);
                if (bolChargeInfo != null && ((!string.IsNullOrWhiteSpace(bolChargeInfo.HOUSENO)) || (!string.IsNullOrWhiteSpace(bolChargeInfo.HAWBNO)) || (!string.IsNullOrWhiteSpace(bolChargeInfo.HBLNO))))
                {
                    data.Currency = bolChargeInfo.CURR;
                }

                //Customers
                var bolCustomers = Common.BookingHeaders.GetBOLCustomers();
                if (bolCustomers != null && bolCustomers.Any())
                {
                    data.CustList = bolCustomers;
                }

                if (IsNewBal)
                {
                    var cust = Common.BalanceDues.GetCustomerBalanceDues(bolNo, companyId);
                    data.CustList = data.CustList.Where(x => !cust.Any(i => i.CustomerId == x.StrId)).ToList();
                }

                data.BalanceDue = new BalanceDue();

                //Payment
                var payment = new BalanceDues_Payments();
                if (!IsNewBal)
                {
                    payment = Common.BalanceDuesPayments.GetBalanceDuesPayment(0, bolNo) ?? new BalanceDues_Payments();
                }


                data.AmountPaid = payment.PaymentAmount;
                data.PaymentId = payment.PaymentId;

                //BolHeader
                var bolHeaderDetails = Common.BookingHeaders.GetBookingHeader(bolNo) ?? new Arg.Ceva.DataAccess.BookingHeader.BookingHeaderImp();

                bolHeaderDetails.Bol = (string.IsNullOrWhiteSpace(bolHeaderDetails.HBLNO) ? bolHeaderDetails.HAWBNO : bolHeaderDetails.HBLNO);
                bolHeaderDetails.Voyage = (string.IsNullOrWhiteSpace(bolHeaderDetails.MAINFLGH) ? bolHeaderDetails.VOYNO : bolHeaderDetails.MAINFLGH);
                bolHeaderDetails.MODE = (string.IsNullOrWhiteSpace(bolHeaderDetails.ONCSERV) ? bolHeaderDetails.SERVTYPE : bolHeaderDetails.ONCSERV);
                data.BookingHeaderDetails = new Arg.Ceva.DataAccess.BookingHeader.BookingHeaderImp();

                //Error Codes
                var errCodes = Common.BDErrorCodes.GetDistinctErrorCodesCeva(companyId);

                //  BalanceDue
                var balDue = new BalanceDue();
                if (!IsNewBal)
                {
                    balDue = Common.BalanceDues.GetBalanceDue(balanceId, bolNo, companyId) ?? new BalanceDue();
                }

                if (balDue != null && !string.IsNullOrWhiteSpace(balDue.Bol))
                {
                    data.Description = balDue.BDDescription;
                    data.CustomerId = balDue.CustomerId;
                    bolHeaderDetails.Bol = balDue.Bol;
                    bolHeaderDetails.Voyage = balDue.Voyage;
                    bolHeaderDetails.VESSCODE = balDue.Vessel;
                    bolHeaderDetails.MODE = balDue.MoveType;
                    bolHeaderDetails.MATRCDATE = balDue.BolExecutionDate;
                    bolHeaderDetails.InvoiceType = balDue.InvoiceType;
                    data.ErrorCode = balDue.BDErrorCode;
                    bolHeaderDetails.MATRCLOCA = balDue.PortOfLoading;
                    bolHeaderDetails.MATRDLOCA = balDue.PortOfDischarge;
                    data.TotalCharges = balDue.BalanceDueAmount;
                    data.AmountDue = balDue.BalanceDueAmount;
                }

                if (string.IsNullOrWhiteSpace(data.CustomerId))
                {
                    data.CustomerId = Common.BookingHeaders.GetCustomersbyBOL(bolNo).StrId;
                }

                data.Quote = balDue.Quote;

                if (!string.IsNullOrWhiteSpace(balDue.BDErrorCode))
                {
                    var errorCode = errCodes.FirstOrDefault(x => x.BdErrorCode == balDue.BDErrorCode);
                    if (errorCode != null)
                    {
                        data.ErrorCode = errorCode.Description;
                    }     
                }

                data.BalanceDue.BalanceId = balDue.BalanceId;

                var bdDescs = Common.BalanceDuesDescriptions.GetBalanceDuesDesc(companyId);
                if (bdDescs != null && bdDescs.Any())
                {
                    data.BDDescriptions = new SelectList(bdDescs, "Description", "BDDescription");
                }
                else
                {
                    data.BDDescriptions = new SelectList("", "Description", "BDDescription");
                }

                var limit = 10000;
                if (bolHeaderDetails != null && !string.IsNullOrWhiteSpace(bolHeaderDetails.Bol))
                {
                    data.BookingHeaderDetails = bolHeaderDetails;
                    data.BookingHeaderDetails.BOLNo = bolNo;
                    //data.InvoiceSummary = Common.InvoiceSummary.GetInvoiceSummary(bolHeaderDetails.Bol).FirstOrDefault();
                    data.ModeCeva = Common.BookingHeaders.GetAllModes().Take(limit).ToList();
                    data.TypeCeva = Common.XrefPackagingCodes.GetAllTypes().Take(limit).ToList();
                    data.POD = Common.BookingHeaders.GetDistinctPOD().Take(limit).ToList();
                    data.POL = Common.BookingHeaders.GetDistinctPOL().Take(limit).ToList();
                    data.ChargeList = Common.BDOtherChargesCodes.GetDistinctChargeCodes().DistinctBy(x => x.ChargeCode).Take(limit).ToList();
                    data.BDErrorCodes = Common.BDErrorCodes.GetDistinctErrorCodesCeva(companyId).Take(limit).ToList();//todo based on CompanyId
                    data.ContainerSizesCeva = Common.ContainerDetails.GetDistinctSize();
                    data.BalanceDuesOtherCharges = new List<BalanceDues_OtherCharges>();
                    data.BalanceDuesOtherCharges.Add(new BalanceDues_OtherCharges { TariffRefNo = "" });
                    data.BalanceDuesItems = new List<BalanceDues_Item>();
                    data.BalanceDuesItems.Add(new BalanceDues_Item { TariffRef = "", GrossWeight = "0", Quantity = !string.IsNullOrWhiteSpace(Convert.ToString(data.BookingHeaderDetails.PCKGS)) ? (Convert.ToInt32(data.BookingHeaderDetails.PCKGS)) : 0, Type = data.BookingHeaderDetails.PCKGSCODE ?? "", AmountDue = 0.00M, CommodityDesc = data.BookingHeaderDetails.GOODSDESC + " " + data.BookingHeaderDetails.GOODSDSC2 ?? "", Weight = Convert.ToString(data.BookingHeaderDetails.CHRGWGHT) ?? "", Measure = Convert.ToString(data.BookingHeaderDetails.MEAS) ?? "" });
                    var balanceDueItem = new List<BalanceDues_Item>();

                    if (!IsNewBal)
                    {
                        balanceDueItem = Common.BOLHeader.GetBalanceDueItems(bolNo, companyId);
                        if (balanceDueItem.Count > 0)
                        {
                            data.BalanceDuesItems.AddRange(balanceDueItem);
                        }
                        else
                        {
                            var ContainerDetail = Common.ContainerDetails.GetBalanceDuesContainerDetail(bolNo);
                            if (ContainerDetail.Count > 0)
                            {
                                data.BalanceDuesItems.AddRange(ContainerDetail);
                            }
                            else
                            {
                                data.BalanceDuesItems.Add(new BalanceDues_Item { TariffRef = "", GrossWeight = "0", Quantity = !string.IsNullOrWhiteSpace(Convert.ToString(data.BookingHeaderDetails.PCKGS)) ? (Convert.ToInt32(data.BookingHeaderDetails.PCKGS)) : 0, Type = data.BookingHeaderDetails.PCKGSCODE ?? "", AmountDue = 0.00M, CommodityDesc = data.BookingHeaderDetails.GOODSDESC + " " + data.BookingHeaderDetails.GOODSDSC2 ?? "", Weight = Convert.ToString(data.BookingHeaderDetails.CHRGWGHT) ?? "", Measure = Convert.ToString(data.BookingHeaderDetails.MEAS) ?? "" });
                                //data.BalanceDuesItems.AddRange(Common.ContainerDetails.GetBalanceDuesContainerDetail(bolNo) ?? new List<Arg.DataModels.BalanceDues_Item>());
                            }
                        }
                    }
                    else
                    {
                        data.BalanceDuesItems.Add(new BalanceDues_Item { TariffRef = "", GrossWeight = "0", Quantity = !string.IsNullOrWhiteSpace(Convert.ToString(data.BookingHeaderDetails.PCKGS)) ? (Convert.ToInt32(data.BookingHeaderDetails.PCKGS)) : 0, Type = data.BookingHeaderDetails.PCKGSCODE ?? "", AmountDue = 0.00M, CommodityDesc = data.BookingHeaderDetails.GOODSDESC + " " + data.BookingHeaderDetails.GOODSDSC2 ?? "", Weight = Convert.ToString(data.BookingHeaderDetails.CHRGWGHT) ?? "", Measure = Convert.ToString(data.BookingHeaderDetails.MEAS) ?? "" });
                        //data.BalanceDuesItems.AddRange(Common.ContainerDetails.GetBalanceDuesContainerDetail(bolNo) ?? new List<Arg.DataModels.BalanceDues_Item>());
                    }

                    var baldueCharges = new List<BalanceDues_OtherCharges>();
                    if (!IsNewBal)
                    {
                        baldueCharges = (Common.BalanceDuesOtherCharges.GetBalanceDuesOtherCharges(bolNo) ?? new List<BalanceDues_OtherCharges>());
                        if (baldueCharges.Count > 0)
                        {
                            data.BalanceDuesOtherCharges.AddRange(baldueCharges);
                        }
                        else
                        {
                            data.BalanceDuesOtherCharges.AddRange(Common.InvoiceCharges.GetBalanceDuesInvoicedChargesDetail(bolNo) ?? new List<BalanceDues_OtherCharges>());
                        }
                    }
                    else
                    {
                        data.BalanceDuesOtherCharges.AddRange(Common.InvoiceCharges.GetBalanceDuesInvoicedChargesDetail(bolNo) ?? new List<BalanceDues_OtherCharges>());
                    }

                    if (bolHeaderDetails.InvoiceType == null)
                    {
                        if (!string.IsNullOrEmpty(data.BookingHeaderDetails.HBLNO))
                        {
                            data.BookingHeaderDetails.InvoiceType = "HBL";
                        }
                        else
                        {
                            data.BookingHeaderDetails.InvoiceType = "HAWB";
                        }
                    }  
                }
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }
            return View(data);
        }

        [Authorize]
        [HttpPost]
        public IActionResult AddBalanceDue(AddBalanceDue data)
        {
            try
            {
                if (Arg.DataAccess.ActiveClient.Info.CompanyId <= 0)
                {
                    return Json("Client not active. Please <a href='" + Common.MyRoot + "Account/Login' title='Login'>re-login</a>");
                }

                var companyId = Common.GetActiveClientId();
                var region = "";

                if (Common.ActiveClient.NameContains("Pasha"))
                {
                    region = "US";
                }
                else
                {
                    var regionInfo = Common.Regions.GetRegion(0, companyId);
                    if (regionInfo != null && regionInfo.RegionId > 0)
                    {
                        region = regionInfo.Region;
                    }
                }

                var invoiceDate = DateTime.Now.Date;//.ToString("MM/dd/yyyy");
                var errCodes = Common.BDErrorCodes.GetDistinctErrorCodesCeva(companyId);
                var bolExecDate = Convert.ToDateTime(data.BookingHeaderDetails.MATRCDATE);

                if (data.BalanceDuesItems != null && data.BalanceDuesItems.Any())
                {
                    List<Arg.DataModels.BalanceDues_Item> dues = data.BalanceDuesItems;

                    foreach (var due in dues)
                    {
                        due.Bol = data.BookingHeaderDetails.BOLNo;
                        due.BalanceDueInvoiceDate = Convert.ToDateTime(invoiceDate);
                        due.BookingId = "0";
                        due.CompanyId = companyId;
                        due.CustomerId = data.CustomerId;
                        due.Region = region;
                        due.ContainerType = "";
                        due.Container = !string.IsNullOrWhiteSpace(due.Container) ? due.Container : "";
                        due.CommodityDesc = !string.IsNullOrWhiteSpace(due.CommodityDesc) ? due.CommodityDesc : "";
                        due.Commodity = !string.IsNullOrWhiteSpace(due.Commodity) ? due.Commodity : "";
                        due.TariffRef = !string.IsNullOrWhiteSpace(due.TariffRef) ? due.TariffRef : "";
                        due.Measure = !string.IsNullOrWhiteSpace(due.Measure) ? due.Measure : "";
                        due.ContainerSize = !string.IsNullOrWhiteSpace(due.ContainerSize) ? due.ContainerSize : "";
                    }
                    Common.BOLHeader.SaveBalanceDueItems(dues);
                }

                if (data.BalanceDuesOtherCharges != null && data.BalanceDuesOtherCharges.Any())
                {
                    List<BalanceDues_OtherCharges> charges = data.BalanceDuesOtherCharges;
                    foreach (var charge in charges)
                    {
                        charge.BOLNo = data.BookingHeaderDetails.BOLNo;
                        charge.CustomerId = data.CustomerId;
                        charge.CompanyId = companyId;
                        charge.Region = region;
                        charge.Description = !string.IsNullOrWhiteSpace(charge.Description) ? charge.Description : "";
                        charge.TariffRefNo = !string.IsNullOrWhiteSpace(charge.TariffRefNo) ? charge.TariffRefNo : "";
                    }
                    Common.BalanceDuesOtherCharges.SaveBDOtherCharges(charges);
                }

                var balDue = new BalanceDue(); //MUST LOAD FROM DATABASE FIRST, FOR UPDATE EXISTING
                if (data.BalanceDue.BalanceId > 0)
                {
                    balDue = Common.BalanceDues.GetBalanceDue(data.BalanceDue.BalanceId, "", companyId) ?? new BalanceDue();
                }
                else
                {
                    balDue.CloseReasonCode = "";
                    balDue.DateAdded = DateTime.Now;
                    balDue.Comments = "";
                }

                balDue.Region = region;
                balDue.CustomerId = data.CustomerId;
                balDue.CompanyId = companyId;
                balDue.Bol = data.BookingHeaderDetails.BOLNo;
                balDue.BookingId = "0";
                balDue.BolExecutionDate = bolExecDate;
                balDue.CustomerLocationCode = Common.ActiveClient.NameContains("Pasha") ? "" : data.BookingHeaderDetails.MATRCLOCA;

                if (balDue.BalanceId <= 0 || balDue.BalanceDueInvoiceDate < DateTime.Now.AddYears(-10))
                {
                    balDue.BalanceDueInvoiceDate = null;
                }

                if (balDue.BalanceId <= 0)
                {
                    balDue.BalanceDueInvoice = "";
                }

                balDue.CustomerId = data.CustomerId;
                balDue.PortOfLoading = data.BookingHeaderDetails.MATRCLOCA;
                balDue.PortOfDischarge = data.BookingHeaderDetails.MATRDLOCA;
                balDue.MoveType = data.BookingHeaderDetails.MODE ?? "";//check
                balDue.BalanceDueAmount = data.AmountDue;
                balDue.BDErrorCode = errCodes.FirstOrDefault(x => x.Description == data.ErrorCode).BdErrorCode;
                balDue.BDDescription = data.Description;

                if (string.IsNullOrWhiteSpace(balDue.CollectionStatus))
                {
                    balDue.CollectionStatus = "Closed";
                }

                if (string.IsNullOrWhiteSpace(balDue.ClientGlStatus))
                {
                    balDue.ClientGlStatus = "Closed";
                }

                if (string.IsNullOrWhiteSpace(balDue.InvoiceStatus))
                {
                    balDue.InvoiceStatus = "Pending";
                }

                balDue.RevenueAnalystAuditor = Common.CurrentUserId;
                balDue.LastModifiedBy = Common.CurrentUserId;
                balDue.RevenueAnalystCollector = Common.CurrentUserId;
                balDue.LastModified = DateTime.Now;
                balDue.Vessel = data.BookingHeaderDetails.VESSCODE ?? "";
                balDue.Voyage = data.BookingHeaderDetails.Voyage ?? "";
                balDue.Quote = data.Quote ?? "";
                balDue.ConsigneeRefNumber = "";
                balDue.ShippersRefNumber = "";
                balDue.PayorRefNumber = "";

                balDue.InvoiceType = data.BookingHeaderDetails.InvoiceType;
                balDue.Currency = data.Currency;
                balDue.OriginLocationCode = data.BookingHeaderDetails.MATRCLOCA;
                balDue.DestinationLocationCode = data.BookingHeaderDetails.MATRDLOCA;
                balDue.BalanceDueInvoiceDate = DateTime.Now;
                Common.BalanceDues.SaveBalanceDue(balDue);

                var payment = new BalanceDues_Payments();
                if (data.PaymentId > 0)
                {
                    payment = Common.BalanceDuesPayments.GetBalanceDuesPayment(data.PaymentId, "") ?? new BalanceDues_Payments();
                }
                else
                {
                    payment.PaymentType = "";
                    payment.BalanceDueInvoiceNo = "";
                    payment.PaymentReference = "";
                    payment.PaymentDate = DateTime.Now;
                }

                payment.CompanyID = companyId;
                payment.Region = region;
                payment.CustomerID = data.CustomerId;
                payment.CustomerLocationCode = data.BookingHeaderDetails.MATRCLOCA;
                payment.BookingID = "";
                payment.BOLNo = data.BookingHeaderDetails.BOLNo;
                payment.BOLExecutionDate = bolExecDate;
                payment.BalanceDueInvoiceDate = Convert.ToDateTime(invoiceDate);
                payment.Payor = data.CustomerId;
                payment.PaymentAmount = data.AmountPaid;
                payment.Currency = data.Currency;
                Common.BalanceDuesPayments.SaveBalanceDuesPayment(payment);

                var participant = Common.Participant.GetParticipant(data.CustomerId) ?? new Arg.Ceva.DataAccess.Participants.Participant();

                var customer = new Customers();

                if (!string.IsNullOrWhiteSpace(data.CustomerId))
                {
                    customer = Common.Customers.GetCustomer(0, Common.GetActiveClientId(), data.CustomerId);
                }

                if (customer == null || customer.BdCustomerId <= 0)
                {
                    customer = new Customers();
                }

                customer.CompanyId = companyId;
                customer.Region = region;
                customer.Address1 = "";
                customer.Address2 = "";
                customer.City = "";
                customer.State = "";
                customer.Country = "";
                customer.ZipCode = !string.IsNullOrWhiteSpace(participant.Zip) ? participant.Zip : "";
                customer.LastUpdated = DateTime.Now;
                customer.CustomerName = participant.Name;
                customer.CustomerId = data.CustomerId;
                Common.Customers.SaveCustomer(customer);

                var resItem = Common.ResearchItems.GetResearchItem(0, Common.GetActiveClientId(), data.BookingHeaderDetails.BOLNo, region, bolExecDate);
                if (resItem != null && resItem.ResearchId > 0)
                {
                    if (resItem.Status == "Open")
                    {
                        resItem.Status = "Closed";
                        resItem.LastModified = DateTime.Now;
                        resItem.LastModifiedBy = Common.CurrentUserId;
                        Common.ResearchItems.SaveResearchItem(resItem);
                    }
                }
                return Json("Success");
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
                return Json(ex.ToString());
            }
        }

        public IActionResult ViewAuditingResultStats(int? queryId, int? idx, int? group)
        {
            var data = new BookingAuditingResult();
            try
            {
                List<Arg.Ceva.DataAccess.BookingHeader.BookingHeaderImp> bolAuditingStats = null;
                var searchOptions = new Arg.Ceva.DataAccess.SearchOptions();
                var _queryId = Convert.ToInt32(queryId);
                var _idx = Convert.ToInt32(idx);
                var qr = new Arg.DataAccess.QueryResultsImpl().GetQueryResults(_queryId);

                searchOptions = Newtonsoft.Json.JsonConvert.DeserializeObject<Arg.Ceva.DataAccess.SearchOptions>(qr.QueryJson);

                bolAuditingStats = Common.BookingHeaders.GetResults(_queryId, "Stats");

                if (bolAuditingStats == null || !bolAuditingStats.Any())
                {
                    data.Message = "No results found related to  your search!";
                    return View(data);
                }
                else
                {
                    data.BolAuditResultStats = bolAuditingStats;
                    data.SearchOptions = searchOptions;
                    data.SpreedSheetUrl = GenerateSpreadsheetforStats(data, "Group by SHIPPER, POL, POD");
                }
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex.ToString());
            }
            return View(data);
        }

        [Authorize]
        public IActionResult ViewAuditingResultStatsByOrigin(int? queryId, int? idx, int? group)
        {
            var data = new BookingAuditingResult();
            try
            {
                List<Arg.Ceva.DataAccess.BookingHeader.BookingHeaderImp> bolAuditingStats = null;

                var searchOptions = new Arg.Ceva.DataAccess.SearchOptions();
                var _queryId = Convert.ToInt32(queryId);
                var _idx = Convert.ToInt32(idx);
                var qr = new Arg.DataAccess.QueryResultsImpl().GetQueryResults(_queryId);

                searchOptions = Newtonsoft.Json.JsonConvert.DeserializeObject<Arg.Ceva.DataAccess.SearchOptions>(qr.QueryJson);

                bolAuditingStats = Common.BookingHeaders.GetResults(_queryId, "StatsByOrigin");

                if (bolAuditingStats == null || !bolAuditingStats.Any())
                {
                    data.Message = "No results found related to  your search!";
                    return View(data);
                }
                else
                {
                    data.BolAuditResultStats = bolAuditingStats;
                    data.SearchOptions = searchOptions;
                    data.SpreedSheetUrl = GenerateSpreadsheetforStats(data, "Group by Origin/Destination");
                }
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex.ToString());
            }
            return View(data);
        }

        [Authorize]
        public IActionResult ViewAuditingResultStatsByPOL(int? queryId, int? idx, int? group)
        {
            var data = new BookingAuditingResult();

            try
            {
                List<Arg.Ceva.DataAccess.BookingHeader.BookingHeaderImp> bolAuditingStats = null;

                var searchOptions = new Arg.Ceva.DataAccess.SearchOptions();
                var _queryId = Convert.ToInt32(queryId);
                var _idx = Convert.ToInt32(idx);
                var qr = new Arg.DataAccess.QueryResultsImpl().GetQueryResults(_queryId);

                searchOptions = Newtonsoft.Json.JsonConvert.DeserializeObject<Arg.Ceva.DataAccess.SearchOptions>(qr.QueryJson);

                bolAuditingStats = Common.BookingHeaders.GetResults(_queryId, "StatsByPOL");

                if (bolAuditingStats == null || !bolAuditingStats.Any())
                {
                    data.Message = "No results found related to  your search!";
                    return View(data);
                }
                else
                {
                    data.BolAuditResultStats = bolAuditingStats;
                    data.SearchOptions = searchOptions;
                    data.SpreedSheetUrl = GenerateSpreadsheetforStats(data, "Group by POL");
                }
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex.ToString());
            }
            return View(data);
        }

        public string GenerateSpreadsheetforStats(BookingAuditingResult stats, string filter)
        {
            try
            {
                if (Arg.DataAccess.ActiveClient.Info.CompanyId <= 0) return "Client not active. Please <a href='" + Common.MyRoot + "Account/Login' title='Login'>re-login.</a>";

                var spreadsheetInfo = new Arg.Ceva.DataAccess.StatsOutputSpreadsheet();

                spreadsheetInfo.OutputSpreadsheetStats = stats.BolAuditResultStats ?? new List<Arg.Ceva.DataAccess.BookingHeader.BookingHeaderImp>();//, Common.GetActiveClientId()
                if (spreadsheetInfo.OutputSpreadsheetStats != null && spreadsheetInfo.OutputSpreadsheetStats.Any())
                {
                    var sheetTitle = Common.ActiveClient.GetName() + "_StatsOutput_" + DateTime.Now.Date.ToString("MMddyyy");
                    //var info = Arg.Ceva.DataAccess.BookingHeader.CreateStatsSpreadsheet(spreadsheetInfo, sheetTitle, Common.MyRoot, filter);
                    //Common.ActivityStats.SaveActivityStats(arg.DataAccess.ActivityStatsImpl.EnumActions.GeneratedStatsOutputSpreadsheet, Common.GetActiveClientId(), sheetTitle, "");
                    //return info.Url;
                }
                return "";
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
                Common.Log.Error(ex);
            }
            return null;
        }

        [Authorize]
        [HttpGet]
        public virtual JsonResult GetAmountPaid(string bolNo, string custId)
        {
            decimal amountPaid = Common.ARCash.GetAmountPaid(bolNo, custId);
            return Json(amountPaid);
        }

        [Authorize]
        public void DeleteBolOtherCharges(int itemId)
        {
            Common.BalanceDuesOtherCharges.DeleteBolOtherCharges(itemId);
        }

        private class AjaxResult
        {
            public int QueryId { get; set; }
            public string Type { get; set; } //RESULTS or STATS or Message
            public bool ResultTableFormat { get; set; }
        }
    }
}
