using ArgCore.Attributes;
using ArgCore.Helpers;
using ArgCore.Models;
using BLToolkit.Aspects;
using Microsoft.AspNetCore.Mvc;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace ArgCore.Controllers
{
    public class ImportController : Controller
    {
        [AuthorizeUser]
        public IActionResult DeleteMapping(int mappingId, string file)
        {
            try
            {
                var result = Common.Mappings.DeleteMapping(mappingId);
                if (result > 0)
                {
                    Common.ActivityStats.SaveActivityStats(Arg.DataAccess.ActivityStatsImpl.EnumActions.Deleted, 0, "Mappings");
                    return RedirectToAction("ManageMappings", new { file = file });
                }
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }
            return RedirectToAction("ManageMappings", new { file = file });
        }

        public IActionResult DeleteTableSettings(int tableSettId)
        {
            try
            {
                var result = Common.TableSettings.DeleteTableSetting(tableSettId);
                if (result > 0)
                {
                    Common.ActivityStats.SaveActivityStats(Arg.DataAccess.ActivityStatsImpl.EnumActions.Deleted, 0, "TableSettings");
                    return RedirectToAction("ManageTableSettings");
                }
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }
            return RedirectToAction("ManageTableSettings");
        }

        [AuthorizeUser]
        public IActionResult ManageMappings(string file)
        {
            var data = new MappingsInfo();
            try
            {
                data.File = file;
                data.CommonObjects.TopHeading = "Mapping Info";
                data.CommonObjects.Heading = "Mapping Info";
                //var fileName = Path.GetFileName(file);

                var mappings = Common.Mappings.GetMappingBySourceFileName(file);
                if (mappings != null && mappings.Any())
                {
                    data.Mappings = mappings.Cast<Arg.DataModels.Mappings.Mapping>().ToList();
                }
                   
                //var filePath = @"E:\clientdocs\pasha\Data Files\Loaded\980000-006(01Mar2017)\980000-006(01Mar2017).xls";
                file = Common.ClientFilesPath + file;
                Common.Log.Info(file);

                var headersList = GetHeadingList(file);
                if (headersList != null && headersList.Any())
                {
                    data.HeadersList = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(headersList, "Index", "Name");
                }
                   
                var tablelist = GetTablesList();
                if (tablelist != null && tablelist.Any())
                {
                    data.TablesList = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(tablelist);
                }
                    
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex.ToString());
            }
            return View(data);
        }

        [AuthorizeUser]
        public IActionResult ManageTableSettings(int? tabSettId)
        {
            var data = new TableSettings();
            try
            {
                data.CommonObjects.TopHeading = "Manage Table Settings";
                data.CommonObjects.Heading = "Manage Table Settings";

                var tabSett = Common.TableSettings.GetTableSettingByTruncateTable();
                if (tabSett != null && tabSett.Any())
                {
                    data.TableSettingsList = tabSett.Cast<Arg.DataModels.TableSettings>().ToList();
                }
                    
                data.TableSettingDetail = new Arg.DataModels.TableSettings();
                var _tabSettId = Convert.ToInt32(tabSettId);
                if (_tabSettId > 0)
                {
                    data.TableSettingDetail = Common.TableSettings.GetTableSettingById(_tabSettId);
                    if (data.TableSettingDetail == null || data.TableSettingDetail.TableSettId <= 0)
                    {
                        return RedirectToAction("ManageTableSettings", new { m = "Table Settings not found or deleted" });
                    }
                       
                }
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex.ToString());
            }
            return View(data);
        }

        public dynamic LoadExcelFile(string fileName)
        {
			try
			{
				var fileExists = Path.GetExtension(fileName);
				if (fileName.ToLower() == ".xls")
				{
					FileStream excelStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
					var book = new HSSFWorkbook(excelStream);
					excelStream.Close();
					return book;

                }
			}
			catch (Exception ex)
			{
				Common.Log.Error(ex);
			}
			return null;
        }

		public List<HeaderInfo> GetHeadingList(string fileName)
		{
			try
			{
				Common.Log.Info(fileName + ", from GetHeadingList");
				var book = LoadExcelFile(fileName);
				var sheet = book.GetSheetAt(0);
				var headerRow = sheet.GetRow(0);
				var cellCount = headerRow.LastCellNum;
				var headerList = new List<HeaderInfo>();
                for (int i = headerRow.FirstCellNum; i < cellCount; i++)
                {
                    headerList.Add(new HeaderInfo { Name = headerRow.GetCell(i).StringCellValue, Index = i });
                }
                return headerList;

            }
			catch (Exception ex)
			{
                Common.Log.Error(ex);
            }
			return null;
		}

        private ValInfo GetCellValue(ICell cell)
        {
            var val = new ValInfo();
            if (cell == null)
            {
                val.SqlVal = "'" + string.Empty + "'";
                val.ValType = CellType.Unknown;
                return val;
            }
            val.ValType = cell.CellType;
            switch (cell.CellType)
            {
                case CellType.Blank:
                    {
                        val.Val = "";
                        val.SqlVal = "'" + string.Empty + "'";
                        return val;
                    }
                case CellType.Boolean:
                    {
                        val.SqlVal = cell.BooleanCellValue.ToString();
                        return val;
                    }
                case CellType.Error:
                    {
                        val.SqlVal = cell.ErrorCellValue.ToString();
                        return val;
                    }
                case CellType.Numeric:
                    {
                        val.SqlVal = cell.NumericCellValue.ToString();
                        return val;
                    }
                case CellType.String:
                    {
                        val.SqlVal = "'" + cell.StringCellValue.ToString() + "'";
                        return val;
                    }
                case CellType.Formula:
                    {
                        try
                        {
                            var e = new HSSFFormulaEvaluator(cell.Sheet.Workbook);
                            e.EvaluateInCell(cell);
                            val.SqlVal = cell.ToString();
                        }
                        catch
                        {
                            val.SqlVal = cell.NumericCellValue.ToString();
                        }
                        return val;
                    }
                case CellType.Unknown:
                default:
                    val.SqlVal = "'" + cell.ToString() + "'";//This is a trick to get the correct value of the cell. NumericCellValue will return a numeric value no matter the cell value is a date or a number
                    return val;
            }
        }

        public JsonResult GetImportProgress()
        {
            return Json(Common.ImportProgress);
        }

        public void ImportData(int companyId)
        {
            var cmd = "";
            try
            {
                //List<string> files = new List<string>();
                //if (filesList != null)
                //{
                //    files = filesList.Split(',').ToList();
                //}
                //var clientFilePath = Common.ClientFilesPath;
                var clientFilePath = Common.ArgClients.GetArgClient(companyId, "").ImportDataPath;
                var files = Directory.GetFiles(clientFilePath, "*.xls", SearchOption.AllDirectories).Where(s => s.EndsWith(".xls"));

                Common.ImportProgress.Add("Total " + files.Count() + " files found!");
                foreach (var file in files)
                {
                    var fileName = Path.GetFileName(file);
                    Common.ImportProgress.Add("New file found - " + file);
                    //var fileName = Path.GetFileName(file);
                    var book = LoadExcelFile(file);
                    var sheet = book.GetSheetAt(0);
                    var rowCount = sheet.PhysicalNumberOfRows;
                    var colCount = sheet.GetRow(0).LastCellNum;

                    var hasData = true;
                    var i = 1;
                    //var fileName = Path.GetFileName(loadClientData.SelectedFile);
                    var mappings = Common.Mappings.GetMappingBySourceFileName(fileName);//.Where(x=>x.SourceFileNameMatch==loadClientData.SelectedFile);
                    if (mappings == null || !mappings.Any())
                    {
                        Common.ImportProgress.Add("No mappings found for file - " + fileName);
                    }
                    else
                    {
                        var tables = mappings.DistinctBy(x => x.TargetTableName).Select(x => x.TargetTableName);
                        var headers = GetHeadingList(file);
                        Common.ImportProgress.Add(headers.Count + " columns found in file");
                        while (hasData)
                        {
                            try
                            {
                                var row = sheet.GetRow(i);
                                hasData = row != null;
                                if (!hasData)
                                {
                                    Common.ImportProgress.Add("Import ended here!");
                                    Trace.TraceInformation("Import ended here!");
                                    break;
                                }
                                var cmdCols = "";
                                var cmdVals = "";
                                cmd = "INSERT INTO ";
                                foreach (var table in tables)
                                {
                                    try
                                    {
                                        cmd += table;
                                        foreach (var map in mappings.Where(x => x.TargetTableName == table))
                                        {
                                            try
                                            {
                                                cmdCols += "[" + map.TargetColName + "],";
                                                var cell = row.GetCell(map.SourceColIndex);
                                                //var cellType = cell.getCellType();
                                                //if (cellType == Cell.CELL_TYPE_NUMERIC)
                                                //{
                                                //    cell.setCellType(Cell.CELL_TYPE_STRING);
                                                //}
                                                //var cellValue = cell.getStringCellValue();
                                                //var cellValue = cell.StringCellValue;
                                                var cellValue = GetCellValue(cell);
                                                if (cellValue.ValType == CellType.Numeric || cellValue.ValType == CellType.Unknown)
                                                {
                                                    var dataType = GetColumnDataType(table, map.TargetColName);
                                                    if ((dataType == "int" && cellValue.ValType == CellType.Numeric) || (dataType == "numeric" && cellValue.ValType == CellType.Numeric) || (dataType == "numeric" && cellValue.ValType == CellType.Unknown))
                                                    {
                                                        if (cellValue.SqlVal == "''")
                                                            cellValue.SqlVal = "0";
                                                    }
                                                    else if (dataType == "varchar" && cellValue.ValType == CellType.Numeric)
                                                    {
                                                        cellValue.SqlVal = "'" + cellValue.SqlVal + "'";
                                                    }
                                                }
                                                if (cellValue.SqlVal.IndexOf("'") > 0) //TO REMOVE SINGLE QUOTE IN CONTENT
                                                    cellValue = cellValue.SqlVal.Replace("'", "");
                                                cmdVals += (cellValue.SqlVal + ",");
                                            }
                                            catch (Exception ex)
                                            {
                                                Trace.TraceInformation(cmd);
                                                Trace.TraceInformation(cmdCols);
                                                Trace.TraceInformation(cmdVals);
                                                Common.Log.Error(ex);
                                            }
                                        }
                                        cmdCols += "[Uploaded]";
                                        //cmdCols = cmdCols.Remove(cmdCols.LastIndexOf(","));
                                        var currentDate = DateTime.Now;
                                        cmdVals += "'" + currentDate.ToString("yyyy-MM-dd hh:mm:ss tt") + "'";
                                        //cmdVals = cmdVals.Remove(cmdVals.LastIndexOf(","));
                                        cmd += ("(" + cmdCols + ") VALUES (" + cmdVals + ")");
                                        var result = Convert.ToInt32(Arg.DataAccess.Utilities.ExecuteCmd(cmd));
                                        if (result > 0)
                                        {
                                            Common.ImportProgress.Add("New row saved " + row.GetCell(0));
                                            Common.Log.Info("New row saved " + row.GetCell(0));
                                        }
                                        else
                                        {
                                            Common.ImportProgress.Add("Failed to save row " + row.GetCell(0));
                                            Common.Log.Info("Failed to save row " + row.GetCell(0));
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Trace.TraceWarning(cmd);
                                        Common.Log.Error(ex);
                                    }
                                }
                                i++;
                            }
                            catch (Exception ex)
                            {
                                Trace.TraceWarning(cmd);
                                Common.Log.Error(ex);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.TraceWarning(cmd);
                Common.Log.Error(ex);
                Trace.TraceError(ex.ToString());
            }
        }

        public List<string> GetTablesList()
        {
            try
            {
                using (SqlConnection connection = Common.GetConnection())
                {
                    connection.Open();
                    List<string> tables = new List<string>();
                    DataTable dt = connection.GetSchema("Tables");
                    foreach (DataRow row in dt.Rows)
                    {
                        string tablename = (string)row[2];
                        tables.Add(tablename);
                    }
                    connection.Close();
                    return tables;
                }
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }
            return null;
        }

        public JsonResult GetTablesColumnList(string tableName, string fileName)
        {
            try
            {
                //fileName = Path.GetFileName(fileName);
                var result = new List<string>();
                using (SqlConnection connection = Common.GetConnection())
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(@"SELECT name FROM sys.columns WHERE object_id = OBJECT_ID('" + tableName + @"') AND
                    name NOT IN (select TargetColName from mappings where TargetTableName='" + tableName + "' AND SourceFileName='" + fileName + "')", connection);
                    var sqlDR = cmd.ExecuteReader();
                    List<string> datalist = new List<string>();
                    while (sqlDR.Read())
                    {
                        datalist.Add(sqlDR["name"].ToString());
                    }
                    return Json(datalist); //JsonRequestBehavior.AllowGet
                }
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
                return Json("Error");
            }
        }

        [Cache(MaxCacheTime = 50000, IsWeak = false)]
        public string GetColumnDataType(string tableName, string colName)
        {
            try
            {
                var result = new List<string>();
                using (SqlConnection connection = Common.GetConnection())
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(@"SELECT TYPE_NAME(system_type_id) AS DataType FROM sys.columns
                            WHERE name = '" + colName + "' AND object_id = OBJECT_ID('" + tableName + "')", connection);
                    var sqlDR = cmd.ExecuteReader();
                    string datalist = "";
                    while (sqlDR.Read())
                    {
                        datalist = sqlDR["DataType"].ToString();
                    }
                    return datalist;
                }
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }
            return null;
        }

        [HttpPost]
        public IActionResult SaveMappings(MappingsInfo mappingsInfo)
        {
            try
            {
                //var fileName = Path.GetFileName(mappingsInfo.File);
                var mappInfo = new Arg.DataModels.Mappings.Mapping
                {
                    SourceColName = mappingsInfo.SelectedHeaderName,
                    SourceColIndex = mappingsInfo.SelectedColumnIndex,
                    TargetTableName = mappingsInfo.SelectedTable,
                    TargetColName = mappingsInfo.SelectedColumn,
                    SourceFileName = mappingsInfo.File
                };
                Common.Mappings.SaveMapping(mappInfo);
                if (mappInfo.MappingId > 0)
                {
                    Common.ActivityStats.SaveActivityStats(Arg.DataAccess.ActivityStatsImpl.EnumActions.Saved, 0, "Mappings");
                    Redirect(Common.MyRoot + "Import/ManageMappings?file=" + mappingsInfo.File);
                }
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }
            return Redirect(Common.MyRoot + "Import/ManageMappings?file=" + mappingsInfo.File);
        }

        [HttpPost]
        public IActionResult SaveTableSettings(TableSettings tabSettings)
        {
            try
            {
                Common.TableSettings.SaveTableSetting(tabSettings.TableSettingDetail);
                if (tabSettings.TableSettingDetail.TableSettId > 0)
                {
                    Common.ActivityStats.SaveActivityStats(Arg.DataAccess.ActivityStatsImpl.EnumActions.Saved, 0, "TableSettings");
                    RedirectToAction("ManageTableSettings","Import");
                }
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }
            return RedirectToAction("ManageTableSettings", "Import");
        }

        public class HeaderInfo
        {
            public int Index { get; set; }
            public string Name { get; set; }
        }

        private class ValInfo
        {
            public CellType ValType { get; set; }
            public object Val { get; set; }
            public string SqlVal { get; set; }
        }
    }
}
