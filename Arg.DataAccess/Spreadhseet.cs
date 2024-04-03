using Arg.DataModels.SharedHelpers;
using Google.Apis.Http;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;

namespace Arg.DataAccess
{
    public class Spreadhseet
    {
        public class SpreadsheetInfo
        {
            public string Id { get; set; }

            public string Url { get; set; }
        }

        public static SheetsService _service;

        public static SheetsService Service
        {
            get
            {
                if (_service == null)
                {
                    _service = new SheetsService(new BaseClientService.Initializer
                    {
                        HttpClientInitializer = (IConfigurableHttpClientInitializer)(object)SharedHelper.UserCredential,
                        ApplicationName = SharedHelper.ApplicationName
                    });
                }

                return _service;
            }
        }

        public static TextFormat DefaultTextFormat => new TextFormat
        {
            FontFamily = "Courier New"
        };

        public static TextFormat DefaultTextFormatDetailsSheet => new TextFormat
        {
            FontFamily = "Courier New",
            FontSize = 9
        };

        public static CellFormat DefaultCellFormat => new CellFormat
        {
            TextFormat = DefaultTextFormat,
            VerticalAlignment = "top"
        };

        public static Border BlackBorder => new Border
        {
            Color = BlackColor,
            Width = 1,
            Style = "SOLID"
        };

        public static Borders AllSideBlackBorders => new Borders
        {
            Top = BlackBorder,
            Right = BlackBorder,
            Bottom = BlackBorder,
            Left = BlackBorder
        };

        public static Color BlackColor => new Color
        {
            Blue = 0f,
            Red = 0f,
            Green = 0f
        };

        public static Color GrayColor => new Color
        {
            Blue = 90f,
            Red = 90f,
            Green = 90f
        };

        public static Color WhiteColor => new Color
        {
            Blue = 255f,
            Red = 255f,
            Green = 255f
        };

        public static CellFormat HeadingCellFormat
        {
            get
            {
                new CellFormat();
                TextFormat defaultTextFormat = DefaultTextFormat;
                CellFormat defaultCellFormat = DefaultCellFormat;
                defaultTextFormat.Bold = true;
                defaultCellFormat.HorizontalAlignment = "CENTER";
                defaultCellFormat.Borders = AllSideBlackBorders;
                defaultCellFormat.BackgroundColor = GrayColor;
                defaultTextFormat.ForegroundColor = WhiteColor;
                defaultCellFormat.TextFormat = defaultTextFormat;
                defaultCellFormat.VerticalAlignment = "top";
                return defaultCellFormat;
            }
        }

        public static CellFormat HeadingCellFormatDetailsSheet
        {
            get
            {
                CellFormat headingCellFormat = HeadingCellFormat;
                TextFormat defaultTextFormatDetailsSheet = DefaultTextFormatDetailsSheet;
                defaultTextFormatDetailsSheet.Bold = true;
                headingCellFormat.TextFormat = defaultTextFormatDetailsSheet;
                return headingCellFormat;
            }
        }

        public static CellFormat CenterAlignCellFormat
        {
            get
            {
                CellFormat cellFormat = new CellFormat();
                TextFormat defaultTextFormat = DefaultTextFormat;
                cellFormat.HorizontalAlignment = "CENTER";
                cellFormat.VerticalAlignment = "TOP";
                cellFormat.Borders = AllSideBlackBorders;
                cellFormat.TextFormat = defaultTextFormat;
                return cellFormat;
            }
        }

        public static CellFormat CenterAlignCellFormatDetailsSheet
        {
            get
            {
                CellFormat centerAlignCellFormat = CenterAlignCellFormat;
                centerAlignCellFormat.TextFormat = DefaultTextFormatDetailsSheet;
                return centerAlignCellFormat;
            }
        }

