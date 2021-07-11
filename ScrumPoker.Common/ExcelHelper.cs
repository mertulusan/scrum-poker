using DocumentFormat.OpenXml.Packaging;
using ScrumPoker.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.IO;
using System.Text;

namespace ScrumPoker.Common
{
    public class ExcelHelper
    {
        public byte[] ExportToExcelByOpenXML<T>(IList<T> list, string docName = "Task List")
        {
            if (list.Count <= 0)
                throw new Exception("list cannot be empty ");

            List<DataSet> dsList = new List<DataSet>();

            var ds = ToDataSet(list, docName);

            dsList.Add(ds);

            MemoryStream ms = new MemoryStream();

            using (var objSpreadsheet = SpreadsheetDocument.Create(ms, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook))
            {
                var workbookPart = objSpreadsheet.AddWorkbookPart();
                objSpreadsheet.WorkbookPart.Workbook = new DocumentFormat.OpenXml.Spreadsheet.Workbook();
                objSpreadsheet.WorkbookPart.Workbook.Sheets = new DocumentFormat.OpenXml.Spreadsheet.Sheets();

                uint sheetId = 1;
                for (int i = 0; i < dsList.Count; i++)
                {
                    foreach (DataTable table in dsList[i].Tables)
                    {
                        var sheetPart = objSpreadsheet.WorkbookPart.AddNewPart<WorksheetPart>();
                        var sheetData = new DocumentFormat.OpenXml.Spreadsheet.SheetData();
                        sheetPart.Worksheet = new DocumentFormat.OpenXml.Spreadsheet.Worksheet(sheetData);

                        DocumentFormat.OpenXml.Spreadsheet.Sheets sheets = objSpreadsheet.WorkbookPart.Workbook.GetFirstChild<DocumentFormat.OpenXml.Spreadsheet.Sheets>();
                        string relationshipId = objSpreadsheet.WorkbookPart.GetIdOfPart(sheetPart);

                        sheetId += 1;

                        DocumentFormat.OpenXml.Spreadsheet.Sheet sheet = new DocumentFormat.OpenXml.Spreadsheet.Sheet() { Id = relationshipId, SheetId = sheetId, Name = dsList[i].DataSetName };
                        sheets.Append(sheet);

                        DocumentFormat.OpenXml.Spreadsheet.Row headerRow = new DocumentFormat.OpenXml.Spreadsheet.Row();

                        List<String> columns = new List<string>();
                        foreach (DataColumn column in table.Columns)
                        {
                            columns.Add(column.ColumnName);

                            DocumentFormat.OpenXml.Spreadsheet.Cell cell = new DocumentFormat.OpenXml.Spreadsheet.Cell();
                            cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
                            cell.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(column.ColumnName);
                            headerRow.AppendChild(cell);
                        }

                        sheetData.AppendChild(headerRow);

                        foreach (DataRow dsrow in table.Rows)
                        {
                            DocumentFormat.OpenXml.Spreadsheet.Row newRow = new DocumentFormat.OpenXml.Spreadsheet.Row();
                            foreach (String col in columns)
                            {
                                DocumentFormat.OpenXml.Spreadsheet.Cell cell = new DocumentFormat.OpenXml.Spreadsheet.Cell();
                                cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
                                cell.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(dsrow[col].ToString());
                                newRow.AppendChild(cell);
                            }

                            sheetData.AppendChild(newRow);
                        }
                    }
                }
            }
            return ms.ToArray();
        }

        private DataSet ToDataSet<T>(IList<T> list, string datasetName)
        {
            Type elementType = typeof(T);
            DataSet ds = new DataSet();
            ds.DataSetName = datasetName;
            DataTable t = new DataTable();
            ds.Tables.Add(t);

            foreach (var propInfo in elementType.GetProperties())
            {
                Type ColType = Nullable.GetUnderlyingType(propInfo.PropertyType) ?? propInfo.PropertyType;

                if (propInfo.GetCustomAttributes(true).Length == 0) continue;

                if (ColType.BaseType.Name == "Enum")
                    ColType = typeof(string);

                var attr = (System.ComponentModel.DataAnnotations.DisplayAttribute)(propInfo.GetCustomAttributes(true)[0]);
                if (attr != null)
                    t.Columns.Add(attr.Name, ColType);
                else
                    t.Columns.Add(propInfo.Name, ColType);
            }
            foreach (T item in list)
            {
                DataRow row = t.NewRow();

                foreach (var propInfo in elementType.GetProperties())
                {
                    if (propInfo.GetCustomAttributes(true).Length == 0) continue;

                    var data = propInfo.GetValue(item, null).GetType().BaseType.Name == "Enum" ? propInfo.GetValue(item, null).GetEnumDescription() : propInfo.GetValue(item, null);

                    row[((DisplayAttribute)propInfo.GetCustomAttributes(true)[0]).Name] = data ?? string.Empty;
                }

                t.Rows.Add(row);
            }

            return ds;
        }
    }
}
