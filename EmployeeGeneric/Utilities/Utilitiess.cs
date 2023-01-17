using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using HtmlAgilityPack;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Document = iTextSharp.text.Document;
using PageSize = iTextSharp.text.PageSize;

namespace EmployeeGeneric.Utilities
{
    public class Utilitiess
    {
        public static byte[] ConvertHtmlToPDF(string GridHtml)
        {
            HtmlNode.ElementsFlags["img"] = HtmlElementFlag.Closed;
            HtmlNode.ElementsFlags["input"] = HtmlElementFlag.Closed;
            HtmlNode.ElementsFlags["br"] = HtmlElementFlag.Closed;
            HtmlNode.ElementsFlags["hr"] = HtmlElementFlag.Closed;

            HtmlDocument doc = new();
            doc.OptionFixNestedTags = true;
            doc.LoadHtml(GridHtml);

            GridHtml = doc.DocumentNode.OuterHtml;

            using (MemoryStream stream = new())
            {
                var sr = new MemoryStream(Encoding.UTF8.GetBytes(GridHtml));
                Document pdfDoc;
                pdfDoc = new Document(PageSize.A3, 40f, 40f, 40f, 20f);

                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                pdfDoc.Open();

                XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr, null, Encoding.UTF8, FontFactory.FontImp);
                pdfDoc.Close();

                return stream.ToArray();
            }
        }

