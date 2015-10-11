using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace DXApplication1
{
    public class ExportTableToExcel
    {
        #region exportToExcel() Overload

        public static void exportToExcel(DataTable sourceTable, string fileName)
        {
            exportToExcel(sourceTable, fileName, "");
        }
        public static void exportToExcel(DataTable sourceTable, string fileName, string strTitle)
        {
            exportToExcel(sourceTable, fileName, 0, strTitle);
        }
        public static void exportToExcel(DataSet sourceDataset, string fileName)
        {
            exportToExcel(sourceDataset, fileName, "");
        }
        public static void exportToExcel(DataSet sourceDataset, string fileName, string strTitle)
        {
            exportToExcel(sourceDataset.Tables[0], fileName, strTitle);
        }
        public static void exportToExcel(DataView sourceDataView, string fileName)
        {
            exportToExcel(sourceDataView, fileName, "");
        }
        public static void exportToExcel(DataView sourceDataView, string fileName, string strTitle)
        {
            exportToExcel(sourceDataView.ToTable(), fileName, strTitle);
        }
        public static void exportToExcel(DataSet sourceDataset, string fileName, int Decimals)
        {
            exportToExcel(sourceDataset.Tables[0], fileName, Decimals);
        }
        public static void exportToExcel(DataSet sourceDataset, string fileName, int Decimals, string strTitle)
        {
            exportToExcel(sourceDataset.Tables[0], fileName, Decimals, strTitle);
        }
        public static void exportToExcel(DataView sourceDataView, string fileName, int Decimals)
        {
            exportToExcel(sourceDataView.ToTable(), fileName, Decimals);
        }
        public static void exportToExcel(DataView sourceDataView, string fileName, int Decimals, string strTitle)
        {
            exportToExcel(sourceDataView.ToTable(), fileName, Decimals, strTitle);
        }
        #endregion

        private static string decimalFormatPlace(int Decimals)//num of places
        {
            //if (Decimals < 0)
            //{
            //    throw new Exception("Parameter is not valid!");
            //}
            if (Decimals == 0)
                return "General";

            string strReturn = "0.";

            if (Decimals >= 0)
            {
                for (int i = 0; i < Decimals; i++)
                {
                    strReturn += "0";
                }
            }
            return strReturn;
        }
        public static void exportToExcel(DataTable sourceTable, string fileName, int Decimals)
        {
            exportToExcel(sourceTable, fileName, Decimals, "AHH");
        }

        /// <summary>
        /// Xuất dữ liệu bảng ra Excel.xls
        /// </summary>
        /// <param name="sourceTable">Bảng dữ liệu nguồn</param>
        /// <param name="fileName">Tên file sẽ lưu</param>
        /// <param name="Decimals">Định dạng số chữ số thập phân</param>
        public static void exportToExcel(DataTable sourceTable, string fileName, int Decimals, string strTitle)
        {
            if (sourceTable.Rows.Count <= 0)
                throw new Exception("No data!");
            System.IO.StreamWriter excelDoc;

            excelDoc = new System.IO.StreamWriter(fileName);
            string startExcelXML = "<xml version>\r\n<Workbook " +
                  "xmlns=\"urn:schemas-microsoft-com:office:spreadsheet\"\r\n" +
                  " xmlns:o=\"urn:schemas-microsoft-com:office:office\"\r\n " +
                  "xmlns:x=\"urn:schemas-    microsoft-com:office:" +
                  "excel\"\r\n xmlns:ss=\"urn:schemas-microsoft-com:" +
                  "office:spreadsheet\">\r\n <Styles>\r\n " +
                //==========Add by huuan============
                  "<Style ss:ID=\"Title\">" +
                    "<Alignment ss:Horizontal=\"Center\" ss:Vertical=\"Bottom\"/>" +
                    "<Borders>" +
                    "<Border ss:Position=\"Bottom\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"/>" +
                    "<Border ss:Position=\"Left\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"/>" +
                    "<Border ss:Position=\"Right\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"/>" +
                    "<Border ss:Position=\"Top\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"/>" +
                    "</Borders>" +
                    "<Font x:Family=\"Arial\" ss:Size=\"16\" ss:Bold=\"1\"/>" +
                  "</Style>" +
                //==================================
                  "<Style ss:ID=\"Default\" ss:Name=\"Normal\">\r\n " +
                  "<Alignment ss:Vertical=\"Bottom\"/>\r\n <Borders/>" +
                  "\r\n <Font/>\r\n <Interior/>\r\n <NumberFormat/>" +
                  "\r\n <Protection/>\r\n </Style>\r\n " +
                  "<Style ss:ID=\"BoldColumn\">\r\n <Font " +
                  "x:Family=\"Swiss\" ss:Bold=\"1\"/>\r\n </Style>\r\n " +
                  "<Style     ss:ID=\"StringLiteral\">\r\n <NumberFormat" +
                  " ss:Format=\"@\"/>\r\n </Style>\r\n <Style " +
                  "ss:ID=\"Decimal\">\r\n <NumberFormat " +
                  "ss:Format=\"" + decimalFormatPlace(Decimals) +
                  "\"/>\r\n </Style>\r\n " +
                  "<Style ss:ID=\"Integer\">\r\n <NumberFormat " +
                  "ss:Format=\"0\"/>\r\n </Style>\r\n <Style " +
                  "ss:ID=\"DateLiteral\">\r\n <NumberFormat " +
                  "ss:Format=\"dd/mm/yyyy;@\"/>\r\n </Style>\r\n " +
                  "</Styles>\r\n ";
            const string endExcelXML = "</Workbook>";

            int rowCount = 0;
            int sheetCount = 1;
            /*
           <xml version>
           <Workbook xmlns="urn:schemas-microsoft-com:office:spreadsheet"
           xmlns:o="urn:schemas-microsoft-com:office:office"
           xmlns:x="urn:schemas-microsoft-com:office:excel"
           xmlns:ss="urn:schemas-microsoft-com:office:spreadsheet">
           <Styles>
           <Style ss:ID="Default" ss:Name="Normal">
             <Alignment ss:Vertical="Bottom"/>
             <Borders/>
             <Font/>
             <Interior/>
             <NumberFormat/>
             <Protection/>
           </Style>
           <Style ss:ID="BoldColumn">
             <Font x:Family="Swiss" ss:Bold="1"/>
           </Style>
           <Style ss:ID="StringLiteral">
             <NumberFormat ss:Format="@"/>
           </Style>
           <Style ss:ID="Decimal">
             <NumberFormat ss:Format="0.0000"/>
           </Style>
           <Style ss:ID="Integer">
             <NumberFormat ss:Format="0"/>
           </Style>
           <Style ss:ID="DateLiteral">
             <NumberFormat ss:Format="mm/dd/yyyy;@"/>
           </Style>
           </Styles>
           <Worksheet ss:Name="Sheet1">
           </Worksheet>
           </Workbook>
           */
            excelDoc.Write(startExcelXML);
            excelDoc.Write("<Worksheet ss:Name=\"Sheet" + sheetCount + "\">");
            excelDoc.Write("<Table>");

            //======================Thêm dòng tiêu đề =================================
            //<Row ss:Height="20.25">
            //    <Cell ss:MergeAcross="3" ss:StyleID="s25"><Data ss:Type="String">Trộn abcd</Data></Cell>
            //</Row>
            excelDoc.Write("<Row ss:Height=\"20.25\">");
            excelDoc.Write("<Cell ss:MergeAcross=\"" + (sourceTable.Columns.Count - 1) + "\" ss:StyleID=\"Title\"><Data ss:Type=\"String\">" + strTitle + "</Data></Cell>");
            excelDoc.Write("</Row>");
            //==================================================================================

            excelDoc.Write("<Row>");
            for (int x = 0; x < sourceTable.Columns.Count; x++)
            {
                excelDoc.Write("<Cell ss:StyleID=\"BoldColumn\"><Data ss:Type=\"String\">");
                excelDoc.Write(sourceTable.Columns[x].ColumnName);
                excelDoc.Write("</Data></Cell>");
            }
            excelDoc.Write("</Row>");
            foreach (DataRow x in sourceTable.Rows)
            {
                rowCount++;
                //if the number of rows is > 64000 create a new page to continue output

                if (rowCount == 64000)
                {
                    rowCount = 0;
                    sheetCount++;
                    excelDoc.Write("</Table>");
                    excelDoc.Write(" </Worksheet>");
                    excelDoc.Write("<Worksheet ss:Name=\"Sheet" + sheetCount + "\">");
                    excelDoc.Write("<Table>");
                }
                excelDoc.Write("<Row>"); //ID=" + rowCount + "

                for (int y = 0; y < sourceTable.Columns.Count; y++)
                {
                    System.Type rowType;
                    rowType = x[y].GetType();
                    switch (rowType.ToString())
                    {
                        case "System.String":
                            string XMLstring = x[y].ToString();
                            XMLstring = XMLstring.Trim();
                            XMLstring = XMLstring.Replace("&", "&");
                            XMLstring = XMLstring.Replace(">", ">");
                            XMLstring = XMLstring.Replace("<", "<");
                            excelDoc.Write("<Cell ss:StyleID=\"StringLiteral\">" +
                                           "<Data ss:Type=\"String\">");
                            excelDoc.Write(XMLstring);
                            excelDoc.Write("</Data></Cell>");
                            break;
                        case "System.DateTime":
                            //Excel has a specific Date Format of YYYY-MM-DD followed by  

                            //the letter 'T' then hh:mm:sss.lll Example 2005-01-31T24:01:21.000

                            //The Following Code puts the date stored in XMLDate

                            //to the format above

                            DateTime XMLDate = (DateTime)x[y];
                            string XMLDatetoString = ""; //Excel Converted Date

                            XMLDatetoString = XMLDate.Year.ToString() +
                                 "-" +
                                 (XMLDate.Month < 10 ? "0" +
                                 XMLDate.Month.ToString() : XMLDate.Month.ToString()) +
                                 "-" +
                                 (XMLDate.Day < 10 ? "0" +
                                 XMLDate.Day.ToString() : XMLDate.Day.ToString()) +
                                 "T" +
                                 (XMLDate.Hour < 10 ? "0" +
                                 XMLDate.Hour.ToString() : XMLDate.Hour.ToString()) +
                                 ":" +
                                 (XMLDate.Minute < 10 ? "0" +
                                 XMLDate.Minute.ToString() : XMLDate.Minute.ToString()) +
                                 ":" +
                                 (XMLDate.Second < 10 ? "0" +
                                 XMLDate.Second.ToString() : XMLDate.Second.ToString()) +
                                 ".000";
                            excelDoc.Write("<Cell ss:StyleID=\"DateLiteral\">" +
                                         "<Data ss:Type=\"DateTime\">");
                            excelDoc.Write(XMLDatetoString);
                            excelDoc.Write("</Data></Cell>");
                            break;
                        case "System.Boolean":
                            excelDoc.Write("<Cell ss:StyleID=\"StringLiteral\">" +
                                        "<Data ss:Type=\"String\">");
                            excelDoc.Write(x[y].ToString());
                            excelDoc.Write("</Data></Cell>");
                            break;
                        case "System.Int16":
                        case "System.Int32":
                        case "System.Int64":
                        case "System.Byte":
                            excelDoc.Write("<Cell ss:StyleID=\"Integer\">" +
                                    "<Data ss:Type=\"Number\">");
                            excelDoc.Write(x[y].ToString());
                            excelDoc.Write("</Data></Cell>");
                            break;
                        case "System.Decimal":
                        case "System.Double":
                            excelDoc.Write("<Cell ss:StyleID=\"Decimal\">" +
                                  "<Data ss:Type=\"Number\">");
                            excelDoc.Write(x[y].ToString());
                            excelDoc.Write("</Data></Cell>");
                            break;
                        case "System.DBNull":
                            excelDoc.Write("<Cell ss:StyleID=\"StringLiteral\">" +
                                  "<Data ss:Type=\"String\">");
                            excelDoc.Write("");
                            excelDoc.Write("</Data></Cell>");
                            break;
                        default:
                            throw (new Exception(rowType.ToString() + " not handled."));
                    }
                }
                excelDoc.Write("</Row>");
            }
            excelDoc.Write("</Table>");
            excelDoc.Write(" </Worksheet>");
            excelDoc.Write(endExcelXML);
            excelDoc.Close();
        }
    }
}