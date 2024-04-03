using Arg.Ceva.DataAccess;
using Arg.DataModels;
using ArgCore.Attributes;
using ArgCore.Helpers;
using ArgCore.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using X.PagedList;

namespace ArgCore.Controllers
{
    public class BOLController : Controller
    {
        [AuthorizeUser]
        public ActionResult Client()

        {
            var bOLAuditing = new BOLAuditing();
            try
            {
                if (HttpContext.Session.GetString("IsSessionActive") == null)
                {
                    return RedirectToAction("LogIn", "Account");
                }

                bOLAuditing.CommonObjects.TopHeading = "Atlas Auditing";
                bOLAuditing.CommonObjects.Heading = "BOL Auditing";
                var companies = Common.ArgClients.GetBOLClients(Common.CurrentUserId);
                bOLAuditing.Companies = new SelectList(companies, "CompanyId", "Name");

                if (Arg.DataAccess.ActiveClient.Info.CompanyId > 0)
                {
                    bOLAuditing.CompanyId = Arg.DataAccess.ActiveClient.Info.CompanyId;
                }
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }
            return View(bOLAuditing);
        }

        [AuthorizeUser]
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

        [AuthorizeUser]
        public IActionResult Index(BOLAuditing bolModel, string ButtonType, List<string> shipperId, List<string> orig, List<string> dest, List<string> POL, int? queryId, int? companyId)
        {
            var bOLAuditing = new BOLAuditing();

            try
            {
                if (HttpContext.Session.GetString("IsSessionActive") == null)
                {
                    return Redirect(Common.MyRoot + "Account/Login");
                }

                bOLAuditing.CommonObjects.TopHeading = "Atlas Auditing";
                bOLAuditing.CommonObjects.Heading = "BOL Auditing";
                bOLAuditing.Operators = Common.ChargeCodeOperators.Select((r, index) => new SelectListItem { Text = r, Value = r });
                bOLAuditing.CompanyId = Arg.DataAccess.ActiveClient.Info.CompanyId;

                if (bOLAuditing.CompanyId > 0 && Arg.DataAccess.ActiveClient.Info.DBName.Contains("Ceva"))
                {
                    return RedirectToAction("Index", "Booking");
                }

                if (bolModel.SearchOptions != null && Arg.DataAccess.ActiveClient.Info.DBName.Contains("ClientPasha"))
                {
                    Common.CheckActiveClient();
                    bolModel.SearchOptions.CompanyId = bOLAuditing.CompanyId;

                    if (Arg.DataAccess.ActiveClient.Info.BolBilltype && bolModel.InvoiceType == "Per Diem Invoice")
                    {
                        bolModel.SearchOptions.BillType = "PerDiem";
                    }

                    var q = Common.QueryResults.SaveQueryResults(bolModel.SearchOptions);
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

                    return Json(new AjaxResult { QueryId = q.QueryId, Type = type, ResultTableFormat = bolModel.SearchOptions.ResultTableFormat });
                }

                else if (bolModel.SearchOptions != null && Arg.DataAccess.ActiveClient.Info.DBName.Contains("ClientAgility"))
                {
                    Common.CheckActiveClient();
                    bolModel.SearchOptions.CompanyId = bOLAuditing.CompanyId;
                    var q = Common.AgilityQueryResults.SaveQueryResults(bolModel.SearchOptions);
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

                    return Json(new AjaxResult { QueryId = q.QueryId, Type = type, ResultTableFormat = bolModel.SearchOptions.ResultTableFormat });
                }
                else if (bolModel.SearchOptions != null && Arg.DataAccess.ActiveClient.Info.DBName.Contains("ClientHellmann"))
                {
                    Common.CheckActiveClient();

                    bolModel.SearchOptions.CompanyId = bOLAuditing.CompanyId;
                    if (Arg.DataAccess.ActiveClient.Info.BolBilltype && bolModel.InvoiceType == "Per Diem Invoice")
                    {
                        bolModel.SearchOptions.BillType = "PerDiem";
                    }
                    var q = Common.QueryResults.SaveQueryResults(bolModel.SearchOptions);
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

                    return Json(new AjaxResult { QueryId = q.QueryId, Type = type, ResultTableFormat = bolModel.SearchOptions.ResultTableFormat });
                }
                else
                {
                    var companies = Common.ArgClients.GetBOLClients(Common.CurrentUserId);
                    bOLAuditing.Companies = new SelectList(companies, "CompanyId", "Name", "DBName");
                    var result = companies.Find(item => item.CompanyId == bOLAuditing.CompanyId);

                    if (bOLAuditing.CompanyId > 0 && result.DBName == "ClientPasha")
                    {
                        var users = Common.AspNetUsers.GetAspNetUsers(true);
                        bOLAuditing.Users = new SelectList(users.OrderBy(x => x.UserName), "Id", "UserName");
                        var contEventTypes = Common.ContainerEventTypes.GetEventTypes(true).OrderBy(x => x.EventDescription);// don't remove order else All will come at end
                        bOLAuditing.ContainerEventTypes = new SelectList(contEventTypes, "EventType", "EventDescription");
                        var pol = Common.BOLHeader.GetDistinctPOL();
                        bOLAuditing.PortOfLoading = new SelectList(pol, "POLCode", "POL");
                        var pod = Common.BOLHeader.GetDistinctPOD();
                        bOLAuditing.PortOfDischarge = new SelectList(pod, "PODCode", "POD");
                        var origin = Common.BOLHeader.GetDistinctOrigin();
                        bOLAuditing.Origins = new SelectList(origin, "OriginLocationCode", "Origin");
                        var destination = Common.BOLHeader.GetDistinctDestination();
                        bOLAuditing.Destinations = new SelectList(destination, "DestinationLocationCode", "Destination");
                        var modes = Common.BOLHeader.GetDistinctModes();
                        bOLAuditing.Modes = new SelectList(modes, "Mode", "Mode");
                        var shipper = Common.BOLHeader.GetDistinctShipper();
                        bOLAuditing.Shippers = new SelectList(shipper, "ShipperID", "Shipper");
                        var consgn = Common.BOLHeader.GetDistinctConsignee();
                        bOLAuditing.Consignee = new SelectList(consgn, "ConsigneeID", "Consignee");
                        var payor = Common.BOLHeader.GetDistinctPayor();
                        bOLAuditing.Payors = new SelectList(payor, "PayorID", "Payor");
                        var equipSize = Common.BOLContainers.GetDistinctSize();
                        bOLAuditing.EquipmentSizes = new SelectList(equipSize, "Size", "Size");
                        var equpTypes = Common.BOLContainers.GetDistinctType();
                        bOLAuditing.EquipmentTypes = new SelectList(equpTypes, "Type", "Type");
                        var unCodes = Common.BOLHazardous.GetDistinctUNHazmatCodes();
                        bOLAuditing.UNHazmatCodes = new SelectList(unCodes, "UNHazmatCode", "UNHazmatCode");
                        var refTypes = Common.BOLReference.GetDistinctBOLReferences();
                        bOLAuditing.ReferenceTypes = new SelectList(refTypes, "ReferenceType", "ReferenceType");
                    }
                    else if (bOLAuditing.CompanyId > 0 && result.DBName == "ClientAgility")
                    {
                        var users = Common.AspNetUsers.GetAspNetUsers(true);
                        bOLAuditing.Users = new SelectList(users.OrderBy(x => x.UserName), "Id", "UserName");
                        var serviceMovementType = Common.AgilityBOLHeader.GetServiceMovementType();
                        bOLAuditing.ServiceMovementType = new SelectList(serviceMovementType, "ServiceMovementTypeCode", "ServiceMovementType");
                        var serviceLevel = Common.AgilityBOLHeader.GetServiceLevel();
                        bOLAuditing.ServiceLevel = new SelectList(serviceLevel, "ServiceLevelCode", "ServiceLevel");
                        var serviceType = Common.AgilityBOLHeader.GetServiceType();
                        bOLAuditing.ServiceType = new SelectList(serviceType, "ServiceTypeCode", "ServiceType");
                        var origin = Common.AgilityBOLHeader.GetDistinctOrigin();
                        bOLAuditing.Origins = new SelectList(origin, "OriginLocationCode", "Origin");
                        var portOfExit = Common.AgilityBOLHeader.GetPortOfExit();
                        bOLAuditing.PortOfExit = new SelectList(portOfExit, "PortOfExitCode", "PortOfExit");
                        var portofEntry = Common.AgilityBOLHeader.GetPortofEntry();
                        bOLAuditing.PortofEntry = new SelectList(portofEntry, "PortofEntryCode", "PortofEntry");
                        var destination = Common.AgilityBOLHeader.GetDestination();
                        bOLAuditing.Destinations = new SelectList(destination, "DestinationLocationCode", "Destination");
                        var shipper = Common.AgilityBOLHeader.GetShipper();
                        bOLAuditing.Shipper = new SelectList(shipper, "ShipperID", "Shipper");
                        var consignee = Common.AgilityBOLHeader.GetConsignee();
                        bOLAuditing.Consignee = new SelectList(consignee, "ConsigneeID", "Consignee");
                        var notifyParty = Common.AgilityBOLHeader.GetNotifyParty();
                        bOLAuditing.NotifyParty = new SelectList(notifyParty, "NotifyPartyCode", "NotifyParty");
                        var exportingCarrier = Common.AgilityBOLHeader.GetExportingCarrier();
                        bOLAuditing.ExportingCarrier = new SelectList(exportingCarrier, "ExportingCarrierCode", "ExportingCarrier");
                        var prepaidCollect = Common.AgilityBOLHeader.GetPrepaidCollect();
                        bOLAuditing.PrepaidCollect = new SelectList(prepaidCollect, "PrepaidCollectCode", "PrepaidCollect");
                        var unitType = Common.ContainerDetailsImpl.GetUnitType();
                        bOLAuditing.UnitType = new SelectList(unitType, "UnitTypeCode", "UnitType");
                        var hazMatFlag = Common.ContainerDetailsImpl.GetHazMatFlag();
                        bOLAuditing.HazMatFlag = new SelectList(hazMatFlag, "HazMatFlagCode", "HazMatFlag");
                    }
                    else if (bOLAuditing.CompanyId > 0 && result.DBName == "ClientHellmann")
                    {
                        var users = Common.AspNetUsers.GetAspNetUsers(true);
                        bOLAuditing.Users = new SelectList(users.OrderBy(x => x.UserName), "Id", "UserName");
                        var mode = Common.Mode.OrderBy(i => i.Text).ToList();
                        bOLAuditing.Mode = new SelectList(mode, "Value", "Text");
                        var ShipmentType = Common.ShipmentJournal.GetDistinctShipmentType();
                        bOLAuditing.ShipmentTypes = new SelectList(ShipmentType, "Shipment_Type", "ShipmentTypeDescription");
                        var IssuingDepts = Common.ShipmentJournal.GetDistinctIssuingDept();
                        bOLAuditing.IssuingDepts = new SelectList(IssuingDepts, "Issuing_Dept", "Issuing_Dept");
                        var ShipmentStatus = Common.ShipmentJournal.GetDistinctShipmentStatus();
                        bOLAuditing.ShipmentStatuss = new SelectList(ShipmentStatus, "Shipment_Status", "ShipmentStatusDescription");
                        var ShipmentCLStatus = Common.ShipmentJournal.GetDistinctShipmentCLStatus();
                        bOLAuditing.ShipmentCLStatuss = new SelectList(ShipmentCLStatus, "Shipment_CL_Status", "ShipmentCLStatusDescription");
                        var Regions = Common.ShipmentJournal.GetDistinctRegion();
                        bOLAuditing.ShipmentRegions = new SelectList(Regions, "Region", "RegionDescription");
                        var Origins = Common.ShipmentJournal.GetDistinctOrigin();
                        bOLAuditing.Origins = new SelectList(Origins, "origin", "origin");
                        var Destinations = Common.ShipmentJournal.GetDistinctDestination();
                        bOLAuditing.Destinations = new SelectList(Destinations, "dest", "dest");
                        var FilterSigns = Common.ChargableWeightOperator.ToList();
                        bOLAuditing.FilterSign = new SelectList(FilterSigns, "Value", "Text");
                    }
                    if (bOLAuditing.CompanyId > 0 && result.DBName == "ClientPasha")
                    {
                        var sortOptions = new List<Arg.DataModels.SortOptionsObj>();
                        string[,] sortCols = { { "Shipper Name", "ParticipantName" }, { "Origin Location Code", "b.OriginLocationCode" }, { "Destination Location Code", "b.DestinationLocationCode" }, { "Commodity Code", "b.CommodityCode" }, { "Mode", "b.Mode" }, { "Actual Departure Date", "b.ActualDepartureDate" } };//{ "Booking ID", "b.BookingID" },
                        bOLAuditing.SearchOptions = new Arg.DataModels.SearchOptions();
                        bOLAuditing.SearchOptions.SortOptions = new List<Arg.DataModels.SortOptionsObj>();
                        if (sortCols != null)
                        {
                            var count = (sortCols.Length / 2) - 1;
                            for (var i = 0; i <= count; i++)
                            {
                                var si = new Arg.DataModels.SortOptionsObj
                                {
                                    DisplayName = sortCols[i, 0],
                                    ColumnName = sortCols[i, 1],
                                    Idx = 0,
                                    IsDesc = false,
                                    IsSelected = true
                                };
                                sortOptions.Add(si);
                            }
                        }
                        if (sortOptions != null)
                        {
                            bOLAuditing.SearchOptions.SortOptions = sortOptions;

                        }  
                    }
                    else if (bOLAuditing.CompanyId > 0 && result.DBName == "ClientAgility")
                    {
                        var sortOptions = new List<Arg.DataModels.SortOptionsObj>();
                        string[,] sortCols = { { "Origin", "Origin" }, { "Destination", "Destination" }, { "Shipper", "Shipper" }, { "PortOfExit", "PortOfExit" }, { "PortOfEntry", "PortOfEntry" }, { "JobConfirmationDate", "JobConfirmationDate" } };
                        bOLAuditing.SearchOptions = new Arg.DataModels.SearchOptions();
                        bOLAuditing.SearchOptions.SortOptions = new List<Arg.DataModels.SortOptionsObj>();

                        if (sortCols != null)
                        {
                            var count = (sortCols.Length / 2) - 1;
                            for (var i = 0; i <= count; i++)
                            {
                                var si = new Arg.DataModels.SortOptionsObj
                                {
                                    DisplayName = sortCols[i, 0],
                                    ColumnName = sortCols[i, 1],
                                    Idx = 0,
                                    IsDesc = false,
                                    IsSelected = true
                                };
                                sortOptions.Add(si);
                            }
                        }
                        if (sortOptions != null)
                        {
                            bOLAuditing.SearchOptions.SortOptions = sortOptions;
                        }  
                    }
                    if (bOLAuditing.CompanyId > 0 && result.DBName == "ClientHellmann")
                    {
                        var sortOptions = new List<Arg.DataModels.SortOptionsObj>();
                        string[,] sortCols = { { "Region", "Region" }, { "Issuing Department", "Issuing_Dept" }, { "Shipper/Consignee", "Shipper_Consignee" }, { "Origin", "b.Origin" }, { "Dest", "Dest" }, { "Shipment Date", "Shipment_Date" } };
                        bOLAuditing.SearchOptions = new Arg.DataModels.SearchOptions();
                        bOLAuditing.SearchOptions.SortOptions = new List<Arg.DataModels.SortOptionsObj>();
                        if (sortCols != null)
                        {
                            var count = (sortCols.Length / 2) - 1;
                            for (var i = 0; i <= count; i++)
                            {
                                var si = new Arg.DataModels.SortOptionsObj
                                {
                                    DisplayName = sortCols[i, 0],
                                    ColumnName = sortCols[i, 1],
                                    Idx = 0,
                                    IsDesc = false,
                                    IsSelected = true
                                };
                                sortOptions.Add(si);
                            }
                        }
                        if (sortOptions != null)
                        {
                            bOLAuditing.SearchOptions.SortOptions = sortOptions;
                        }    
                    }
                    if ((shipperId != null) || (orig != null) || (dest != null) || queryId != null)
                    {
                        var _queryId = Convert.ToInt32(queryId);
                        var qr = new Arg.DataAccess.QueryResultsImpl().GetQueryResults(_queryId);

                        if (qr != null)
                        {
                            var searchOptions = JsonConvert.DeserializeObject<Arg.DataModels.SearchOptions>(qr.QueryJson);
                            bOLAuditing.SearchOptions = searchOptions;
                            if (shipperId != null)
                            {
                                bOLAuditing.SearchOptions.ShipperID = shipperId;
                            }
                            if (orig != null)
                            {
                                bOLAuditing.SearchOptions.OriginLocationCode = orig;
                            }
                            if (dest != null)
                            {
                                bOLAuditing.SearchOptions.DestinationLocationCode = dest;
                            }
                            if (POL != null)
                            {
                                bOLAuditing.SearchOptions.POLCode = POL;
                            }
                            bOLAuditing.SearchOptions.DepartureStartDate = searchOptions.DepartureStartDate;
                            bOLAuditing.SearchOptions.DepartureEndDate = searchOptions.DepartureEndDate;
                        }
                        bOLAuditing.SearchOptions.SortOptions = new List<Arg.DataModels.SortOptionsObj>();
                    }
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

        [IpAuth]
        public ActionResult ViewAuditingResultStats(int? queryId, int? idx, int? group)
        {
            var data = new BOLAuditingResults();
            try
            {
                var searchOptions = new Arg.DataModels.SearchOptions();
                var _queryId = Convert.ToInt32(queryId);
                var _idx = Convert.ToInt32(idx);

                if (Arg.DataAccess.ActiveClient.Info.DBName.Contains("ClientPasha"))
                {
                    var qr = new Arg.DataAccess.QueryResultsImpl().GetQueryResults(_queryId);
                    searchOptions = JsonConvert.DeserializeObject<Arg.DataModels.SearchOptions>(qr.QueryJson);
                    List<BOLHeader> bolAuditingStats = Common.BOLHeader.GetResults(_queryId, "Stats");

                    if (bolAuditingStats == null || !bolAuditingStats.Any())
                    {
                        data.Message = "No results found related to  your search!";
                        return View(data);
                    }
                    else
                    {
                        data.BolAuditResultStats = bolAuditingStats;
                        data.SearchOptions = searchOptions;
                        data.SpreedSheetUrl = GenerateSpreadsheetforStats(data, "Group by Shipper/Origin/Destination");
                    }
                }
                else if (Arg.DataAccess.ActiveClient.Info.DBName.Contains("ClientAgility"))
                {
                    var qr = Common.AgilityQueryResults.GetQueryResults(_queryId);
                    searchOptions = Newtonsoft.Json.JsonConvert.DeserializeObject<Arg.DataModels.SearchOptions>(qr.QueryJson);
                    List<Arg.Agility.DataModels.BOLHeaders> agilityBolAuditResultStats = Common.AgilityBOLHeader.GetResults(_queryId, "Stats");

                    if (agilityBolAuditResultStats == null || !agilityBolAuditResultStats.Any())
                    {
                        data.Message = "No results found related to  your search!";
                        return View(data);
                    }
                    else
                    {
                        data.AgilityBolAuditResultStats = agilityBolAuditResultStats;
                        data.SearchOptions = searchOptions;
                        data.SpreedSheetUrl = GenerateSpreadsheetforStats(data, "Group by Shipper/Origin/Destination");
                    }
                }
                else if (Arg.DataAccess.ActiveClient.Info.DBName.Contains("ClientHellmann"))
                {
                    var qr = new Arg.DataAccess.QueryResultsImpl().GetQueryResults(_queryId);
                    searchOptions = Newtonsoft.Json.JsonConvert.DeserializeObject<Arg.DataModels.SearchOptions>(qr.QueryJson);
                    List<Arg.DataModels.ShipmentJournal> ShipmentAuditingStats = Common.ShipmentJournal.GetResults(_queryId, "Stats");

                    if (ShipmentAuditingStats == null || !ShipmentAuditingStats.Any())
                    {
                        data.Message = "No results found related to  your search!";
                        return View(data);
                    }
                    else
                    {
                        data.ShipmentJournalAuditResultStats = ShipmentAuditingStats;
                        data.SearchOptions = searchOptions;
                        //data.SpreedSheetUrl = GenerateSpreadsheetforStats(data, "Group by Shipper/Origin/Destination");
                    }
                }
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex.ToString());
            }
            return View(data);
        }

        public string GenerateSpreadsheetforStats(BOLAuditingResults stats, string filter)
        {
            try
            {
                if (Arg.DataAccess.ActiveClient.Info.CompanyId <= 0) return "Client not active. Please <a href='" + Common.MyRoot + "Account/Login' title='Login'>re-login.</a>";
                var spreadsheetInfo = new Arg.DataModels.StatsOutputSpreadsheet();
                if (Arg.DataAccess.ActiveClient.Info.DBName.Contains("ClientPasha"))
                {
                    spreadsheetInfo.OutputSpreadsheetStats = stats.BolAuditResultStats ?? new List<BOLHeader>();//, Common.GetActiveClientId()
                    if (spreadsheetInfo.OutputSpreadsheetStats != null && spreadsheetInfo.OutputSpreadsheetStats.Any())
                    {
                        var sheetTitle = Common.ActiveClient.GetName() + "_StatsOutput_" + DateTime.Now.Date.ToString("MMddyyy");
                        var info = Arg.DataAccess.BalanceDuesImpl.CreateStatsSpreadsheet(spreadsheetInfo, sheetTitle, Common.MyRoot, filter);
                        //Common.ActivityStats.SaveActivityStats(arg.DataAccess.ActivityStatsImpl.EnumActions.GeneratedStatsOutputSpreadsheet, Common.GetActiveClientId(), sheetTitle, "");
                        return info.Url;
                    }

                    return "";
                }

                if (Arg.DataAccess.ActiveClient.Info.DBName.Contains("ClientAgility"))
                {
                    spreadsheetInfo.AgilityOutputSpreadsheetStats = stats.AgilityBolAuditResultStats ?? new List<Arg.Agility.DataModels.BOLHeaders>();//, Common.GetActiveClientId()
                    if (spreadsheetInfo.AgilityOutputSpreadsheetStats != null && spreadsheetInfo.AgilityOutputSpreadsheetStats.Any())
                    {
                        var sheetTitle = Common.ActiveClient.GetName() + "_StatsOutput_" + DateTime.Now.Date.ToString("MMddyyy");
                        var info = Arg.DataAccess.BalanceDuesImpl.CreateStatsSpreadsheet(spreadsheetInfo, sheetTitle, Common.MyRoot, filter);
                        //Common.ActivityStats.SaveActivityStats(arg.DataAccess.ActivityStatsImpl.EnumActions.GeneratedStatsOutputSpreadsheet, Common.GetActiveClientId(), sheetTitle, "");
                        return info.Url;
                    }

                    return "";
                }
                if (Arg.DataAccess.ActiveClient.Info.DBName.Contains("ClientHellmann"))
                {
                    spreadsheetInfo.ShipmentOutputSpreadsheetStats = stats.ShipmentJournalAuditResultStats ?? new List<ShipmentJournal>();
                    if (spreadsheetInfo.ShipmentOutputSpreadsheetStats != null && spreadsheetInfo.ShipmentOutputSpreadsheetStats.Any())
                    {
                        var sheetTitle = Common.ActiveClient.GetName() + "_StatsOutput_" + DateTime.Now.Date.ToString("MMddyyy");
                        var info = Arg.DataAccess.BalanceDuesImpl.CreateStatsSpreadsheet(spreadsheetInfo, sheetTitle, Common.MyRoot, filter);
                        return info.Url;
                    }
                    return "";
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
                Common.Log.Error(ex);
            }
            return null;
        }

        [Authorize]
        public IActionResult ViewAuditingResultStatsByOrigin(int? queryId, int? idx, int? group)
        {
            var data = new BOLAuditingResults();
            try
            {
                List<BOLHeader> bolAuditingStats = null;
                var searchOptions = new Arg.DataModels.SearchOptions();
                var _queryId = Convert.ToInt32(queryId);
                var _idx = Convert.ToInt32(idx);
                if (Arg.DataAccess.ActiveClient.Info.DBName.Contains("ClientPasha"))
                {
                    var qr = new Arg.DataAccess.QueryResultsImpl().GetQueryResults(_queryId);

                    searchOptions = JsonConvert.DeserializeObject<Arg.DataModels.SearchOptions>(qr.QueryJson);

                    bolAuditingStats = Common.BOLHeader.GetResults(_queryId, "StatsByOrigin");

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
                else if (Arg.DataAccess.ActiveClient.Info.DBName.Contains("ClientAgility"))
                {
                    var qr = Common.AgilityQueryResults.GetQueryResults(_queryId);
                    searchOptions = JsonConvert.DeserializeObject<Arg.DataModels.SearchOptions>(qr.QueryJson);
                    List<Arg.Agility.DataModels.BOLHeaders> agilityBolAuditResultStats = Common.AgilityBOLHeader.GetResults(_queryId, "StatsByOrigin");

                    if (agilityBolAuditResultStats == null || !agilityBolAuditResultStats.Any())
                    {
                        data.Message = "No results found related to  your search!";
                        return View(data);
                    }
                    else
                    {
                        data.AgilityBolAuditResultStats = agilityBolAuditResultStats;
                        data.SearchOptions = searchOptions;
                        data.SpreedSheetUrl = GenerateSpreadsheetforStats(data, "Group by Origin/Destination");
                    }
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
            var data = new BOLAuditingResults();
            try
            {
                List<Arg.DataModels.BOLHeader> bolAuditingStats = null;
                var searchOptions = new Arg.DataModels.SearchOptions();
                var _queryId = Convert.ToInt32(queryId);
                var _idx = Convert.ToInt32(idx);

                if (Arg.DataAccess.ActiveClient.Info.DBName.Contains("ClientPasha"))
                {
                    var qr = new Arg.DataAccess.QueryResultsImpl().GetQueryResults(_queryId);
                    searchOptions = Newtonsoft.Json.JsonConvert.DeserializeObject<Arg.DataModels.SearchOptions>(qr.QueryJson);
                    bolAuditingStats = Common.BOLHeader.GetResults(_queryId, "StatsByPOL");
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
                else if (Arg.DataAccess.ActiveClient.Info.DBName.Contains("ClientAgility"))
                {
                    var qr = Common.AgilityQueryResults.GetQueryResults(_queryId);
                    searchOptions = Newtonsoft.Json.JsonConvert.DeserializeObject<Arg.DataModels.SearchOptions>(qr.QueryJson);
                    List<Arg.Agility.DataModels.BOLHeaders> agilityBolAuditResultStats = Common.AgilityBOLHeader.GetResults(_queryId, "StatsByPOL");
                    if (agilityBolAuditResultStats == null || !agilityBolAuditResultStats.Any())
                    {
                        data.Message = "No results found related to  your search!";
                        return View(data);
                    }
                    else
                    {
                        data.AgilityBolAuditResultStats = agilityBolAuditResultStats;
                        data.SearchOptions = searchOptions;
                        data.SpreedSheetUrl = GenerateSpreadsheetforStats(data, "Group by POL");
                    }
                }
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex.ToString());
            }
            return View(data);
        }

        [Authorize]
        public IActionResult ViewAuditingResultStatsByShipper(int? queryId, int? idx, int? group)
        {
            var data = new BOLAuditingResults();
            try
            {
                List<BOLHeader> bolAuditingStats = null;
                var searchOptions = new Arg.DataModels.SearchOptions();
                var _queryId = Convert.ToInt32(queryId);
                var _idx = Convert.ToInt32(idx);

                if (Arg.DataAccess.ActiveClient.Info.DBName.Contains("ClientPasha"))
                {
                    var qr = new Arg.DataAccess.QueryResultsImpl().GetQueryResults(_queryId);

                    searchOptions = JsonConvert.DeserializeObject<Arg.DataModels.SearchOptions>(qr.QueryJson);

                    bolAuditingStats = Common.BOLHeader.GetResults(_queryId, "StatsByShipper");

                    if (bolAuditingStats == null || !bolAuditingStats.Any())
                    {
                        data.Message = "No results found related to  your search!";
                        return View(data);
                    }
                    else
                    {
                        data.BolAuditResultStats = bolAuditingStats;
                        data.SearchOptions = searchOptions;
                        data.SpreedSheetUrl = GenerateSpreadsheetforStats(data, "Group by Shipper");
                    }
                }
                else if (Arg.DataAccess.ActiveClient.Info.DBName.Contains("ClientAgility"))
                {
                    var qr = Common.AgilityQueryResults.GetQueryResults(_queryId);
                    searchOptions = JsonConvert.DeserializeObject<Arg.DataModels.SearchOptions>(qr.QueryJson);
                    List<Arg.Agility.DataModels.BOLHeaders> agilityBolAuditResultStats = Common.AgilityBOLHeader.GetResults(_queryId, "StatsByShipper");
                    if (agilityBolAuditResultStats == null || !agilityBolAuditResultStats.Any())
                    {
                        data.Message = "No results found related to  your search!";
                        return View(data);
                    }
                    else
                    {
                        data.AgilityBolAuditResultStats = agilityBolAuditResultStats;
                        data.SearchOptions = searchOptions;
                        data.SpreedSheetUrl = GenerateSpreadsheetforStats(data, "Group by Shipper");
                    }
                }
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex.ToString());
            }
            return View(data);
        }

        [IpAuth]
        public IActionResult AuditingResults(int? queryId, int? idx, string bolNo, int? companyId, string customerId, string region, DateTime? bolExecDate, List<string> shipperId, List<string> orig, List<string> dest, List<string> POL, string stats)
        {
            var data = new BOLAuditingResults();

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

                BOLHeader bolAuditingResults = null;
                Arg.Agility.DataModels.BOLHeaders agilityBolAuditingResults = null;
                ShipmentJournal ShipmentAuditingResults = null;

                var _queryId = Convert.ToInt32(queryId);
                var _idx = Convert.ToInt32(idx);

                if (((shipperId != null) || (orig != null) || (dest != null) || (POL != null)) && (stats == "stats"))
                {
                    queryId = SaveQueryJson(shipperId, orig, dest, POL, _queryId);
                    _queryId = Convert.ToInt32(queryId);
                }

                var note = @"BOL#: <a href='" + Common.MyRoot + "/Bol/AuditingResults?queryId=" + queryId + "&CompanyId=" + _companyId + "' target='_blank'>" + bolNo + "</a>";
                data.ShowNavigation = true;

                if (queryId != null && queryId > 0)
                {
                    if (Arg.DataAccess.ActiveClient.Info.DBName.Contains("ClientPasha"))
                    {
                        var qry = Common.QueryResults.GetQueryResults(_queryId);
                        data.QueryId = _queryId;
                        var avc = Thread.CurrentThread.CurrentCulture;
                        if (qry.SqlQuery == null)
                        {
                            bolAuditingResults = Common.BOLHeader.GetResult(_queryId, _idx);
                        }
                    }
                    else if (Arg.DataAccess.ActiveClient.Info.DBName.Contains("ClientAgility"))
                    {
                        var qr = Common.AgilityQueryResults.GetQueryResults(_queryId);
                        data.QueryId = _queryId;
                        var avc = Thread.CurrentThread.CurrentCulture;
                        //var searchOptions = Newtonsoft.Json.JsonConvert.DeserializeObject<SearchOptions>(qr.QueryJson);
                        if (qr.SqlQuery == null)
                        {
                            agilityBolAuditingResults = Common.AgilityBOLHeader.GetResult(_queryId, _idx);
                        }
                    }
                    if (Arg.DataAccess.ActiveClient.Info.DBName.Contains("ClientHellmann"))
                    {
                        var QueryResult = Common.QueryResults.GetQueryResults(_queryId);
                        data.QueryId = _queryId;
                        var avc = Thread.CurrentThread.CurrentCulture;
                        if (QueryResult.SqlQuery == null)
                        {
                            ShipmentAuditingResults = Common.ShipmentJournal.GetResult(_queryId, _idx);
                        }
                    }
                    if (bolAuditingResults == null && Arg.DataAccess.ActiveClient.Info.DBName.Contains("ClientPasha"))
                    {
                        data.Message = "No results found related to  your search!";
                        return View(data);
                    }
                    else if (agilityBolAuditingResults == null && Arg.DataAccess.ActiveClient.Info.DBName.Contains("ClientAgility"))
                    {
                        data.Message = "No results found related to  your search!";
                        return View(data);
                    }
                    if (ShipmentAuditingResults == null && Arg.DataAccess.ActiveClient.Info.DBName.Contains("ClientHellmann"))
                    {
                        data.Message = "No results found related to  your search!";
                        return View(data);
                    }
                    else
                    {
                        if (Arg.DataAccess.ActiveClient.Info.DBName.Contains("ClientPasha"))
                        {
                            data.TotalResultCount = bolAuditingResults.ResultCount;
                            data.InvoiceBillType = bolAuditingResults.BillType;
                            data.BOLAuditResults = bolAuditingResults;
                        }
                        else if (Arg.DataAccess.ActiveClient.Info.DBName.Contains("ClientAgility"))
                        {
                            data.TotalResultCount = agilityBolAuditingResults.ResultCount;
                            data.AgilityBOLAuditResults = agilityBolAuditingResults;
                        }
                        if (Arg.DataAccess.ActiveClient.Info.DBName.Contains("ClientHellmann"))
                        {
                            data.TotalResultCount = ShipmentAuditingResults.ResultCount.Value;
                            data.ShipmentJournal = ShipmentAuditingResults;
                        }
                    }
                    if (Arg.DataAccess.ActiveClient.Info.DBName.Contains("ClientPasha"))
                    {
                        bolNo = data.BOLAuditResults.BOLNo;
                    }
                    else if (Arg.DataAccess.ActiveClient.Info.DBName.Contains("ClientAgility"))
                    {
                        bolNo = data.AgilityBOLAuditResults.JobNumber;
                    }
                    if (Arg.DataAccess.ActiveClient.Info.DBName.Contains("ClientHellmann"))
                    {
                        bolNo = data.ShipmentJournal.Shipment_No;
                        var pdfFile = Common.ShipmentJournal.GetDocumentImage(bolNo, data.ShipmentJournal.Shipment_Date.ToString(), _companyId);
                        if (pdfFile != null && pdfFile.Any())
                        {
                            data.HellmannDocuments = pdfFile;
                            data.FileNames = JavaScript.Serialize(data.PDFFile.Select(x => x.fileName));
                        }
                    }
                    data.Idx = _idx;
                    data.ShowNavigation = true;

                    if (HttpContext.Session.GetString("IsSessionActive") == null)
                    {
                        return RedirectToAction("LogIn", "Account");
                    }
                }
                if (!string.IsNullOrWhiteSpace(bolNo))
                {
                    var balDue = Common.BalanceDues.GetARGBalanceDue(bolNo, _companyId, customerId, region, Convert.ToDateTime(bolExecDate));
                    data.BOLAuditResults = new BOLHeader();
                    data.BOLAuditResults.BOLNo = bolNo;
                    var bdPaymentAmount = Common.BalanceDues.GetBDPaymentAmount(bolNo, _companyId);
                    data.BDPaymentAmount = bdPaymentAmount;

                    if (balDue != null)
                    {
                        data.BalanceDueDetails = balDue;
                    }

                    if (Arg.DataAccess.ActiveClient.Info.DBName.Contains("ClientPasha"))
                    {
                        var bookingInfo = Common.Bookings.GetBookingInfo(bolNo);
                        if (bookingInfo != null && !string.IsNullOrWhiteSpace(bookingInfo.BookingID))
                        {
                            data.BookingInfo = bookingInfo;
                        }

                        var bookingItemDetails = Common.Bookings.GetBookingItemDetails(bolNo);
                        if (bookingItemDetails != null && !string.IsNullOrWhiteSpace(bookingItemDetails.BookingID))
                        {
                            data.BookingItemDetails = bookingItemDetails;
                        }

                        var bolHeader = Common.Bookings.GetBOLHeaderSection(bolNo);
                        if (bolHeader != null && !string.IsNullOrWhiteSpace(bolHeader.BookingID))
                        {
                            data.BOLHeaderDetails = bolHeader;
                            var pdfFile = Common.DocumentImages.GetDocumentImage(bolNo, data.BOLHeaderDetails.BookingID);
                            if (pdfFile != null && pdfFile.Any())
                            {
                                data.PDFFile = pdfFile;
                                data.FileNames = JavaScript.Serialize(data.PDFFile.Select(x => x.fileName));
                            }
                        }

                        var references = Common.BOLReference.GetBOLReferences(bolNo);
                        if (references != null && references.Any())
                        {
                            data.BOLReferences = references;
                        }

                        var bolCommodity = Common.BOLCommodity.GetBOLItemDetail(bolNo);
                        if (bolCommodity != null && bolCommodity.Any())
                        {
                            data.BOLCommodity = bolCommodity;
                        }

                        var bolCharges = Common.BOLCharges.GetBOLCharges(bolNo);
                        if (bolCharges != null && bolCharges.Any())
                        {
                            data.BOLCharges = bolCharges;
                        }

                        var innvSummary = Common.InvoiceSummary.GetInvoiceSummary(bolNo);
                        if (innvSummary != null && innvSummary.Any())
                        {
                            data.InvoiceSummary = innvSummary;
                        }

                        var eventHistory = Common.ContainerEventHistory.GetContEventHist(data.BookingInfo.BookingID, bolNo);
                        if (eventHistory != null && eventHistory.Any())
                        {
                            data.ContainerEventHistory = eventHistory;
                        }

                        var bookRemarks = Common.BookingsRemarks.GetBookingsRemarks(bolNo);
                        if (bookRemarks != null && bookRemarks.Any())
                        {
                            data.BookingRemarks = bookRemarks;
                        }

                        var bookNotes = Common.BookingsNotes.GetBookingsNotes(bolNo);
                        if (bookNotes != null && bookNotes.Any())
                        {
                            data.BookingNotes = bookNotes;
                        }

                        var bolRemarks = Common.BOLRemarks.GetBOLRemarks(bolNo);
                        if (bolRemarks != null && bolRemarks.Any())
                        {
                            data.BOLRemarks = bolRemarks;
                        }
                        note = @"BOL#: <a href='" + Common.MyRoot + "/Bol/AuditingResults?bolNo=" + bolNo + "&CompanyId=" + _companyId + "' target='_blank'>" + bolNo + "</a>";
                    }
                    else if (Arg.DataAccess.ActiveClient.Info.DBName.Contains("ClientAgility"))
                    {
                        var bookingInfo = Common.AgilityBookingHeaders.GetBookingInfo(bolNo);
                        if (bookingInfo != null && !string.IsNullOrWhiteSpace(bookingInfo.JobNumber))
                        {
                            bookingInfo.CarrierName = Common.AgilityBookingHeaders.GetCarrierName(bookingInfo.CarrierCode);
                            data.AgilityBookingInfo = bookingInfo;
                        }

                        var shipmentHeader = Common.AgilityBookingHeaders.GetBOLHeaderSection(bolNo);
                        if (shipmentHeader != null && !string.IsNullOrWhiteSpace(shipmentHeader.JobNumber))
                        {
                            data.AgilityBOLAuditResults = shipmentHeader;
                            var pdfFile = Common.AgilityBookingHeaders.GetDocumentImage(data.AgilityBOLAuditResults.JobNumber);
                            if (pdfFile != null && pdfFile.Any())
                            {
                                data.AgilityPDFFile = pdfFile;
                                data.FileNames = JavaScript.Serialize(data.AgilityPDFFile.Select(x => x.fileName));
                            }
                        }

                        var shipmentTracking = Common.AgilityBookingHeaders.GetShipmentTrackingDetails(bolNo);
                        if (shipmentTracking != null && !string.IsNullOrWhiteSpace(shipmentTracking.JobNumber))
                        {
                            data.AgilityShipmentTrackingDetails = shipmentTracking;
                        }

                        var shipmentContainerDetails = Common.AgilityBookingHeaders.GetBOLContainerDetails(bolNo);
                        data.AgilityBOLContainerDetails = new List<Arg.Agility.DataModels.BOLContainerDetails>();

                        foreach (var item in shipmentContainerDetails)
                        {
                            if (item != null)
                            {
                                data.AgilityBOLContainerDetails.Add(item);
                            }
                        }

                        var supplierInvoicing = Common.AgilityBookingHeaders.GetSupplierInvoicing(bolNo);
                        data.AgilityPurchaseInvoices = new List<Arg.Agility.DataModels.PurchaseInvoices>();

                        foreach (var item in supplierInvoicing)
                        {
                            if (item != null && !string.IsNullOrWhiteSpace(item.JobNumber))
                            {
                                data.AgilityPurchaseInvoices.Add(item);
                            }    
                        }

                        var customerInvoicing = Common.AgilityBookingHeaders.GetSalesInvoices(bolNo);
                        data.AgilitySalesInvoices = new List<Arg.Agility.DataModels.SalesInvoices>();
                        foreach (var item in customerInvoicing)
                        {
                            if (item != null)
                            {
                                data.AgilitySalesInvoices.Add(item);
                            }  
                        }
                    }
                    else if (Arg.DataAccess.ActiveClient.Info.DBName.Contains("ClientHellmann"))
                    {
                        if (ShipmentAuditingResults == null)
                        {
                            ShipmentAuditingResults = Common.ShipmentJournal.GetShipment(bolNo);
                            data.ShipmentJournal = ShipmentAuditingResults;
                        }
                        if (ShipmentAuditingResults != null && !string.IsNullOrEmpty(ShipmentAuditingResults.AWB_BL_No) || !string.IsNullOrEmpty(ShipmentAuditingResults.M_AWB_BL_No))
                        {
                            var HouseHAWBAir = Common.ShipmentJournal.GetHouseHAWBAir(ShipmentAuditingResults.AWB_BL_No, ShipmentAuditingResults.M_AWB_BL_No, ShipmentAuditingResults.Shipment_Date.ToString());
                            if (HouseHAWBAir != null)
                            {
                                data.HouseHAWBAirDetails = HouseHAWBAir;
                            }
                            var MasterAWB = Common.ShipmentJournal.GetMasterAWB(ShipmentAuditingResults.AWB_BL_No, ShipmentAuditingResults.M_AWB_BL_No, ShipmentAuditingResults.Shipment_Date.ToString());
                            if (MasterAWB != null)
                            {
                                data.MasterAWBDetails = MasterAWB;
                            }
                        }
                    }
                }

                data.BolNo = bolNo;
                data.EnablePullForResearchBtn = true;

                if (Arg.DataAccess.ActiveClient.Info.DBName.Contains("ClientPasha"))
                {
                    Common.ActivityStats.SaveActivityStats(Arg.DataAccess.ActivityStatsImpl.EnumActions.BOLViewed, _companyId, note, bolNo);

                    var actStatsByEvent = Common.ActivityStats.GetActivityStatsByEventType(Arg.DataAccess.ActivityStatsImpl.EnumActions.BOLViewed.ToString(), bolNo, _companyId, Common.CurrentUserId);
                    if (actStatsByEvent != null && actStatsByEvent.Any())
                    {
                        data.ActStatsByEvent = actStatsByEvent;
                    }
                }
                else if (Arg.DataAccess.ActiveClient.Info.DBName.Contains("ClientAgility"))
                {
                    Common.ActivityStats.SaveActivityStats(Arg.DataAccess.ActivityStatsImpl.EnumActions.BOLViewed, _companyId, note, bolNo);
                    var actStatsByEvent = Common.ActivityStats.GetActivityStatsByEventType(Arg.DataAccess.ActivityStatsImpl.EnumActions.BOLViewed.ToString(), bolNo, _companyId, Common.CurrentUserId);
                    if (actStatsByEvent != null && actStatsByEvent.Any())
                    {
                        data.ActStatsByEvent = actStatsByEvent;
                    }
                }
                else if (Arg.DataAccess.ActiveClient.Info.DBName.Contains("ClientHellmann") && ShipmentAuditingResults != null && !string.IsNullOrEmpty(ShipmentAuditingResults.Shipment_No))
                {
                    Common.ActivityStats.SaveActivityStats(Arg.DataAccess.ActivityStatsImpl.EnumActions.BOLViewed, _companyId, note, ShipmentAuditingResults.Shipment_No);

                    var actStatsByEvent = Common.ActivityStats.GetActivityStatsByEventType(Arg.DataAccess.ActivityStatsImpl.EnumActions.BOLViewed.ToString(), bolNo, _companyId, Common.CurrentUserId);
                    if (actStatsByEvent != null && actStatsByEvent.Any())
                    {
                        data.ActStatsByEvent = actStatsByEvent;
                    }
                }

                List<Arg.DataModels.BOLAuditSorting> pkg = new List<Arg.DataModels.BOLAuditSorting>();
                string LoginID = User.Identity.GetUserId();
                List<Arg.DataModels.BOLAuditSorting> qr2 = Common.BOLAuditSorting.GetQueryResults(_companyId, LoginID);
                data.BOLAuditSorting = new Models.BOLAuditSorting();
                if (qr2 != null && qr2.Count > 0)
                {
                    data.BOLAuditSorting.ClientId = _companyId;
                    pkg = qr2;
                }
                data.BOLAuditSorting.CompanyID = Arg.DataAccess.ActiveClient.Info.CompanyId;
                data.BOLAuditSorting.DBName = Arg.DataAccess.ActiveClient.Info.DBName;
                data.BOLAuditSorting.ConvertedJson = pkg;
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }
            return View(data);
        }

        public int Dummy()
        {
            return 1;
        }

        [AuthorizeUser]
        [CheckActiveClient]
        public IActionResult AddBalanceDue(string bolNo, int balanceId = 0, bool IsNewBal = false)
        {
            var data = new AddBalanceDue();

            try
            {
                if (Arg.DataAccess.ActiveClient.Info.DBName.Contains("ClientPasha"))
                {
                    data.Message = Arg.Core.ActiveObjects.BolIsBeingEditedInfo(bolNo);
                    Arg.Core.ActiveObjects.SetBolBeingEdited(bolNo, "", Common.CurrentUserName);
                    data.BolNo = bolNo;
                    data.CommonObjects.TopHeading = "Add Balance Due";

                    var companyId = Arg.DataAccess.ActiveClient.Info.CompanyId;

                    var currencies = Common.BOLCharges.GetDistinctCurrency();
                    data.Currencies = new SelectList(currencies, "Currency", "Currency");

                    var bolChargeInfo = Common.BOLCharges.GetBOLCharge(bolNo);
                    if (bolChargeInfo != null && !string.IsNullOrWhiteSpace(bolChargeInfo.BOLNo))
                    {
                        data.Currency = bolChargeInfo.Currency;
                    }

                    var bolCustomers = Common.BOLHeader.GetBOLCustomers();
                    if (bolCustomers != null && bolCustomers.Any())
                    {
                        data.CustomersList = bolCustomers;
                    }

                    if (IsNewBal)
                    {
                        var cust = Common.BalanceDues.GetCustomerBalanceDues(bolNo, companyId);
                        data.CustomersList = data.CustomersList.Where(x => !cust.Any(i => i.CustomerId == x.StrId)).ToList();
                    }

                    data.BalanceDue = new BalanceDue();
                    data.BalanceDue.InvoiceType = "BOL";

                    var shippBolRef = Common.BOLReference.GetBOLReference(bolNo, "SHPREF");
                    if (shippBolRef != null && !string.IsNullOrWhiteSpace(shippBolRef.Reference))
                    {
                        data.ShippersRefNo = shippBolRef.Reference;
                    }

                    var consiBolRef = Common.BOLReference.GetBOLReference(bolNo, "CONREF");
                    if (consiBolRef != null && !string.IsNullOrWhiteSpace(consiBolRef.Reference))
                    {
                        data.ConsigneeRefNo = consiBolRef.Reference;
                    }

                    var payorBolRef = Common.BOLReference.GetBOLReference(bolNo, "PAYREF");
                    if (payorBolRef != null && !string.IsNullOrWhiteSpace(payorBolRef.Reference))
                    {
                        data.PayorRefNo = payorBolRef.Reference;
                    }

                    //BolContainers
                    var bolItems = Common.BOLCommodity.GetBOLItemDetail(bolNo);
                    var bdItem = new Arg.DataModels.BalanceDues_Item { TariffRef = "980000-006" };
                    if (bolItems != null && bolItems.Any())
                    {
                        bdItem.ContainerSize = bolItems[0].Size;
                        bdItem.ContainerType = bolItems[0].Type;
                        bdItem.Container = bolItems[0].ContainerID;
                        bdItem.Weight = bolItems[0].Weight;
                        bdItem.Commodity = bolItems[0].CommodityCode;
                        bdItem.CommodityDesc = bolItems[0].CommodityDescription;
                        bdItem.Measure = Convert.ToString(bolItems[0].CBF);
                        bdItem.AmountDue = Common.BOLCharges.GetPashaAmountDue(bolNo);
                    }

                    //Payment
                    var payment = new BalanceDues_Payments();
                    if (!IsNewBal)
                    {
                        payment = Common.BalanceDuesPayments.GetBalanceDuesPayment(0, bolNo) ?? new BalanceDues_Payments();
                    }

                    data.AmountPaid = payment.PaymentAmount;
                    data.PaymentId = payment.PaymentId;

                    //BolHeader
                    var bolHeaderDetails = Common.BOLHeader.GetBOLHeader(bolNo) ?? new BOLHeader();
                    data.CustomerId = bolHeaderDetails.PayorID;

                    //BalanceDue
                    var balDue = new BalanceDue();
                    if (!IsNewBal)
                    {
                        balDue = Common.BalanceDues.GetBalanceDue(balanceId, bolNo, companyId) ?? new BalanceDue();
                    }
                    data.Description = balDue.BDDescription;

                    //data.CustomerId = balDue.CustomerId;
                    if (balDue.InvoiceType != null)
                    {
                        data.BalanceDue.InvoiceType = balDue.InvoiceType;
                    }

                    var errCodes = Common.BDErrorCodes.GetDistinctErrorCodes(companyId, false, "", data.BalanceDue.InvoiceType);
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

                    var bdDescs = Common.BalanceDuesDescriptions.GetBalanceDuesDesc(companyId, data.BalanceDue.InvoiceType);
                    if (bdDescs != null && bdDescs.Any())
                    {
                        data.BDDescriptions = new SelectList(bdDescs, "Description", "BDDescription");
                    }

                    data.BalanceDuesItems = new List<BalanceDues_Item>();

                    if (!IsNewBal)
                    {
                        data.BalanceDuesItems.AddRange(Common.BOLHeader.GetBalanceDueItems(bolNo, companyId));
                    }
                    else
                    {
                        data.BalanceDuesItems.Add(bdItem);
                    }
                    var limit = 10000;

                    if (bolHeaderDetails != null && !string.IsNullOrWhiteSpace(bolHeaderDetails.BOLNo))
                    {
                        data.InvoiceSummary = Common.InvoiceSummary.GetInvoiceSummary(bolHeaderDetails.BOLNo).FirstOrDefault();
                        data.Modes = Common.BOLHeader.GetAllModes().Take(limit).ToList();
                        data.PodList = Common.BOLHeader.GetDistinctPOD().Take(limit).ToList();
                        data.PolList = Common.BOLHeader.GetDistinctPOL().Take(limit).ToList();
                        data.OriginLocList = Common.BOLHeader.GetDistinctOrigin().Take(limit).ToList();
                        data.DestinationLocList = Common.BOLHeader.GetDistinctDestination().Take(limit).ToList();
                        data.Participants = Common.BOLHeader.GetAllParticipants().Take(limit).ToList();
                        data.ChargeList = Common.BDOtherChargesCodes.GetDistinctChargeCodes(companyId, true).DistinctBy(x => x.ChargeCode).Take(limit).ToList();
                        data.BDErrorCodes = Common.BDErrorCodes.GetDistinctErrorCodes(Common.GetActiveClientId(), false, "", data.BalanceDue.InvoiceType).Take(limit).ToList();
                        data.Containers = Common.BOLContainers.GetDistinctType();
                        data.ContainerSizes = Common.BOLContainers.GetDistinctSize();
                        data.BalanceDuesOtherCharges = new List<BalanceDues_OtherCharges>();

                        if (!IsNewBal)
                        {
                            data.BalanceDuesOtherCharges.AddRange(Common.BalanceDuesOtherCharges.GetBalanceDuesOtherCharges(bolNo) ?? new List<Arg.DataModels.BalanceDues_OtherCharges>());
                        }
                        else
                        {
                            data.PashaBalanceDuesOtherCharges = new List<Arg.DataModels.BOLChargesModel>();
                            var data1 = Common.BOLCharges.GetPashaBalanceDuesOtherChargesWithDesc(bolNo);
                            data.PashaBalanceDuesOtherCharges.AddRange(data1);
                            foreach (var item in data.PashaBalanceDuesOtherCharges)
                            {
                                var balanceDuesOtherChargesItem = new BalanceDues_OtherCharges
                                {
                                    TariffRefNo = "980000-006",
                                    BOLNo = item.BOLNo,
                                    Description = item.ChargeDescription,
                                    ChargeCode = item.ChargeCode,
                                    AmountDue = Convert.ToDecimal(item.USAmount)
                                };
                                data.BalanceDuesOtherCharges.Add(balanceDuesOtherChargesItem);
                            }
                        }

                        if (data.BalanceDuesOtherCharges.Count() == 0)
                        {
                            data.BalanceDuesOtherCharges.Add(new BalanceDues_OtherCharges { });
                        }
                    }
                }
                else if (Arg.DataAccess.ActiveClient.Info.DBName.Contains("ClientAgility"))
                {
                    data.Message = Arg.Core.ActiveObjects.BolIsBeingEditedInfo(bolNo);
                    Arg.Core.ActiveObjects.SetBolBeingEdited(bolNo, "", Common.CurrentUserName);
                    data.BolNo = bolNo;
                    data.CommonObjects.TopHeading = "Add Balance Due";

                    var companyId = Arg.DataAccess.ActiveClient.Info.CompanyId;

                    var currencies = Common.salesBOLCharges.GetDistinctCurrency();
                    data.Currencies = new SelectList(currencies, "InvoiceCurrency", "InvoiceCurrency");

                    var bolChargeInfo = Common.salesBOLCharges.GetBOLCharge(bolNo);
                    if (bolChargeInfo != null && !string.IsNullOrWhiteSpace(bolChargeInfo.JobNumber))
                    {
                        data.Currency = bolChargeInfo.InvoiceCurrency;
                    }

                    var bolCustomers = Common.AgilityBOLHeader.GetBOLCustomers();
                    if (bolCustomers != null && bolCustomers.Any())
                    {
                        data.AgilityCustomersList = bolCustomers;
                    }

                    if (IsNewBal)
                    {
                        var cust = Common.BalanceDues.GetCustomerBalanceDues(bolNo, companyId);
                        data.AgilityCustomersList = data.AgilityCustomersList.Where(x => !cust.Any(i => i.CustomerId == x.participantid)).ToList();
                    }

                    data.BalanceDue = new BalanceDue();
                    data.BalanceDue.InvoiceType = "BOL";

                    var shippBolRef = Common.AgilityBOLHeader.GetShipper(bolNo);
                    if (shippBolRef != null && !string.IsNullOrWhiteSpace(shippBolRef.ConsignmentID))
                    {
                        data.ShippersRefNo = shippBolRef.ConsignmentID;
                    }

                    var consiBolRef = Common.AgilityBookingHeaders.GetConsigneeReference(bolNo);
                    if (consiBolRef != null && !string.IsNullOrWhiteSpace(consiBolRef.ConsignmentID))
                    {
                        data.ConsigneeRefNo = consiBolRef.ConsignmentID;
                    }

                    //BolContainers
                    var bolItems = Common.ContainerDetailsImpl.GetBOLItemDetail(bolNo);

                    var bdItem = new BalanceDues_Item();
                    if (bolItems != null && bolItems.Any())
                    {
                        bdItem.Container = bolItems[0].UnitNumber;
                        bdItem.ContainerType = bolItems[0].UnitType;
                        bdItem.ContainerSize = bolItems[0].UnitType;
                        bdItem.Weight = Convert.ToString(bolItems[0].UnitMaxGrossWeight);
                        bdItem.WeightUnit = "Kilos";
                        bdItem.Measure = Convert.ToString(bolItems[0].ActUnitVolume);
                        bdItem.MeasureUnit = "Cubic Meters";
                        bdItem.Hazmat = Convert.ToBoolean(Convert.ToInt32(bolItems[0].HazmatFlag));
                        bdItem.TariffRef = IsNewBal ? "" : Common.AgilityBookingHeaders.GetTeriffRef(bolNo);
                        bdItem.Commodity = "";
                        bdItem.CommodityDesc = bolItems[0].GoodsDescription.Length > 80 ? bolItems[0].GoodsDescription.Substring(0, 80) : bolItems[0].GoodsDescription;
                        bdItem.AmountDue = Common.AgilityBookingHeaders.GetAmountDue(bolNo);
                    }

                    //Payment
                    var payment = new BalanceDues_Payments();
                    if (!IsNewBal)
                    {
                        payment = Common.BalanceDuesPayments.GetBalanceDuesPayment(0, bolNo) ?? new BalanceDues_Payments();
                    }

                    data.AmountPaid = payment.PaymentAmount;
                    data.PaymentId = payment.PaymentId;

                    //BolHeader
                    var bolHeaderDetails = Common.AgilityBOLHeader.GetBOLHeader(bolNo) ?? new Arg.Agility.DataModels.BOLHeaders();
                    //BookingHeader
                    var bolBookingHeaderDetails = Common.AgilityBookingHeaders.GetBookingInfo(bolNo) ?? new Arg.Agility.DataModels.BookingHeaders();
                    var balDue = new BalanceDue();

                    if (!IsNewBal)
                    {
                        balDue = Common.BalanceDues.GetBalanceDue(balanceId, bolNo, companyId) ?? new BalanceDue();
                        data.CustomerId = balDue.CustomerId;
                    }
                    data.Description = balDue.BDDescription;
                    data.BDDescription = string.IsNullOrEmpty(balDue.BDDescription) ? "" : balDue.BDDescription.Split(new[] { Environment.NewLine }, StringSplitOptions.None)[0];

                    if (balDue.InvoiceType != null)
                    {
                        data.BalanceDue.InvoiceType = balDue.InvoiceType;
                    }

                    var errCodes = Common.BDErrorCodes.GetDistinctErrorCodes(companyId, false, "", data.BalanceDue.InvoiceType);

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

                    var bdDescs = Common.BalanceDuesDescriptions.GetBalanceDuesDesc(companyId, data.BalanceDue.InvoiceType);
                    if (bdDescs != null && bdDescs.Any())
                    {
                        data.BDDescriptions = new SelectList(bdDescs, "Description", "BDDescription");
                    }
                    data.BalanceDuesItems = new List<Arg.DataModels.BalanceDues_Item>();

                    if (!IsNewBal)
                    {
                        data.BalanceDuesItems.AddRange(Common.BOLHeader.GetBalanceDueItems(bolNo, companyId));
                    }
                    else
                    {
                        data.BalanceDuesItems.Add(bdItem);
                    }
                    var limit = 10000;

                    if (bolBookingHeaderDetails != null && !string.IsNullOrWhiteSpace(bolBookingHeaderDetails.JobNumber))
                    {
                        data.AgilityBookingHeaderDetails = bolBookingHeaderDetails;
                        if (data.AgilityBookingHeaderDetails != null && data.AgilityBookingHeaderDetails.DepartureDate != null)
                        {
                            data.AgilityBookingHeaderDetails.DepartureDateString = data.AgilityBookingHeaderDetails.DepartureDate.Value.ToString("MM-dd-yyyy");
                        }
                    }
                    data.AgilityBookingHeaderDetails = bolBookingHeaderDetails;
                    if (bolHeaderDetails != null)
                    {
                        data.AgilityBolHeaderDetails = bolHeaderDetails;
                    }

                    data.ServiceMovementType = Common.AgilityBOLHeader.GetServiceMovementType().Take(limit).ToList();
                    data.Origin = Common.AgilityBOLHeader.GetDistinctOrigin().Take(limit).ToList();
                    data.PortOfExit = Common.AgilityBOLHeader.GetPortOfExit().Take(limit).ToList();
                    data.PortOfEntry = Common.AgilityBOLHeader.GetPortofEntry().Take(limit).ToList();
                    data.Destination = Common.AgilityBOLHeader.GetDestination().Take(limit).ToList();
                    data.ChargeList = Common.BDOtherChargesCodes.GetDistinctChargeCodes(companyId, true).DistinctBy(x => x.ChargeCode).Take(limit).ToList();
                    data.BDErrorCodes = Common.BDErrorCodes.GetDistinctErrorCodes(Common.GetActiveClientId(), false, "", data.BalanceDue.InvoiceType).Take(limit).ToList();
                    data.Containers = new List<BOLContainers>();
                    data.ContainerSizes = new List<BOLContainers>();
                    var containers = Common.ContainerDetailsImpl.GetDistinctType();

                    foreach (var item in containers)
                    {
                        var listItem = new BOLContainers
                        {
                            Type = item.UnitType
                        };
                        data.Containers.Add(listItem);
                        data.ContainerSizes.Add(listItem);
                    }
                    data.BalanceDuesOtherCharges = new List<BalanceDues_OtherCharges>();

                    if (!IsNewBal)
                    {
                        data.BalanceDuesOtherCharges.AddRange(Common.BalanceDuesOtherCharges.GetBalanceDuesOtherCharges(bolNo) ?? new List<BalanceDues_OtherCharges>());
                    }
                    else
                    {
                        data.AgilityBalanceDuesOtherCharges = new List<Arg.Agility.DataModels.SalesInvoices>();
                        data.AgilityBalanceDuesOtherCharges.AddRange(Common.salesBOLCharges.GetAgilityBalanceDuesOtherChargesWithDesc(bolNo) ?? new List<Arg.Agility.DataModels.SalesInvoices>());
                        foreach (var item in data.AgilityBalanceDuesOtherCharges)
                        {
                            var balanceDuesOtherChargesItem = new BalanceDues_OtherCharges
                            {
                                //ItemId = item.ItemId,
                                TariffRefNo = "",
                                BOLNo = item.JobNumber,
                                Description = item.ChargeDescription,
                                ChargeCode = item.ChargeCode,
                                AmountDue = string.IsNullOrEmpty(item.ChargeValue) ? 0 : Decimal.Parse(item.ChargeValue)
                            };
                            data.BalanceDuesOtherCharges.Add(balanceDuesOtherChargesItem);
                        }
                    }
                    if (data.BalanceDuesOtherCharges.Count() == 0)
                    {
                        data.BalanceDuesOtherCharges.Add(new BalanceDues_OtherCharges { });
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
        public void DeleteBdItem(int itemId)
        {
            Common.BOLHeader.DeleteBDItem(itemId);
        }

        [Authorize]
        public void DeleteBolOtherCharges(int itemId)
        {
            Common.BalanceDuesOtherCharges.DeleteBolOtherCharges(itemId);
        }

        [Authorize]
        [HttpGet]
        public virtual JsonResult GetBolItemCharges(string bolNo, bool oceanCharges)
        {
            var chargeDesc = Common.BOLCharges.GetBOLOceanCharges(bolNo, oceanCharges);
            return Json(chargeDesc);
        }

        [Authorize]
        [HttpGet]
        public virtual JsonResult GetAmountPaid(string bolNo, string custId)
        {
            decimal amountPaid = Common.ARCash.GetAmountPaid(bolNo, custId);
            return Json(amountPaid);
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
                var region = "US";
                var invoiceDate = DateTime.Now.Date;//.ToString("MM/dd/yyyy");
                var errCodes = Common.BDErrorCodes.GetDistinctErrorCodes(companyId);


                var balDue = new BalanceDue(); //MUST LOAD FROM DATABASE FIRST, FOR UPDATE EXISTING
                var payment = new BalanceDues_Payments();

                if (Arg.DataAccess.ActiveClient.Info.DBName.Contains("ClientPasha"))
                {
                    var bolExecDate = Convert.ToDateTime(data.BolHeaderDetails.ActualDepartureDate);

                    if (data.BalanceDuesItems != null && data.BalanceDuesItems.Any())
                    {
                        List<BalanceDues_Item> dues = data.BalanceDuesItems;

                        foreach (var due in dues)
                        {
                            due.Bol = data.BolHeaderDetails.BOLNo;
                            due.BalanceDueInvoiceDate = Convert.ToDateTime(invoiceDate);
                            due.BookingId = data.BolHeaderDetails.BookingID;
                            due.CompanyId = companyId;
                            due.CustomerId = data.CustomerId;
                            due.Region = region;
                        }

                        Common.BOLHeader.DeleteBDItem(companyId, data.BolHeaderDetails.BOLNo, data.CustomerId, region);
                        Common.BOLHeader.SaveBalanceDueItems(dues);
                    }

                    if (data.BalanceDuesOtherCharges != null && data.BalanceDuesOtherCharges.Any())
                    {
                        List<BalanceDues_OtherCharges> charges = data.BalanceDuesOtherCharges;
                        foreach (var charge in charges)
                        {
                            charge.BOLNo = data.BolHeaderDetails.BOLNo;
                            charge.CustomerId = data.CustomerId;
                            charge.CompanyId = companyId;
                            charge.Region = region;
                        }
                        Common.BalanceDuesOtherCharges.DeleteBolOtherCharges(companyId, data.BolHeaderDetails.BOLNo, data.CustomerId, region);
                        Common.BalanceDuesOtherCharges.SaveBDOtherCharges(charges);
                    }

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
                    balDue.Bol = data.BolHeaderDetails.BOLNo;
                    balDue.BookingId = data.BolHeaderDetails.BookingID;
                    balDue.BolExecutionDate = bolExecDate;
                    balDue.CustomerLocationCode = Common.ActiveClient.NameContains("Pasha") ? "" : data.BolHeaderDetails.OriginLocationCode;

                    if (balDue.BalanceId <= 0 || balDue.BalanceDueInvoiceDate < DateTime.Now.AddYears(-10))
                    {
                        balDue.BalanceDueInvoiceDate = null;
                    }

                    if (balDue.BalanceId <= 0)
                    {
                        balDue.BalanceDueInvoice = "";
                    }

                    balDue.CustomerId = data.CustomerId;
                    balDue.PortOfLoading = data.BolHeaderDetails.POL;
                    balDue.PortOfDischarge = data.BolHeaderDetails.POD;
                    balDue.MoveType = data.BolHeaderDetails.Mode;//check
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
                    balDue.Vessel = data.BolHeaderDetails.Vessel;
                    balDue.Voyage = data.BolHeaderDetails.Voyage;
                    balDue.Quote = data.Quote ?? "";
                    balDue.ConsigneeRefNumber = data.ConsigneeRefNo;
                    balDue.ShippersRefNumber = data.ShippersRefNo;
                    balDue.PayorRefNumber = data.PayorRefNo;
                    balDue.InvoiceType = data.BalanceDue.InvoiceType ?? "BOL";
                    balDue.Currency = Common.ActiveClient.NameContains("Pasha") ? "USD" : "";
                    balDue.OriginLocationCode = data.BolHeaderDetails.OriginLocationCode;
                    balDue.DestinationLocationCode = data.BolHeaderDetails.DestinationLocationCode;
                    Common.BalanceDues.SaveBalanceDue(balDue);

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
                    payment.CustomerLocationCode = data.BolHeaderDetails.OriginLocationCode;
                    payment.BookingID = data.BolHeaderDetails.BookingID;
                    payment.BOLNo = data.BolHeaderDetails.BOLNo;
                    payment.BOLExecutionDate = bolExecDate;
                    payment.BalanceDueInvoiceDate = Convert.ToDateTime(invoiceDate);
                    payment.Payor = data.CustomerId;
                    payment.PaymentAmount = data.AmountPaid;
                    payment.Currency = data.Currency;
                    Common.BalanceDuesPayments.DeleteBDPay(data.CustomerId, data.BolHeaderDetails.BOLNo, companyId, region);
                    Common.BalanceDuesPayments.SaveBalanceDuesPayment(payment);

                    var partipant = Common.Participants.GetParticipant(data.CustomerId) ?? new Arg.DataModels.Participants();

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
                    customer.Address1 = partipant.Address1;
                    customer.Address2 = partipant.Address2;
                    customer.City = partipant.City;
                    customer.State = partipant.State;
                    customer.Country = partipant.Country;
                    customer.ZipCode = partipant.Zip;
                    customer.LastUpdated = DateTime.Now;
                    customer.CustomerName = partipant.ParticipantName;
                    customer.CustomerId = data.CustomerId;
                    Common.Customers.SaveCustomer(customer);
                }
                else if (Arg.DataAccess.ActiveClient.Info.DBName.Contains("ClientAgility"))
                {
                    if (!string.IsNullOrEmpty(data.AgilityBookingHeaderDetails.DepartureDateString))
                    {
                        data.AgilityBookingHeaderDetails.DepartureDate = DateTime.ParseExact(data.AgilityBookingHeaderDetails.DepartureDateString, "MM-dd-yyyy", null);
                    }

                    var jobconfirmationdate = Convert.ToDateTime(data.AgilityBookingHeaderDetails.DepartureDate);

                    if (data.BalanceDuesItems != null && data.BalanceDuesItems.Any())
                    {
                        List<BalanceDues_Item> dues = data.BalanceDuesItems;

                        foreach (var due in dues)
                        {
                            due.Bol = data.AgilityBookingHeaderDetails.JobNumber;
                            due.BalanceDueInvoiceDate = Convert.ToDateTime(invoiceDate);
                            due.BookingId = data.AgilityBookingHeaderDetails.BookingReference;
                            due.CompanyId = companyId;
                            due.CustomerId = data.CustomerId;
                            due.Region = region;

                            var bolItems = Common.ContainerDetailsImpl.GetBOLItemDetail(data.AgilityBookingHeaderDetails.JobNumber);
                            if (bolItems != null && bolItems.Any())
                            {
                                due.ContainerType = bolItems[0].UnitType;
                                due.ContainerSize = bolItems[0].UnitType;
                                due.Weight = Convert.ToString(bolItems[0].ActUnitGrossW);
                                due.TariffRef = data.BalanceDuesItems[0].TariffRef;
                                due.Commodity = data.BalanceDuesItems[0].Commodity;
                                due.CommodityDesc = bolItems[0].GoodsDescription.Length > 80 ? bolItems[0].GoodsDescription.Substring(0, 80) : bolItems[0].GoodsDescription;
                            }
                        }
                        Common.BOLHeader.DeleteBDItem(companyId, data.AgilityBookingHeaderDetails.JobNumber, data.CustomerId, region);
                        Common.BOLHeader.SaveBalanceDueItems(dues);
                    }

                    if (data.BalanceDuesOtherCharges != null && data.BalanceDuesOtherCharges.Any())
                    {
                        List<BalanceDues_OtherCharges> charges = data.BalanceDuesOtherCharges;

                        foreach (var charge in charges)
                        {
                            charge.BOLNo = data.AgilityBookingHeaderDetails.JobNumber;
                            charge.CustomerId = data.CustomerId;
                            charge.CompanyId = companyId;
                            charge.Region = region;
                        }

                        Common.BalanceDuesOtherCharges.DeleteBolOtherCharges(companyId, data.AgilityBookingHeaderDetails.JobNumber, data.CustomerId, region);
                        Common.BalanceDuesOtherCharges.SaveBDOtherCharges(charges);
                    }

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
                    balDue.Bol = data.AgilityBookingHeaderDetails.JobNumber;
                    balDue.BookingId = data.AgilityBookingHeaderDetails.BookingReference;
                    balDue.BolExecutionDate = jobconfirmationdate;
                    balDue.CustomerLocationCode = Common.ActiveClient.NameContains("Agility") ? "" : data.AgilityBolHeaderDetails.Destination;

                    if (balDue.BalanceId <= 0 || balDue.BalanceDueInvoiceDate < DateTime.Now.AddYears(-10))
                    {
                        balDue.BalanceDueInvoiceDate = null;
                    }

                    if (balDue.BalanceId <= 0)
                    {
                        balDue.BalanceDueInvoice = "";
                    }

                    balDue.PortOfLoading = data.AgilityBookingHeaderDetails.PortOfExit;
                    balDue.PortOfDischarge = data.AgilityBookingHeaderDetails.PortOfEntry;
                    balDue.MoveType = data.AgilityBookingHeaderDetails.ServiceMovementType;
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
                    balDue.Vessel = data.AgilityBookingHeaderDetails.Vessel1;
                    balDue.Voyage = data.AgilityBookingHeaderDetails.Voyage1;
                    balDue.Quote = data.Quote ?? "";
                    balDue.ConsigneeRefNumber = data.ConsigneeRefNo;
                    balDue.ShippersRefNumber = data.ShippersRefNo;
                    balDue.PayorRefNumber = data.PayorRefNo;
                    balDue.InvoiceType = data.BalanceDue.InvoiceType ?? "BOL";
                    balDue.Currency = Common.ActiveClient.NameContains("Agility") ? "USD" : "";
                    balDue.OriginLocationCode = data.AgilityBolHeaderDetails.Origin;
                    balDue.DestinationLocationCode = data.AgilityBolHeaderDetails.Destination;
                    Common.BalanceDues.SaveBalanceDue(balDue);

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
                    payment.CustomerLocationCode = data.AgilityBolHeaderDetails.Origin;
                    payment.BookingID = data.AgilityBookingHeaderDetails.BookingReference;
                    payment.BOLNo = data.AgilityBookingHeaderDetails.JobNumber;
                    payment.BOLExecutionDate = jobconfirmationdate;
                    payment.BalanceDueInvoiceDate = Convert.ToDateTime(invoiceDate);
                    payment.Payor = data.CustomerId;
                    payment.PaymentAmount = data.AmountPaid;
                    payment.Currency = Common.ActiveClient.NameContains("Agility") ? "USD" : "";

                    Common.BalanceDuesPayments.DeleteBDPay(data.CustomerId, data.AgilityBookingHeaderDetails.JobNumber, companyId, region);
                    Common.BalanceDuesPayments.SaveBalanceDuesPayment(payment);
                    var partipant = Common.Participants.GetParticipant(data.CustomerId) ?? new Arg.DataModels.Participants();

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
                    customer.CustomerName = partipant.ParticipantName;
                    customer.Region = region;
                    customer.Address1 = partipant.Address1;
                    customer.Address2 = partipant.Address2;
                    customer.City = partipant.City;
                    customer.State = partipant.State;
                    customer.Country = partipant.Country;
                    customer.ZipCode = partipant.Zip;
                    customer.LastUpdated = DateTime.Now;
                    customer.CustomerName = partipant.ParticipantName;
                    customer.CustomerId = data.CustomerId;
                    Common.Customers.SaveCustomer(customer);
                }
                else
                {
                    var regionInfo = Common.Regions.GetRegion(0, companyId);
                    if (regionInfo != null && regionInfo.RegionId > 0)
                    {
                        region = regionInfo.Region;
                    }
                }
                var resItem = Common.ResearchItems.GetResearchItem(0, Common.GetActiveClientId(), data.BolHeaderDetails.BOLNo, region);
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


        [AuthorizeUser]
        public IActionResult PullForResearch(string bolNo)
        {
            var data = new PullForResearch();
            try
            {
                if (Arg.DataAccess.ActiveClient.Info.DBName.Contains("ClientPasha"))
                {
                    var bolDetails = Common.BOLHeader.GetBOLHeader(bolNo);
                    if (bolDetails != null && !string.IsNullOrWhiteSpace(bolDetails.BOLNo))
                    {
                        data.BolDetails = bolDetails;
                    }
                    var reasonCodes = Common.RSReasonCodes.GetDistinctReasonCodes();
                    data.ReasonCodes = new SelectList(reasonCodes, "ReasonCode", "ReasonCode");
                }
                else if (Arg.DataAccess.ActiveClient.Info.DBName.Contains("ClientAgility"))
                {
                    var jobDetails = Common.AgilityBOLHeader.GetBOLHeader(bolNo);
                    if (jobDetails != null && !string.IsNullOrWhiteSpace(jobDetails.JobNumber))
                    {
                        data.JobDetails = jobDetails;
                    }
                    var reasonCodes = Common.RSReasonCodes.GetDistinctReasonCodes();
                    data.ReasonCodes = new SelectList(reasonCodes, "ReasonCode", "ReasonCode");
                }
                else if (Arg.DataAccess.ActiveClient.Info.DBName.Contains("ClientHellmann"))
                {
                    var ShipmentDetails = Common.ShipmentJournal.GetShipment(bolNo);
                    if (ShipmentDetails != null && !string.IsNullOrWhiteSpace(ShipmentDetails.Shipment_No))
                    {
                        data.ShipmentDetails = ShipmentDetails;
                        data.IsShipmentDetails = true;
                    }
                    var reasonCodes = Common.RSReasonCodes.GetDistinctReasonCodes();
                    data.ReasonCodes = new SelectList(reasonCodes, "ReasonCode", "ReasonCode");
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
        public JsonResult SaveResearchDetails(PullForResearch pullForResearch)
        {
            try
            {
                if (pullForResearch != null && pullForResearch.IsShipmentDetails)
                {
                    var bolHeaderDetails = Common.ShipmentJournal.GetShipment(pullForResearch.BOLNo) ?? new ShipmentJournal();
                    var region = "";
                    //var regionInfo = ARG.Common.Regions.GetRegion(0, ARG.Common.GetActiveClientId());
                    if (bolHeaderDetails != null && !string.IsNullOrEmpty(bolHeaderDetails.Region))
                    {
                        region = bolHeaderDetails.Region;
                    }
                       
                    var resItem = new Arg.DataModels.ResearchItems
                    {
                        Region = region,
                        BOL = pullForResearch.BOLNo,
                        ResearchReasonCode = pullForResearch.PullReasonCode,
                        Status = "Open",
                        Comments = pullForResearch.ResearchComments,
                        LastModified = DateTime.Now,
                        CompanyId = Arg.DataAccess.ActiveClient.Info.CompanyId,
                        //BookingId = bolHeaderDetails.BookingID,
                        BookingId = "0", //need to check
                        BolExecutionDate = Convert.ToDateTime(bolHeaderDetails.Shipment_Date),
                        RevenueAnalystAuditor = Common.CurrentUserId,
                        LastModifiedBy = Common.CurrentUserId
                    };
                    Common.ResearchItems.SaveResearchItem(resItem);
                    if (resItem.ResearchId > 0)
                    {
                        Common.ActivityStats.SaveActivityStats(Arg.DataAccess.ActivityStatsImpl.EnumActions.Saved, resItem.CompanyId, "Research Items; BOL#: " + resItem.BOL + " ; Client: " + Arg.DataAccess.ActiveClient.Info.Name);
                    }
                }
                else
                {
                    var bolHeaderDetails = Common.BOLHeader.GetBOLHeader(pullForResearch.BOLNo) ?? new BOLHeader();
                    var region = "";
                    var regionInfo = Common.Regions.GetRegion(0, Common.GetActiveClientId());

                    if (regionInfo != null && regionInfo.RegionId > 0)
                    {
                        region = regionInfo.Region;
                    }
                       
                    var resItem = new Arg.DataModels.ResearchItems
                    {
                        Region = region,
                        BOL = pullForResearch.BOLNo,
                        ResearchReasonCode = pullForResearch.PullReasonCode,
                        Status = "Open",
                        Comments = pullForResearch.ResearchComments,
                        LastModified = DateTime.Now,
                        CompanyId = Arg.DataAccess.ActiveClient.Info.CompanyId,
                        BookingId = bolHeaderDetails.BookingID,
                        BolExecutionDate = Convert.ToDateTime(bolHeaderDetails.ActualDepartureDate),
                        RevenueAnalystAuditor = Common.CurrentUserId,
                        LastModifiedBy = Common.CurrentUserId
                    };
                    Common.ResearchItems.SaveResearchItem(resItem);
                    if (resItem.ResearchId > 0)
                    {
                        Common.ActivityStats.SaveActivityStats(Arg.DataAccess.ActivityStatsImpl.EnumActions.Saved, resItem.CompanyId, "Research Items; BOL#: " + resItem.BOL + " ; Client: " + Arg.DataAccess.ActiveClient.Info.Name);
                    }
                }
                //var balDue = Common.BalanceDues.GetBalanceDue(0, pullForResearch.BOLNo);

                return Json("Research Item saved!");
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
                return Json(ex.ToString());
            }
        }

        [AuthorizeUser]
        public IActionResult AuditorPlaybook(int? queryId, int? playId)
        {
            var playbooks = new AuditorPlaybooks();
            try
            {
                playbooks.AuditorPlaybookList = Common.AuditorPlaybooks.GetAuditorPlaybooks();

                foreach (var item in playbooks.AuditorPlaybookList)
                {
                    if (!string.IsNullOrWhiteSpace(item.QueryJson))
                    {
                        var filter = JObject.Parse(@"" + item.QueryJson + "");

                        foreach (var kv in filter)
                        {
                            if (!string.IsNullOrWhiteSpace(kv.Value.ToString()) && kv.Value.ToString() != "0" && kv.Value.ToString() != "False" && kv.Value.ToString() != "1/1/0001 12:00:00 AM")
                                item.FielterField += kv.Key + ":" + kv.Value + "<br/>";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex.ToString());
            }
            return View(playbooks);
        }

        [Authorize]
        public JsonResult AddToPlaybook(int? queryId)
        {
            try
            {
                var _queryId = Convert.ToInt32(queryId);
                if (_queryId > 0)
                {
                    var qr = new Arg.DataAccess.QueryResultsImpl().GetQueryResults(_queryId);
                    if (qr != null)
                    {
                        var searchOptions = Newtonsoft.Json.JsonConvert.DeserializeObject<Arg.DataModels.SearchOptions>(qr.QueryJson);
                        var region = Common.Regions.GetRegion(0, searchOptions.CompanyId);//temporary
                        if (region != null && region.RegionId > 0)
                            searchOptions.Region = region.Region;
                        var result = new Arg.DataModels.AuditorPlaybook
                        {
                            CompanyID = searchOptions.CompanyId,
                            Region = searchOptions.Region,
                            AuditingScreenFilters = qr.QueryJson,
                            Status = Convert.ToInt32(Common.Contants.StatusTypeEnum.InProcess),
                            StatusDate = DateTime.Now,
                            Priority = Convert.ToInt32(Common.Contants.PriorityTypeEnum.High),
                            UserID = Common.CurrentUserId, //Common.ActiveClient.GetUserFLName(),
                            Comments = "",
                            QueryId = _queryId
                        };
                        Common.AuditorPlaybooks.SaveAuditorPlaybook(result);
                        if (result.PlayID > 0)
                        {
                            // return Json("Saved!", JsonRequestBehavior.AllowGet);
                            return Json(result);
                        }
                    }
                }
                return Json("Error!");
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex.ToString());
                return Json("Error!");
            }
        }

        [Authorize]
        public void DeletePlaybook(int playId, int companyId)
        {
            try
            {
                int deleteStatus = Common.AuditorPlaybooks.DeleteAuditorPlaybook(playId, companyId);
                if (deleteStatus > 0)
                {
                    var result = new PlaybookComments
                    {
                        PlayId = playId,
                        Comment = "Playbook was deleted by user " + Common.CurrentUserId + " on " + DateTime.Now,
                        AddedBy = Common.CurrentUserId,
                        AddedOn = DateTime.Now
                    };
                    Common.PlaybookComment.SavePlaybookComment(result);
                }
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex.ToString());
            }
        }

        [Authorize]
        [HttpGet]
        public IActionResult UpdatePlaybookStatus(int playId, int companyId)
        {
            var playbooks = new AuditorPlaybooks();
            try
            {
                playbooks.AuditorPlaybookDetail = new Arg.DataModels.AuditorPlaybook();
                playbooks.AuditorPlaybookDetail = Common.AuditorPlaybooks.GetAuditorPlaybook(playId, companyId);
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex.ToString());
            }
            return View(playbooks);
        }

        [Authorize]
        [HttpPost]
        public JsonResult UpdatePlaybookStatus(AuditorPlaybooks playbookModel)
        {
            try
            {
                if (playbookModel != null)
                {
                    var playbookDetail = Common.AuditorPlaybooks.GetAuditorPlaybook(playbookModel.AuditorPlaybookDetail.PlayID, playbookModel.AuditorPlaybookDetail.CompanyID);
                    int status = playbookDetail.Status;
                    var title = playbookDetail.Title;
                    int priority = playbookDetail.Priority;
                    playbookDetail.Status = playbookModel.AuditorPlaybookDetail.Status;
                    playbookDetail.Priority = playbookModel.AuditorPlaybookDetail.Priority;
                    playbookDetail.Title = playbookModel.AuditorPlaybookDetail.Title;
                    playbookDetail.StatusDate = DateTime.Now;
                    playbookDetail.UserID = Common.CurrentUserId;
                    Common.AuditorPlaybooks.SaveAuditorPlaybook(playbookDetail);
                    if (status != playbookModel.AuditorPlaybookDetail.Status)
                    {
                        playbookDetail.Comment = "User changed Status from  " + (Common.Contants.StatusTypeEnum)Enum.Parse(typeof(Common.Contants.StatusTypeEnum), status.ToString()) + " to " + (Common.Contants.StatusTypeEnum)Enum.Parse(typeof(Common.Contants.StatusTypeEnum), playbookDetail.Status.ToString());
                        AddPlayBookComment(playbookModel.AuditorPlaybookDetail.PlayID, playbookDetail.Comment);
                    }
                    if (priority != playbookModel.AuditorPlaybookDetail.Priority)
                    {
                        playbookDetail.Comment = "User changed Priority from  " + (Common.Contants.PriorityTypeEnum)Enum.Parse(typeof(Common.Contants.PriorityTypeEnum), priority.ToString()) + " to " + (Common.Contants.PriorityTypeEnum)Enum.Parse(typeof(Common.Contants.PriorityTypeEnum), playbookDetail.Priority.ToString());
                        AddPlayBookComment(playbookModel.AuditorPlaybookDetail.PlayID, playbookDetail.Comment);
                    }
                    if (title != playbookModel.AuditorPlaybookDetail.Title)
                    {
                        if (!string.IsNullOrWhiteSpace(title))
                        {
                            playbookDetail.Comment = "User changed Title from  " + title + " to " + playbookDetail.Title;
                        }
                        else
                        {
                            playbookDetail.Comment = "User changed Title to " + playbookDetail.Title;
                        }
                        AddPlayBookComment(playbookModel.AuditorPlaybookDetail.PlayID, playbookDetail.Comment);
                    }

                    return Json(playbookDetail);
                    //return RedirectToAction("AuditorPlaybook", "BOL");
                }
                return Json("Error");
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex.ToString());
                return Json("Error");
            }
            // return RedirectToAction("AuditorPlaybook", "BOL");
        }

        public void AddPlayBookComment(int playId, string Comment)
        {
            try
            {
                var result = new PlaybookComments
                {
                    PlayId = playId,
                    Comment = Comment,
                    AddedBy = Common.CurrentUserId,
                    AddedOn = DateTime.Now
                };
                Common.PlaybookComment.SavePlaybookComment(result);
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex.ToString());
            }
        }

        [Authorize]
        [HttpPost]
        public JsonResult SavePlaybookComments(PlaybookComment playbookComment)
        {
            try
            {
                var playbookInfo = Common.AuditorPlaybooks.GetAuditorPlaybook(playbookComment.PlayId, 0);
                var result = new PlaybookComments
                {
                    PlayId = playbookComment.PlayId,
                    Comment = playbookComment.PlaybookCommentsDetail.Comment,
                    AddedBy = Common.CurrentUserId,
                    AddedOn = DateTime.Now
                };
                Common.PlaybookComment.SavePlaybookComment(result);
                if (result.CommentId > 0)
                {
                    result.CollectorName = Common.ActiveClient.GetUserFLName(result.AddedBy);

                    return Json(result);
                }
                return Json("Playbook Comment cannot be empty!");
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
                return Json(ex.ToString());
            }
        }

        public PartialViewResult LoadPlaybookComments(int playId)
        {
            var data = new PlaybookComment();
            try
            {
                data.PlayId = playId;
                var playbookInfo = Common.AuditorPlaybooks.GetAuditorPlaybook(playId, 0);
                if (playbookInfo != null && playbookInfo.PlayID > 0)
                {
                    data.AuditorPlaybookDetails = playbookInfo;
                    var collectionComments = Common.PlaybookComment.GetPlaybookComments(playId).OrderByDescending(x => x.AddedOn).ToList();
                    if (collectionComments != null && collectionComments.Any())
                    {
                        data.PlaybookCommentsList = collectionComments;
                    }
                }
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex.ToString());
            }
            return PartialView(data);
        }

        [Authorize]
        [HttpGet]
        public IActionResult AddSqlQuery(int? queryId)
        {
            var data = new SqlQuery();
            try
            {
                if (queryId != null)
                {
                    var _queryId = Convert.ToInt32(queryId);
                    var sqlqueryinfo = Common.QueryResults.GetQueryResults(_queryId);
                    data.Query = sqlqueryinfo.SqlQuery;
                    data.QueryId = _queryId;
                }
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex.ToString()); throw;
            }
            return View(data);
        }

        [Authorize]
        [HttpPost]
        public JsonResult AddSqlQuery(SqlQuery sqlquery)
        {
            try
            {
                if (sqlquery != null)
                {
                    var query = Common.BOLHeader.GetResults(sqlquery.Query, 1);

                    if (query.ResultCount > 0)
                    {
                        if (sqlquery.QueryId <= 0)
                        {
                            var result = new QueryResults
                            {
                                VARBINARY = new byte[] { },
                                QueryJson = "",
                                SqlQuery = sqlquery.Query
                            };
                            var q = Common.QueryResults.SaveQueryResults(result);

                            if (q.QueryId > 0)
                            {
                                q.ResultTableFormat = sqlquery.ResultTableFormat;
                                return Json(q);
                            }
                        }
                        else
                        {
                            var queryinfo = Common.QueryResults.GetQueryResults(sqlquery.QueryId);
                            if (queryinfo != null)
                            {
                                queryinfo.SqlQuery = sqlquery.Query;
                            }
                            var q = Common.QueryResults.SaveQueryResults(queryinfo);
                            q.ResultTableFormat = sqlquery.ResultTableFormat;
                            q.Message = "Updated Successfully..";
                            return Json(q);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex.ToString());
                return Json(ex.ToString());
            }
            return Json(sqlquery);
        }

        [Authorize]
        public IActionResult AddPlaybook(int? queryId)
        {
            try
            {
                var _queryId = Convert.ToInt32(queryId);
                var _comapnyId = Arg.DataAccess.ActiveClient.Info.CompanyId;
                if (_comapnyId < 0)
                {
                    Common.GoToLogin();
                }

                if (_queryId > 0 && _comapnyId > 0)
                {
                    var qr = new Arg.DataAccess.QueryResultsImpl().GetQueryResults(_queryId);
                    if (qr != null)
                    {
                        var region = Common.Regions.GetRegion(0, _comapnyId);//temporary
                        var result = new Arg.DataModels.AuditorPlaybook
                        {
                            CompanyID = _comapnyId,
                            Region = region.Region,
                            AuditingScreenFilters = "",
                            Status = Convert.ToInt32(Common.Contants.StatusTypeEnum.InProcess),
                            StatusDate = DateTime.Now,
                            Priority = Convert.ToInt32(Common.Contants.PriorityTypeEnum.High),
                            UserID = Common.CurrentUserId, //Common.ActiveClient.GetUserFLName(),
                            Comments = "",
                            QueryId = _queryId
                        };

                        Common.AuditorPlaybooks.SaveAuditorPlaybook(result);
                        if (result.PlayID > 0)
                        {
                            return RedirectToAction("AuditorPlaybook", "BOL", new { queryId = queryId, playId = result.PlayID });
                        }
                    }
                }
                return Json("Success");
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex.ToString());
                return Json(ex.ToString());
            }
        }

        public IActionResult ViewAuditingResultTableFormat(int? queryId, int? idx, int? pageNo, List<string> shipperId, List<string> orig, List<string> dest, List<string> POL, string stats)
        {
            var data = new BOLAuditingResults();

            try
            {
                int pageSize = Common.PageSize;
                int pageIndex = 1;
                if (Arg.DataAccess.ActiveClient.Info.DBName.Contains("ClientPasha"))
                {
                    data.BolAuditResultTableFormat = new List<Arg.DataModels.BOLHeader>().ToPagedList(pageIndex, pageSize);

                    var bolAuditingtableformatresult = new List<Arg.DataModels.BOLHeader>();

                    bolAuditingtableformatresult.ToPagedList(pageIndex, pageSize);
                    pageIndex = pageNo.HasValue ? Convert.ToInt32(pageNo) : 1;
                    var searchOptions = new Arg.DataModels.SearchOptions();

                    var _queryId = Convert.ToInt32(queryId);
                    var _idx = Convert.ToInt32(idx);
                    if (((shipperId != null) || (orig != null) || (dest != null) || (POL != null)) && (stats == "stats"))
                    {
                        queryId = SaveQueryJson(shipperId, orig, dest, POL, _queryId);
                        _queryId = Convert.ToInt32(queryId);
                    }

                    var qr = new Arg.DataAccess.QueryResultsImpl().GetQueryResults(_queryId);
                    if (qr.SqlQuery == null)
                    {
                        searchOptions = JsonConvert.DeserializeObject<Arg.DataModels.SearchOptions>(qr.QueryJson);
                        bolAuditingtableformatresult = Common.BOLHeader.GetResults(_queryId, "table");
                        data.InvoiceBillType = searchOptions.BillType;
                    }
                    else
                    {
                        bolAuditingtableformatresult = Common.BOLHeader.GetResults(_queryId, "table");
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
                else if (Arg.DataAccess.ActiveClient.Info.DBName.Contains("ClientAgility"))
                {
                    data.AgilityBolAuditResultTableFormat = new List<Arg.Agility.DataModels.BOLHeaders>().ToPagedList(pageIndex, pageSize);

                    var agilitybolAuditingtableformatresult = new List<Arg.Agility.DataModels.BOLHeaders>();

                    agilitybolAuditingtableformatresult.ToPagedList(pageIndex, pageSize);
                    pageIndex = pageNo.HasValue ? Convert.ToInt32(pageNo) : 1;
                    var searchOptions = new Arg.DataModels.SearchOptions();

                    var _queryId = Convert.ToInt32(queryId);
                    var _idx = Convert.ToInt32(idx);
                    if (((shipperId != null) || (orig != null) || (dest != null) || (POL != null)) && (stats == "stats"))
                    {
                        queryId = SaveQueryJson(shipperId, orig, dest, POL, _queryId);
                        _queryId = Convert.ToInt32(queryId);
                    }

                    var qr = new Arg.DataAccess.QueryResultsImpl().GetQueryResults(_queryId);
                    if (qr.SqlQuery == null)
                    {
                        searchOptions = JsonConvert.DeserializeObject<Arg.DataModels.SearchOptions>(qr.QueryJson);
                        agilitybolAuditingtableformatresult = Common.AgilityBOLHeader.GetResults(_queryId, "table");
                    }
                    else
                    {
                        agilitybolAuditingtableformatresult = Common.AgilityBOLHeader.GetResults(_queryId, "table");
                    }

                    if (agilitybolAuditingtableformatresult == null || !agilitybolAuditingtableformatresult.Any())
                    {
                        data.Message = "No results found related to  your search!";
                        return View(data);
                    }
                    else
                    {
                        data.CompanyId = Arg.DataAccess.ActiveClient.Info.CompanyId;
                        data.AgilityBolAuditResultTableFormat = agilitybolAuditingtableformatresult.ToPagedList(pageIndex, pageSize);
                        data.SearchOptions = searchOptions;
                    }
                }
                else if (Arg.DataAccess.ActiveClient.Info.DBName.Contains("ClientHellmann"))
                {
                    data.ShipmentJournalAuditResultTableFormat = new List<Arg.DataModels.ShipmentJournal>().ToPagedList(pageIndex, pageSize);

                    var ShipmentAuditingtableformatresult = new List<Arg.DataModels.ShipmentJournal>();

                    ShipmentAuditingtableformatresult.ToPagedList(pageIndex, pageSize);
                    pageIndex = pageNo.HasValue ? Convert.ToInt32(pageNo) : 1;
                    var searchOptions = new Arg.DataModels.SearchOptions();

                    var _queryId = Convert.ToInt32(queryId);
                    var _idx = Convert.ToInt32(idx);
                    if (((shipperId != null) || (orig != null) || (dest != null) || (POL != null)) && (stats == "stats"))
                    {
                        queryId = SaveQueryJson(shipperId, orig, dest, POL, _queryId);
                        _queryId = Convert.ToInt32(queryId);
                    }

                    var qr = new Arg.DataAccess.QueryResultsImpl().GetQueryResults(_queryId);
                    if (qr.SqlQuery == null)
                    {
                        searchOptions = JsonConvert.DeserializeObject<Arg.DataModels.SearchOptions>(qr.QueryJson);
                        ShipmentAuditingtableformatresult = Common.ShipmentJournal.GetResults(_queryId, "table");
                        data.InvoiceBillType = searchOptions.BillType;
                    }
                    else
                    {
                        ShipmentAuditingtableformatresult = Common.ShipmentJournal.GetResults(_queryId, "table");
                    }
                    if (ShipmentAuditingtableformatresult == null || !ShipmentAuditingtableformatresult.Any())
                    {
                        data.Message = "No results found related to  your search!";
                        return View(data);
                    }
                    else
                    {
                        data.CompanyId = Arg.DataAccess.ActiveClient.Info.CompanyId;
                        data.ShipmentJournalAuditResultTableFormat = ShipmentAuditingtableformatresult.ToPagedList(pageIndex, pageSize);
                        //data.BolAuditResultTableFormat = bolAuditingtableformatresult;
                        data.SearchOptions = searchOptions;
                    }
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

                var note = @"BOL#: <a href='" + Common.MyRoot + "/Bol/AuditingResults?bolNo=" + bolNo + "&CompanyId=" + companyId + "' target='_blank'>" + bolNo + "</a>";
                Common.ActivityStats.SaveActivityStats(Arg.DataAccess.ActivityStatsImpl.EnumActions.BOLViewed, companyId, note, bolNo);
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex.ToString());
                return Json(ex.ToString());
            }
            return Json("Success");
        }

        public int SaveQueryJson(List<string> shipperId, List<string> orig, List<string> dest, List<string> POL, int queryId)
        {
            int _queryId = 0;
            var bOLAuditing = new BOLAuditing();

            try
            {
                if ((shipperId != null) || (orig != null) || (dest != null) || POL != null)
                {
                    bOLAuditing.SearchOptions = new Arg.DataModels.SearchOptions();
                    var qr = Common.QueryResults.GetQueryResults(queryId);
                    bOLAuditing.SearchOptions = JsonConvert.DeserializeObject<Arg.DataModels.SearchOptions>(qr.QueryJson);

                    if (shipperId != null) 
                    {
                        bOLAuditing.SearchOptions.ShipperID = shipperId; 
                    }
                       
                    if (orig != null)
                    {
                        bOLAuditing.SearchOptions.OriginLocationCode = orig;
                    }
                        
                    if (dest != null)
                    {
                        bOLAuditing.SearchOptions.DestinationLocationCode = dest;
                    }
                        
                    if (POL != null)
                    {
                        bOLAuditing.SearchOptions.POLCode = POL;
                    }

                    bOLAuditing.CompanyId = Arg.DataAccess.ActiveClient.Info.CompanyId;
                    bOLAuditing.SearchOptions.CompanyId = bOLAuditing.CompanyId;
                    var q = Common.QueryResults.SaveQueryResults(bOLAuditing.SearchOptions);

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

        [HttpPost]
        public IActionResult SaveArrangement(List<AuditSortingType> values)
        {
            int clientId = Arg.DataAccess.ActiveClient.Info.CompanyId;
            string loginId = User.Identity.GetUserId();
            var qr = Common.BOLAuditSorting.GetQueryResults(clientId, loginId);

            if (qr != null && qr.Count > 0)
            {
                Common.BOLAuditSorting.DeleteSortingOrder(clientId, loginId);
            }

            foreach (AuditSortingType ast in values)
            {
                Arg.DataModels.BOLAuditSorting auditSorting = new Arg.DataModels.BOLAuditSorting();
                auditSorting.ClientId = Arg.DataAccess.ActiveClient.Info.CompanyId;
                auditSorting.FieldName = ast.FieldName;
                auditSorting.FieldValue = ast.FieldValue;
                auditSorting.Height = ast.Height;
                auditSorting.LoginID = loginId;
                Common.BOLAuditSorting.SaveSortingOrder(auditSorting);
            }

            return Json(new { success = true });
        }


        private class AjaxResult
        {
            public int QueryId { get; set; }
            public string Type { get; set; } //RESULTS or STATS or Message
            public bool ResultTableFormat { get; set; }
        }

        public class AuditSortingType
        {
            public string FieldName { get; set; }
            public string FieldValue { get; set; }
            public string Height { get; set; }
        }
    }
}