        public static CellFormat RightAlignCellFormat
        {
            get
            {
                CellFormat cellFormat = new CellFormat();
                TextFormat defaultTextFormat = DefaultTextFormat;
                cellFormat.HorizontalAlignment = "RIGHT";
                cellFormat.VerticalAlignment = "TOP";
                cellFormat.Borders = AllSideBlackBorders;
                cellFormat.TextFormat = defaultTextFormat;
                return cellFormat;
            }
        }

        public static CellFormat RightAlignCellFormatDetailsSheet
        {
            get
            {
                CellFormat rightAlignCellFormat = RightAlignCellFormat;
                rightAlignCellFormat.TextFormat = DefaultTextFormatDetailsSheet;
                return rightAlignCellFormat;
            }
        }

        public static CellFormat LeftAlignCellFormat
        {
            get
            {
                CellFormat cellFormat = new CellFormat();
                TextFormat defaultTextFormat = DefaultTextFormat;
                cellFormat.HorizontalAlignment = "LEFT";
                cellFormat.VerticalAlignment = "TOP";
                cellFormat.Borders = AllSideBlackBorders;
                cellFormat.TextFormat = defaultTextFormat;
                return cellFormat;
            }
        }

        public static CellFormat LeftAlignCellFormatDetailsSheet
        {
            get
            {
                CellFormat leftAlignCellFormat = LeftAlignCellFormat;
                leftAlignCellFormat.TextFormat = DefaultTextFormatDetailsSheet;
                return leftAlignCellFormat;
            }
        }

        public static CellFormat BDDescFormatDetailsSheet
        {
            get
            {
                CellFormat leftAlignCellFormatDetailsSheet = LeftAlignCellFormatDetailsSheet;
                leftAlignCellFormatDetailsSheet.WrapStrategy = "Wrap";
                return leftAlignCellFormatDetailsSheet;
            }
        }

        public static CellFormat BoldCellFormat
        {
            get
            {
                CellFormat cellFormat = new CellFormat();
                TextFormat defaultTextFormat = DefaultTextFormat;
                cellFormat.Borders = AllSideBlackBorders;
                defaultTextFormat.Bold = true;
                cellFormat.TextFormat = defaultTextFormat;
                cellFormat.HorizontalAlignment = "CENTER";
                cellFormat.VerticalAlignment = "TOP";
                return cellFormat;
            }
        }

        public static CellFormat BoldCellFormatDetailsSheet
        {
            get
            {
                CellFormat boldCellFormat = BoldCellFormat;
                TextFormat defaultTextFormat = DefaultTextFormat;
                defaultTextFormat.Bold = true;
                defaultTextFormat.FontSize = 9;
                boldCellFormat.TextFormat = defaultTextFormat;
                return boldCellFormat;
            }
        }

        public static CellFormat H1HeadingCellFormat
        {
            get
            {
                CellFormat cellFormat = new CellFormat();
                TextFormat defaultTextFormat = DefaultTextFormat;
                defaultTextFormat.Bold = true;
                defaultTextFormat.FontSize = 20;
                cellFormat.TextFormat = defaultTextFormat;
                cellFormat.VerticalAlignment = "top";
                return cellFormat;
            }
        }

        public static void MergeCells(int startRowIdx, int endRowIdx, int startColIdx, int endColIdx, ref Sheet sheet)
        {
            if (!sheet.Properties.SheetId.HasValue)
            {
                throw new Exception("SheetId can't be null. Just set a value in properties, like 0, 1 etc");
            }

            sheet.Merges.Add(new GridRange
            {
                StartRowIndex = startRowIdx,
                EndRowIndex = endRowIdx,
                StartColumnIndex = startColIdx,
                EndColumnIndex = endColIdx,
                SheetId = sheet.Properties.SheetId
            });
        }

        public static RowData GenerateRowData(string[] values, ref GridData gd, CellFormat cf, bool upperCase)
        {
            RowData rd = new RowData();
            if (cf == null)
            {
                cf = DefaultCellFormat;
            }

            rd.Values = new List<CellData>();
            foreach (string text in values)
            {
                if (upperCase)
                {
                    AddCellDataRow(text.ToUpper(), ref rd, cf);
                }
                else
                {
                    AddCellDataRow(text, ref rd, cf);
                }
            }

            return rd;
        }