        public static byte[] PrintPaperClaimValueinWords(AllClaimData PaperClaimData, string webRootPath)
        {
            try
            {
                object sourceFile = webRootPath + "/Templates/PaperClaim/CMS-1500-Form-Template-with-watermark.docx"; //CMS-1500-Form-Template.docx
                var d = "ClaimForm_" + DateTime.Now.ToString("HHmmssfff");
                object destinationFile = webRootPath + "/Templates/PaperClaim/" + d + ".docx";

                //System.IO.File.Copy((string)sourceFile, (string)destinationFile, true);

                Dictionary<string, string> dict = new()
                {
                    { "strInsuranceName", PaperClaimData.StrInsuranceName },
                    { "strInsuranceAddress1", PaperClaimData.StrInsuranceAddress1 },
                    { "strInsuranceAddress2", PaperClaimData.StrInsuranceAddress2 },
                    { "strInsuranceCityStateZip", PaperClaimData.StrInsuranceCityStateZip },
                    //dict.Add("strInsuranceName", PaperClaimData.strInsuranceName);

                    { "StrBox1a", PaperClaimData.StrBox1a },
                    { "StrBox2", PaperClaimData.StrBox2 },
                    //DOB_MM_DD_YY
                    //dict.Add("StrBox3_DOB", PaperClaimData.StrBox3_MM + " " + PaperClaimData.StrBox3_DD + " " + PaperClaimData.StrBox3_YY);

                    { "StrBox3dm", PaperClaimData.StrBox3_MM },
                    { "StrBox3dd", PaperClaimData.StrBox3_DD },
                    { "StrBox3dy", PaperClaimData.StrBox3_YY },

                    { "StrBox3M", PaperClaimData.StrBox3_SEX == "M" ? "X" : "" },
                    { "StrBox3F", PaperClaimData.StrBox3_SEX != "M" ? "X" : "" },
                    { "StrBox4", PaperClaimData.StrBox4 },
                    { "StrBox5_street", PaperClaimData.StrBox5_street },
                    { "StrBox6s", PaperClaimData.StrBox6 == "18" ? "X" : " " },
                    { "StrBox6w", "" },
                    { "StrBox6c", "" },
                    { "StrBox6o", "" },
                    { "StrBox7_street", PaperClaimData.StrBox7_street },
                    { "StrBox5_city", PaperClaimData.StrBox5_city },
                    { "StrBox5_state", PaperClaimData.StrBox5_state },
                    { "StrBox7_city", PaperClaimData.StrBox7_city },
                    { "StrBox7_state", PaperClaimData.StrBox7_state },
                    { "StrBox5_zip", PaperClaimData.StrBox5_zip },
                    { "StrBox5_TelePhone", PaperClaimData.StrBox5_TelePhone },
                    { "StrBox7_zip", "" },
                    { "StrBox7_TelePhone", "" },
                    { "StrBox9", PaperClaimData.StrBox9 },
                    { "StrBox11", PaperClaimData.StrBox11 },

                    { "StrBox9a", PaperClaimData.StrBox9a },
                    { "StrBox10ay", "" },
                    { "StrBox10an", "X" },
                    //dict.Add("StrBox11a_M_D_Y", PaperClaimData.StrBox11a_MM + " " + PaperClaimData.StrBox11a_DD + " " + PaperClaimData.StrBox11a_YY);

                    { "StrBox11aM", PaperClaimData.StrBox11a_MM },
                    { "StrBox11aD", PaperClaimData.StrBox11a_DD },
                    { "StrBox11aY", PaperClaimData.StrBox11a_YY },

                    //StrBox11a_M_D_Y

                    { "StrBox11a_M", PaperClaimData.StrBox11a_SEX == "M" ? "X" : "" },
                    { "StrBox11a_F", PaperClaimData.StrBox11a_SEX == "F" ? "X" : "" },

                    //StrBox11a_M_F

                    { "StrBox9b", PaperClaimData.StrBox9b },
                    { "StrBox10by", "" },
                    { "StrBox10bn", "X" },
                    { "StrBox10bs", PaperClaimData.StrBox10bs },

                    { "StrBox11b", PaperClaimData.StrBox11b },

                    //

                    { "StrBox9c", PaperClaimData.StrBox9c },
                    { "StrBox10cy", "" },
                    { "StrBox10cn", "X" },
                    { "StrBox11c", PaperClaimData.StrBox11c },

                    { "StrBox9d", PaperClaimData.StrBox9d },
                    { "StrBox10d", PaperClaimData.StrBox10d },
                    //dict.Add("StrBox11d", PaperClaimData.StrBox11d != "YES" ? "      X" : "X");

                    { "StrBox11dy", PaperClaimData.StrBox11d == "YES" ? "X" : "" },
                    { "StrBox11dn", PaperClaimData.StrBox11d != "YES" ? "X" : "" },

                    //StrBox11dy

                    //

                    { "StrBox12_MMDDYYYY", String.IsNullOrEmpty(PaperClaimData.StrBox12_Date) ? DateTime.Now.ToString("MMddyyyy") : PaperClaimData.StrBox12_Date },

                    //dict.Add("StrBox14_M_D_Y", PaperClaimData.StrBox14_MM + " " + PaperClaimData.StrBox14_DD + " " + PaperClaimData.StrBox14_YY);

                    { "StrBox14_M", PaperClaimData.Strbox14_MM },
                    { "StrBox14_D", PaperClaimData.StrBox14_DD },
                    { "StrBox14_Y", PaperClaimData.StrBox14_YY },

                    { "StrBox14q", PaperClaimData.Strbox14q },
                    { "StrBox15q", PaperClaimData.Strbox15q },
                    //dict.Add("StrBox15_M_D_Y", PaperClaimData.StrBox15_MM + " " + PaperClaimData.StrBox15_DD + " " + PaperClaimData.StrBox15_YY);

                    { "StrBox15_M", PaperClaimData.Strbox15_MM },
                    { "StrBox15_D", PaperClaimData.StrBox15_DD },
                    { "StrBox15_Y", PaperClaimData.StrBox15_YY },

                    //dict.Add("StrBox16F_M_D_Y", PaperClaimData.StrBox16F_MM + " " + PaperClaimData.StrBox16F_DD + " " + PaperClaimData.StrBox16F_YY);

                    { "StrBox16F_M", PaperClaimData.Strbox16F_MM },
                    { "StrBox16F_D", PaperClaimData.Strbox16F_DD },
                    { "StrBox16F_Y", PaperClaimData.Strbox16F_YY },

                    //dict.Add("StrBox16T_M_D_Y", PaperClaimData.StrBox16T_MM + " " + PaperClaimData.StrBox16T_DD + " " + PaperClaimData.StrBox16T_YY);

                    { "StrBox16T_M", PaperClaimData.Strbox16T_MM },
                    { "StrBox16T_D", PaperClaimData.Strbox16T_DD },
                    { "StrBox16T_Y", PaperClaimData.Strbox16T_YY },

                    { "StrBox17", PaperClaimData.StrBox17 },
                    { "StrBox17a", PaperClaimData.StrBox17a },
                    { "StrBox17b", PaperClaimData.StrBox17b },

                    //dict.Add("StrBox18F_M_D_Y", PaperClaimData.StrBox18F_MM + " " + PaperClaimData.StrBox18F_DD + " " + PaperClaimData.StrBox18F_YY);

                    { "StrBox18F_M", PaperClaimData.Strbox18F_MM },
                    { "StrBox18F_D", PaperClaimData.Strbox18F_DD },
                    { "StrBox18F_Y", PaperClaimData.Strbox18F_YY },

                    //dict.Add("StrBox18T_M_D_Y", PaperClaimData.StrBox18T_MM + " " + PaperClaimData.StrBox18T_DD + " " + PaperClaimData.StrBox18T_YY);

                    { "StrBox18T_M", PaperClaimData.Strbox18T_MM },
                    { "StrBox18T_D", PaperClaimData.Strbox18T_DD },
                    { "StrBox18T_Y", PaperClaimData.Strbox18T_YY },

                    { "StrBox19", PaperClaimData.StrBox19 },
                    { "StrBox20y", "" },
                    { "StrBox20n", "X" },
                    { "StrBox20chges", PaperClaimData.StrBox20 },

                    { "StrBox21ind", "0" },
                    { "StrBox21a", PaperClaimData.StrBox21A },
                    { "StrBox21b", PaperClaimData.StrBox21B },
                    { "StrBox21c", PaperClaimData.StrBox21C },
                    { "StrBox21d", PaperClaimData.StrBox21D },
                    { "StrBox22c", PaperClaimData.StrBox22 },
                    { "StrBox22ref", PaperClaimData.StrBox22ref },

                    { "StrBox21e", PaperClaimData.StrBox21E },
                    { "StrBox21f", PaperClaimData.StrBox21F },
                    { "StrBox21g", PaperClaimData.StrBox21G },
                    { "StrBox21h", PaperClaimData.StrBox21H },

                    { "StrBox21i", PaperClaimData.StrBox21I },
                    { "StrBox21j", PaperClaimData.StrBox21J },
                    { "StrBox21k", PaperClaimData.StrBox21K },
                    { "StrBox21l", PaperClaimData.StrBox21L },

                    { "StrBox23", PaperClaimData.StrBox23 }
                };

                for (int i = 0; i < PaperClaimData.LstBox24.Count; i++)
                {
                    //dict.Add("StrBoxPC"+(i + 1) + "_FD", PaperClaimData.LstBox24[i].StrBox24A_FMM + " " + PaperClaimData.LstBox24[i].StrBox24A_FDD + " " + PaperClaimData.LstBox24[i].StrBox24A_FYY);
                    //StrBoxPC1_FM
                    dict.Add("StrBoxPC" + (i + 1) + "_FM", PaperClaimData.LstBox24[i].StrBox24A_FMM);
                    dict.Add("StrBoxPC" + (i + 1) + "_FD", PaperClaimData.LstBox24[i].StrBox24A_FDD);
                    dict.Add("StrBoxPC" + (i + 1) + "_FY", PaperClaimData.LstBox24[i].StrBox24A_FYY);

                    //dict.Add("StrBoxPC" + (i + 1) + "_TD", PaperClaimData.LstBox24[i].StrBox24A_TMM + " " + PaperClaimData.LstBox24[i].StrBox24A_TDD + " " + PaperClaimData.LstBox24[i].SBrbox24A_TYY);
                    //StrBoxPC1_TD
                    dict.Add("StrBoxPC" + (i + 1) + "_TM", PaperClaimData.LstBox24[i].StrBox24A_TMM);
                    dict.Add("StrBoxPC" + (i + 1) + "_TD", PaperClaimData.LstBox24[i].StrBox24A_TDD);
                    dict.Add("StrBoxPC" + (i + 1) + "_TY", PaperClaimData.LstBox24[i].StrBox24A_TYY);

                    dict.Add("StrBoxPC" + (i + 1) + "b", PaperClaimData.LstBox24[i].StrBox24B);
                    dict.Add("StrBoxPC" + (i + 1) + "c", PaperClaimData.LstBox24[i].StrBox24C);
                    dict.Add("StrBoxPC" + (i + 1) + "cd", PaperClaimData.LstBox24[i].StrBox24D_CPT);
                    dict.Add("StrBoxPC" + (i + 1) + "1", PaperClaimData.LstBox24[i].StrBox24D_M1);
                    dict.Add("StrBoxPC" + (i + 1) + "2", PaperClaimData.LstBox24[i].StrBox24D_M2);
                    dict.Add("StrBoxPC" + (i + 1) + "3", PaperClaimData.LstBox24[i].StrBox24D_M3);
                    dict.Add("StrBoxPC" + (i + 1) + "4", PaperClaimData.LstBox24[i].StrBox24D_M4);
                    dict.Add("StrBoxPC" + (i + 1) + "e", PaperClaimData.LstBox24[i].StrBox24E);
                    dict.Add("StrBoxPC" + (i + 1) + "fc", PaperClaimData.LstBox24[i].StrBox24F_ChargeInt);
                    dict.Add("StrBoxPC" + (i + 1) + "d", PaperClaimData.LstBox24[i].StrBox24F_ChargeDecimal);
                    dict.Add("StrBoxPC" + (i + 1) + "g", PaperClaimData.LstBox24[i].StrBox24G);
                    dict.Add("StrBoxPC" + (i + 1) + "h", PaperClaimData.LstBox24[i].StrBox24H);
                    dict.Add("StrBoxPC" + (i + 1) + "j", PaperClaimData.LstBox24[i].StrBox24J);
                }

                dict.Add("StrBox25_Number", PaperClaimData.StrBox25_Number);
                //dict.Add("StrBox25_EinORSSN", PaperClaimData.StrBox25_EinORSSN == "S" ? "X" : "  X");

                dict.Add("StrBox25s", PaperClaimData.StrBox25_EinORSSN == "S" ? "X" : "");
                dict.Add("StrBox25e", PaperClaimData.StrBox25_EinORSSN != "S" ? "X" : "");

                //StrBox25s
                //StrBox25e
                dict.Add("StrBox26", PaperClaimData.StrBox26);
                dict.Add("StrBox28_int", PaperClaimData.StrBox28_int);
                dict.Add("StrBox28_decimal", PaperClaimData.StrBox28_decimal);
                dict.Add("StrBox29", PaperClaimData.StrBox29);
                dict.Add("StrBox29_decimal", "");

                dict.Add("StrBox31", PaperClaimData.StrBox31);

                dict.Add("StrBox32_1", PaperClaimData.StrBox32_1);
                dict.Add("StrBox32_2", PaperClaimData.StrBox32_2);
                dict.Add("StrBox32_3", PaperClaimData.StrBox32_3);
                dict.Add("StrBox32a", PaperClaimData.StrBox32a);
                dict.Add("StrBox32b", PaperClaimData.StrBox32b);

                dict.Add("StrBox33_ph", PaperClaimData.StrBox33_ph);
                dict.Add("StrBox33_1", PaperClaimData.StrBox33_1);
                dict.Add("StrBox33_2", PaperClaimData.StrBox33_2);
                dict.Add("StrBox33_3", PaperClaimData.StrBox33_3);
                dict.Add("StrBox33a", PaperClaimData.StrBox33a);
                dict.Add("StrBox33b", PaperClaimData.StrBox33b);

                return SearchAndReplace((string)sourceFile, dict);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static bool PropertyResult(object prop)
        {
            return (prop != null && (bool)prop == true);
        }

        public static IDictionary<string, object> ToDictionary(object instance)
        {
            if (instance == null)
                throw new NullReferenceException();

            // if an object is dynamic it will convert to IDictionary<string, object>
            var result = instance as IDictionary<string, object>;
            if (result != null)
                return result;

            return instance.GetType()
                .GetProperties()
                .ToDictionary(x => x.Name, x => x.GetValue(instance));
        }

        public static byte[] DownloadAsCsvOrExcel(IEnumerable<dynamic> response, string reportname)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            FileInfo newFile = new FileInfo(Guid.NewGuid().ToString());
            using ExcelPackage package = new ExcelPackage(newFile);
            ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(reportname);

            foreach (var row in response)
            {
                int col = 0;
                worksheet.Row(1).Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Row(1).Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#00ff99"));

                foreach (var header in ToDictionary(row))
                {
                    col += 1;
                    worksheet.Cells[1, col].Value = header.Key;
                    worksheet.Column(col).Width = 30;
                }
            }

            int newRow = 2;
            foreach (var row in response)
            {
                int newCol = 0;
                foreach (KeyValuePair<string, object> data in ToDictionary(row))
                {
                    newCol += 1;
                    worksheet.Cells[newRow, newCol].Value = data.Value;
                }
                newRow++;
            }
            worksheet.Cells["A1:XFD1"].Style.Font.Bold = true;
            worksheet.View.PageLayoutView = false;

            return package.GetAsByteArray();
        }

        internal static byte[] SearchAndReplace(string FilePath, Dictionary<string, string> dict)
        {
            MemoryStream ms = new();
            MemoryStream mem = new(System.IO.File.ReadAllBytes(FilePath));

            //using (WordprocessingDocument document = WordprocessingDocument.CreateFromTemplate(FilePath))
            using (WordprocessingDocument document = WordprocessingDocument.Open(mem, true))
            {
                //var tables = document.MainDocumentPart.Document.Descendants<Table>().ToList();

                IDictionary<String, BookmarkStart> bookmarkMap = new Dictionary<String, BookmarkStart>();

                foreach (BookmarkStart bookmarkStart in document.MainDocumentPart.RootElement.Descendants<BookmarkStart>())
                {
                    bookmarkMap[bookmarkStart.Name] = bookmarkStart;
                }

                foreach (BookmarkStart bookmarkStart in bookmarkMap.Values)
                {
                    Run bookmarkText = bookmarkStart.NextSibling<Run>();

                    if (bookmarkText != null)
                    {
                        var obj = dict.FirstOrDefault(k => k.Key == bookmarkStart.Name);

                        InsertIntoBookmark(bookmarkStart, obj.Value);
                    }
                }
            }
            return mem.ToArray();
        }

        public static void InsertIntoBookmark(BookmarkStart bookmarkStart, string text)
        {
            OpenXmlElement elem = bookmarkStart.NextSibling();

            while (elem != null && !(elem is BookmarkEnd))
            {
                OpenXmlElement nextElem = elem.NextSibling();
                elem.Remove();
                elem = nextElem;
            }

            bookmarkStart.Parent.InsertAfter<Run>(new Run(new Text(text)), bookmarkStart);
        }
    }
}