        public static void AddRowData(string[] values, ref GridData gd, CellFormat cf = null, bool upperCase = false)
        {
            RowData item = GenerateRowData(values, ref gd, cf, upperCase);
            gd.RowData.Add(item);
        }

        public static void AddImageRowToSheetData(string val, ref GridData gd)
        {
            ExtendedValue extendedValue = new ExtendedValue();
            extendedValue.FormulaValue = val;
            CellFormat defaultCellFormat = DefaultCellFormat;
            defaultCellFormat.WrapStrategy = "OVERFLOW_CELL";
            AddValueRowToSheetData(val, ref gd, defaultCellFormat, extendedValue);
        }

        public static void AddValueRowToSheetData(string val, ref GridData gd, CellFormat cf, ExtendedValue ev)
        {
            RowData rowData = new RowData();
            CellData cellData = new CellData();
            if (ev == null)
            {
                ev = new ExtendedValue();
                ev.StringValue = val;
            }

            if (cf == null)
            {
                cf = DefaultCellFormat;
            }

            cellData.UserEnteredValue = ev;
            cellData.UserEnteredFormat = cf;
            if (rowData.Values == null)
            {
                rowData.Values = new List<CellData>();
            }

            rowData.Values.Add(cellData);
            if (gd.RowData == null)
            {
                gd.RowData = new List<RowData>();
            }

            gd.RowData.Add(rowData);
        }

        public static void AddValueRowToSheetData(string val, ref GridData gd, CellFormat cf = null)
        {
            AddValueRowToSheetData(val, ref gd, cf, null);
        }

        public static void AddHyperlinkCellDataRow(string val, string link, ref RowData rd, CellFormat cf)
        {
            AddCellDataRow(val, ref rd, cf, new ExtendedValue
            {
                FormulaValue = "=HYPERLINK(\"" + link + "\",\"" + val + "\")"
            });
        }

        public static void AddCellDataRow(string val, ref RowData rd, CellFormat cf = null)
        {
            AddCellDataRow(val, ref rd, cf, null);
        }

        public static Request GenerateUpdateColumnWidthRequest(int sheetId, int width, int startIdx, int endIdx)
        {
            DimensionProperties dimensionProperties = new DimensionProperties();
            dimensionProperties.PixelSize = width;
            Request request = new Request();
            request.UpdateDimensionProperties = new UpdateDimensionPropertiesRequest();
            request.UpdateDimensionProperties.Range = new DimensionRange
            {
                SheetId = sheetId,
                StartIndex = startIdx,
                EndIndex = endIdx,
                Dimension = "COLUMNS"
            };
            request.UpdateDimensionProperties.Properties = dimensionProperties;
            request.UpdateDimensionProperties.Fields = "*";
            return request;
        }

        public static void ProcessRequests(List<Request> requests, string spreadsheetId)
        {
            BatchUpdateSpreadsheetRequest batchUpdateSpreadsheetRequest = new BatchUpdateSpreadsheetRequest();
            batchUpdateSpreadsheetRequest.Requests = new List<Request>();
            foreach (Request request in requests)
            {
                batchUpdateSpreadsheetRequest.Requests.Add(request);
            }

            Service.Spreadsheets.BatchUpdate(batchUpdateSpreadsheetRequest, spreadsheetId).Execute();
        }

        public static void AddCellDataRow(string val, ref RowData rd, CellFormat cf, ExtendedValue ev)
        {
            CellData cellData = new CellData();
            if (ev == null)
            {
                ev = new ExtendedValue();
                ev.StringValue = val;
            }

            if (cf == null)
            {
                cf = DefaultCellFormat;
                cf.Borders = AllSideBlackBorders;
            }

            cellData.UserEnteredValue = ev;
            cellData.UserEnteredFormat = cf;
            rd.Values.Add(cellData);
        }
    }
}
