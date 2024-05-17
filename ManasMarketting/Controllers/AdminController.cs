using BusinessLogic.Repository;
using Microsoft.AspNetCore.Mvc;
using ManasMarketting.Models;
using ManasMarketting.Controllers;
using System.Data;
using System.Data.SqlClient;
using NuGet.Protocol.Plugins;
using System.Data.OleDb;
using OfficeOpenXml;
using System.DirectoryServices.Protocols;
using System.Composition;
using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.IO;

namespace ManasMarketting.Controllers
{
    public class AdminController : Controller
    {
        readonly IAdmin _iAdmin;
        readonly IHttpContextAccessor _accessor;
        private readonly IWebHostEnvironment _env;
        readonly ISession _session;

        public AdminController(IAdmin iAdmin, IHttpContextAccessor accessor, IWebHostEnvironment env, IHttpContextAccessor session)
        {
            _iAdmin = iAdmin;
            _accessor = accessor;
            _env = env;
            _session = session.HttpContext.Session;
        }
        public IActionResult Index()
        {
            Dashboard res = new Dashboard();
            int vendor_count = _iAdmin.GetVendorList().Count();
            decimal total_sales = _iAdmin.GetSalesListById(new tbl_sales_viewmodel()).ToList().Select(s => new { s.vendor_id, s.BILL_AMOUNT }).Distinct().Sum(s => s.BILL_AMOUNT);
            res.vendor_count = vendor_count;
            res.total_sales = total_sales;
            return View(res);
        }
        public IActionResult VendorList()
        {
            var data = _iAdmin.GetVendorList();
            return View(data);
        }

        public IActionResult SupplierList()
        {
            var data = _iAdmin.GetSupplierList();

            return View(data);
        }

        public IActionResult SalesReport(int vendor_id)
        {
            var model = _iAdmin.GetVendor();
            ViewBag.vendor_id = vendor_id;
            return View(model);
        }

        public IActionResult PurchaseReport()
        {
            var model = _iAdmin.GetSupplier();
            //viewbag.supplier_id = supplier_id;
            return View(model);
        }



        [HttpPost]
        public PartialViewResult _UpdateSupplier(tbl_supplier_viewmodel data)
        {
            data.state_list = _iAdmin.GetState();
            data.city_list = _iAdmin.GetCity(25);
            return PartialView("Partial/_UpdateSupplier", data);
        }

        [HttpPost]
        public PartialViewResult _UpdateVendor(tbl_vendor_viewmodel data)
        {
            data.state_list = _iAdmin.GetState();
            data.city_list = _iAdmin.GetCity(0);
            return PartialView("Partial/_UpdateVendor", data);
        }

        [HttpPost]
        public JsonResult Getcity(int state_id)
        {
            var list = _iAdmin.GetCity(state_id);
            return Json(list);
        }

        [HttpPost]
        public JsonResult GetGST(int supplier_id)
        {
            var list = _iAdmin.GetGST(supplier_id);
            return Json(list);
        }

        [HttpPost]
        public JsonResult AddUpdateVendor(tbl_vendor_viewmodel req)
        {
            req.entry_by = Convert.ToInt32(_session.GetInt32("emp_id"));
            return Json(_iAdmin.UpdateVendor(req));
        }

        [HttpPost]
        public JsonResult AddUpdateSupplier(tbl_supplier_viewmodel req)
        {
            req.entry_by = Convert.ToInt32(_session.GetInt32("emp_id"));
            return Json(_iAdmin.UpdateSupplier(req));
        }

        [HttpPost]
        public JsonResult UpdateSalesDetails(tbl_sales_viewmodel req)
        {
            req.entry_by = Convert.ToInt32(_session.GetInt32("emp_id"));
            var res = _iAdmin.UpdateSalesDetails(req);
            return Json(res);
        }


        [HttpPost]
        public JsonResult UpdatePurchaseDetails(tbl_purchase_viewmodel req)
        {
            //req.entry_by = Convert.ToInt32(_session.GetInt32("emp_id"));
            var res = _iAdmin.UpdatePurchaseDetails(req);
            return Json("Hello");
        }

        [HttpGet]
        public IActionResult AddUpdatePurcahse(int purcahse_id = 0)
        {
            tbl_purchase_viewmodel res = new tbl_purchase_viewmodel();



            res.supplier_list = _iAdmin.GetSupplierList();
            res.state_list = _iAdmin.GetState();
                res.city_list = _iAdmin.GetCity(25);
            


            return View(res);
        }

        [HttpPost]
        public PartialViewResult _GetDetailsByBillNo(tbl_sales_viewmodel req)
        {
            var list = _iAdmin._GetDetailsByBillNo(req);
            return PartialView("Partial/_GetDetailsByBillNo", list);
        }

        [HttpPost]
        public PartialViewResult _GetDetailsByInvoiceNo(tbl_purchase_viewmodel req)
        {
            //var list = _iAdmin._GetDetailsByBillNo(req);
            return PartialView("Partial/_GetDetailsByInvoiceNo");
        }

        [HttpPost]
        public IActionResult _UpdateInvoiceDetails(tbl_purchase_viewmodel req)
        {
            return PartialView("Partial/_UpdateInvoiceDetails", req);
        }

        [HttpPost]
        public IActionResult _UpdateBilDetails(tbl_sales_viewmodel req)
        {
            return PartialView("Partial/_UpdateBillDetails", req);
        }

        public IActionResult AddUpdateSales(int sales_id = 0)
        {
            tbl_sales_viewmodel data = new tbl_sales_viewmodel();
            if (sales_id == 0)
            {
                data.vendor_list = _iAdmin.GetVendor();
                data.state_list = _iAdmin.GetState();
                data.city_list = _iAdmin.GetCity(0);
            }
            else
            {
                data = _iAdmin.EditSales(sales_id);
                data.vendor_list = _iAdmin.GetVendor();
                data.state_list = _iAdmin.GetState();
                data.city_list = _iAdmin.GetCity(0);
            }
            return View(data);
        }

        [HttpGet]
        public ActionResult UploadPurcahseExcel()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadPurcahseExcel(IFormFile postedFile)
        {
            Response res = new Response()
            {
                status = 0,
                msg = "Something went wrong"
            };
            try
            {
                string filePath = string.Empty;
                if (postedFile != null)
                {
                    string path = Path.Combine(_env.WebRootPath, "Uploads");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    string extension = Path.GetExtension(postedFile.FileName);
                    string file_name = DateTime.Now.Ticks.ToString() + extension;
                    filePath = Path.Combine(path, file_name);
                    using (FileStream fs = System.IO.File.Create(filePath))
                    {
                        postedFile.CopyTo(fs);
                    }

                    string conString = string.Empty;
                    switch (extension)
                    {
                        case ".xls":
                            conString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;HDR=YES'";
                            break;
                        case ".xlsx":
                            conString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 8.0;HDR=YES;IMEX=2'";
                            break;
                    }

                    DataTable dt = new DataTable();

                    // Add columns to the DataTable
                    dt.Columns.Add("SNO", typeof(int));
                    dt.Columns.Add("INVOICE NUMBER", typeof(string));
                    dt.Columns.Add("INVOICE DATE", typeof(DateTime));
                    dt.Columns.Add("INVOICE VALUE", typeof(decimal));
                    dt.Columns.Add("HSN CODE", typeof(string));
                    dt.Columns.Add("DESCRIPTION", typeof(string));
                    dt.Columns.Add("TOTAL QUANTITY", typeof(int));
                    dt.Columns.Add("GROSS AMOUNT", typeof(decimal));
                    dt.Columns.Add("Tax 3-IGST INPUT 12%", typeof(decimal));
                    dt.Columns.Add("Tax 3-IGST INPUT 5%", typeof(decimal));
                    dt.Columns.Add("PURCHASE ACCOUNT NAME", typeof(string));
                    dt.Columns.Add("TOTAL TAX PERCENT", typeof(decimal));
                    dt.Columns.Add("IGST", typeof(decimal));
                    dt.Columns.Add("TCS", typeof(decimal));
                    dt.Columns.Add("NET  AMOUNT", typeof(decimal));
                    dt.Columns.Add("INVOICE TYPE", typeof(string));
                    dt.Columns.Add("REVERSE CHARGE", typeof(string));
                    dt.Columns.Add("PARTY NAME", typeof(string));
                    dt.Columns.Add("GST NO", typeof(string));
                    dt.Columns.Add("GST STATE CODE", typeof(string));
                    dt.Columns.Add("GST STATE NAME", typeof(string));
                    dt.Columns.Add("INTER OR INTRA", typeof(string));

                    conString = string.Format(conString, filePath);

                    using (OleDbConnection connExcel = new OleDbConnection(conString))
                    {
                        using (OleDbCommand cmdExcel = new OleDbCommand())
                        {
                            using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                            {
                                cmdExcel.Connection = connExcel;

                                //Get the name of First Sheet.
                                connExcel.Open();
                                DataTable dtExcelSchema = new DataTable();
                                dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                                connExcel.Close();
                                string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                                connExcel.Open();
                                cmdExcel.CommandText = "SELECT * From [" + sheetName + "]";
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(dt);
                                connExcel.Close();

                                DataTable dt1 = new DataTable();
                                dt1.Columns.Add("supplier_name", typeof(string));
                                dt1.Columns.Add("city_name", typeof(string));
                                dt1.Columns.Add("invoice_number", typeof(string));
                                dt1.Columns.Add("invoice_date", typeof(string));
                                dt1.Columns.Add("invoice_value", typeof(string));
                                dt1.Columns.Add("hsn_code", typeof(string));
                                dt1.Columns.Add("description", typeof(string));
                                dt1.Columns.Add("total_quantity", typeof(int));
                                dt1.Columns.Add("gross_amount", typeof(decimal));
                                dt1.Columns.Add("tax_3_igst_input_12", typeof(decimal));
                                dt1.Columns.Add("tax_3_igst_input_5", typeof(decimal));
                                dt1.Columns.Add("purchase_account_name", typeof(string));
                                dt1.Columns.Add("total_tax_percent", typeof(decimal));
                                dt1.Columns.Add("igst", typeof(decimal));
                                dt1.Columns.Add("tcs", typeof(decimal));
                                dt1.Columns.Add("net_amount", typeof(decimal));
                                dt1.Columns.Add("invoice_type", typeof(string));
                                dt1.Columns.Add("reverse_charge", typeof(string));
                                //dt1.Columns.Add("party_name", typeof(string));
                                dt1.Columns.Add("gst_no", typeof(string));
                                dt1.Columns.Add("gst_state_code", typeof(string));
                                dt1.Columns.Add("gst_state_name", typeof(string));
                                dt1.Columns.Add("inter_or_intra", typeof(string));


                                foreach (DataRow row in dt.Rows)
                                {

                                    var supplier_name = row["PARTY NAME"].ToString();
                                    var gst_no = Convert.ToString(row["GST NO"]);
                                    if (supplier_name != "" && gst_no != null)
                                    {
                                        DataRow dr = dt1.NewRow();
                                        dr["supplier_name"] = row["PARTY NAME"].ToString().Split("-")[0].Trim();
                                        dr["city_name"] = row["PARTY NAME"].ToString().Split("-")[1].Trim();
                                        dr["invoice_number"] = row["INVOICE NUMBER"];
                                        dr["invoice_date"] = row["INVOICE DATE"];
                                        dr["invoice_value"] = row["INVOICE VALUE"];
                                        dr["hsn_code"] = row["HSN CODE"];
                                        dr["description"] = row["DESCRIPTION"];
                                        dr["total_quantity"] = row["TOTAL QUANTITY"];
                                        dr["gross_amount"] = row["GROSS AMOUNT"];
                                        dr["tax_3_igst_input_12"] = row["Tax 3-IGST INPUT 12%"];
                                        dr["tax_3_igst_input_5"] = row["Tax 3-IGST INPUT 5%"];
                                        dr["purchase_account_name"] = row["PURCHASE ACCOUNT NAME"];
                                        dr["total_tax_percent"] = row["TOTAL TAX PERCENT"];
                                        dr["igst"] = row["IGST"];
                                        dr["tcs"] = row["TCS"];
                                        dr["net_amount"] = row["NET  AMOUNT"];
                                        dr["invoice_type"] = row["INVOICE TYPE"];
                                        dr["reverse_charge"] = row["REVERSE CHARGE"];
                                        dr["gst_no"] = row["GST NO"];
                                        dr["gst_state_code"] = row["GST STATE CODE"];
                                        dr["gst_state_name"] = row["GST STATE NAME"];
                                        dr["inter_or_intra"] = row["INTER OR INTRA"];

                                        dt1.Rows.Add(dr);
                                    }
                                }
                                res = _iAdmin.UploadExcelFile(dt1);
                            }
                        }
                    }

                }
                res.status = 1;
                res.msg = "File uploaded successfully";
            }
            catch (Exception ex)
            {
                res.msg = ex.Message;
            }

            return Json(res);
        }

        [HttpGet]
        public ActionResult UploadBulkExcel()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UploadBulkExcel(IFormFile postedFile)
        {
            Response res = new Response()
            {
                status = 0,
                msg = "Something went wrong"
            };

            try
            {
                string filePath = string.Empty;
                if (postedFile != null)
                {
                    //string path = Server.MapPath("~/Uploads/");
                    string path = Path.Combine(_env.WebRootPath, "Uploads");

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    string extension = Path.GetExtension(postedFile.FileName);
                    string file_name = DateTime.Now.Ticks.ToString() + extension;
                    filePath = path + "/" + file_name;
                    using (FileStream fs = System.IO.File.Create(filePath))
                    {
                        postedFile.CopyTo(fs);
                    };
                    //postedFile.CopyTo();
                    string conString = string.Empty;
                    switch (extension)
                    {
                        case ".xls": //Excel 97-03.
                            conString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;HDR=YES'";
                            break;
                        case ".xlsx": //Excel 07 and above.
                            conString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 8.0;HDR=YES;IMEX=2'";
                            break;
                    }

                    DataTable dt = new DataTable();
                    dt.Columns.Add("SNO", typeof(int));
                    dt.Columns.Add("PARTY NAME", typeof(string));
                    dt.Columns.Add("GST NO", typeof(string));
                    dt.Columns.Add("BILL NO", typeof(string));
                    dt.Columns.Add("BILL DATE", typeof(string));
                    dt.Columns.Add("HSN CODE", typeof(string));
                    dt.Columns.Add("UD AMOUNT-2", typeof(string));
                    dt.Columns.Add("TOTAL QUANTITY", typeof(string));
                    dt.Columns.Add("TOTAL TAX(%)", typeof(string));
                    dt.Columns.Add("TOTAL CGST AMOUNT", typeof(string));
                    dt.Columns.Add("TOTAL SGST AMOUNT", typeof(string));
                    dt.Columns.Add("FRIEGHT", typeof(string));
                    dt.Columns.Add("GST SALE 12%", typeof(string));
                    dt.Columns.Add("SGST OUTPUT 6%", typeof(string));
                    dt.Columns.Add("CGST OUTPUT 6%", typeof(string));
                    dt.Columns.Add("GST SALE 5%", typeof(string));
                    dt.Columns.Add("CGST OUTPUT 2.5%", typeof(string));
                    dt.Columns.Add("NET AMOUNT", typeof(string));
                    dt.Columns.Add("BILL AMOUNT", typeof(string));

                    conString = string.Format(conString, filePath);

                    using (OleDbConnection connExcel = new OleDbConnection(conString))
                    {
                        using (OleDbCommand cmdExcel = new OleDbCommand())
                        {
                            using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                            {
                                cmdExcel.Connection = connExcel;

                                //Get the name of First Sheet.
                                connExcel.Open();
                                DataTable dtExcelSchema = new DataTable();
                                dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                                string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                                connExcel.Close();

                                //Read Data from First Sheet.
                                connExcel.Open();
                                cmdExcel.CommandText = "SELECT * From [" + sheetName + "]";
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(dt);
                                connExcel.Close();

                                DataTable dt1 = new DataTable();
                                dt1.Columns.Add("vendor_name", typeof(string));
                                dt1.Columns.Add("city_name", typeof(string));
                                dt1.Columns.Add("gst_no", typeof(string));
                                dt1.Columns.Add("bill_no", typeof(string));
                                dt1.Columns.Add("bill_date", typeof(string));
                                dt1.Columns.Add("hsn_code", typeof(string));
                                dt1.Columns.Add("amount", typeof(string));
                                dt1.Columns.Add("total_quantity", typeof(string));
                                dt1.Columns.Add("total_tax_per", typeof(string));
                                dt1.Columns.Add("total_cgst_amount", typeof(string));
                                dt1.Columns.Add("total_sgst_amount", typeof(string));
                                dt1.Columns.Add("frieght", typeof(string));
                                dt1.Columns.Add("bill_amount", typeof(string));

                                foreach (DataRow row in dt.Rows)
                                {
                                    var vendor_name = row["PARTY NAME"].ToString();
                                    var gst_no = Convert.ToString(row["GST NO"]);
                                    if (vendor_name != "" && gst_no != null)
                                    {
                                        DataRow dr = dt1.NewRow();
                                        dr["vendor_name"] = row["PARTY NAME"].ToString().Split(" -")[0].Trim();
                                        dr["city_name"] = row["PARTY NAME"].ToString().Split(" -")[1].Trim();
                                        dr["gst_no"] = row["GST NO"];
                                        dr["bill_no"] = row["BILL NO"];
                                        dr["bill_date"] = row["BILL DATE"];
                                        dr["hsn_code"] = row["HSN CODE"];
                                        dr["amount"] = row["UD AMOUNT-2"];
                                        dr["total_quantity"] = row["TOTAL QUANTITY"];
                                        dr["total_tax_per"] = row["TOTAL TAX(%)"];
                                        dr["total_cgst_amount"] = row["TOTAL CGST AMOUNT"];
                                        dr["total_sgst_amount"] = row["TOTAL SGST AMOUNT"];
                                        dr["frieght"] = row["FRIEGHT"];
                                        dr["bill_amount"] = row["BILL AMOUNT"];

                                        dt1.Rows.Add(dr);
                                    }
                                }

                                res = _iAdmin.UploadExcelFile(dt1);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                res.msg = ex.Message;
            }
            return Json(res);
        }
        [HttpPost]
        public JsonResult AddUpdateSales(tbl_sales_viewmodel req)
        {
            req.entry_by = Convert.ToInt32(_session.GetInt32("emp_id"));
            return Json(_iAdmin.UpdateSales(req));
        }


        [HttpPost]
        public JsonResult AddUpdatePurcahse(tbl_purchase_viewmodel req)
        {

            return Json(_iAdmin.UpdatePurchase(req));
            
        }
        public IActionResult GetSalesList(int Id = 0)
        {
            tbl_sales_viewmodel data = new tbl_sales_viewmodel();
            if (Id == 0)
            {
                data.vendor_list = _iAdmin.GetVendor();
            }
            else
            {
                data = (tbl_sales_viewmodel)TempData["mydata_" + Id];
                TempData["mydata_" + Id] = data;
            }

            return View(data);
        }
        [HttpPost]
        public PartialViewResult _GetPurchaseList(tbl_purchase_viewmodel req)
        {
            req.datefrom = null;
            req.dateto = null;
            var list = _iAdmin.GetPurchaseListById(req);
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            ExcelPackage Ep = new ExcelPackage();
            ExcelWorksheet Sheet = Ep.Workbook.Worksheets.Add("Report");
            Sheet.Cells["A1"].Value = "Sr.No.";
            Sheet.Cells["B1"].Value = "INVOICE NUMBER";
            Sheet.Cells["C1"].Value = "INVOICE DATE";
            Sheet.Cells["D1"].Value = "INVOICE VALUE";
            Sheet.Cells["E1"].Value = "HSN CODE";
            Sheet.Cells["F1"].Value = "DESCRIPTION";
            Sheet.Cells["G1"].Value = "TOTAL QUANTITY";
            Sheet.Cells["H1"].Value = "GROSS AMOUNT";
            Sheet.Cells["I1"].Value = "Tax 3-IGST INPUT 12%";
            Sheet.Cells["J1"].Value = "Tax 3-IGST INPUT 5%";
            Sheet.Cells["K1"].Value = "PURCHASE ACCOUNT NAME";
            Sheet.Cells["L1"].Value = "TOTAL TAX PERCENT";
            Sheet.Cells["M1"].Value = "IGST";
            Sheet.Cells["N1"].Value = "TCS";
            Sheet.Cells["O1"].Value = "NET_AMOUNT";
            Sheet.Cells["P1"].Value = "INVOICE TYPE";
            Sheet.Cells["Q1"].Value = "REVERSE CHARGE";
            Sheet.Cells["R1"].Value = "PARTY NAME";
            Sheet.Cells["S1"].Value = "GST NO";
            Sheet.Cells["T1"].Value = "GST STATE CODE";
            Sheet.Cells["U1"].Value = "GST STATE NAME";
            Sheet.Cells["V1"].Value = "INTER OR INTRA";



            int row = 2;
            byte flag = 0;
            int srno = 1;
            bool mergered = false;

            foreach (var item in list)
            {
                int rowspan = list.Where(w => w.invoice_number == item.invoice_number && w.invoice_number == item.invoice_number && w.invoice_value == item.invoice_value).Count();

                if (rowspan > 1)
                {
                    if (flag == 0)
                    {
                        Sheet.Cells[string.Format("A{0}", row)].Value = srno;
                        Sheet.Cells[string.Format("B{0}", row)].Value = item.invoice_number;
                        Sheet.Cells[string.Format("C{0}", row)].Value = item.invoice_date.ToString("dd-MM-yyyy HH:mm:ss");
                        Sheet.Cells[string.Format("D{0}", row)].Value = item.invoice_value;
                        Sheet.Cells[string.Format("E{0}", row)].Value = item.hsn_code;
                        Sheet.Cells[string.Format("F{0}", row)].Value = item.description;
                        Sheet.Cells[string.Format("G{0}", row)].Value = item.total_quantity;
                        Sheet.Cells[string.Format("H{0}", row)].Value = item.gross_amount;
                        Sheet.Cells[string.Format("I{0}", row)].Value = item.igst_12_amount;
                        Sheet.Cells[string.Format("J{0}", row)].Value = item.igst_5_amount;
                        Sheet.Cells[string.Format("K{0}", row)].Value = item.purchase_account_name;
                        Sheet.Cells[string.Format("L{0}", row)].Value = item.total_tax_per;
                        Sheet.Cells[string.Format("M{0}", row)].Value = item.igst;
                        Sheet.Cells[string.Format("N{0}", row)].Value = item.TCS;
                        Sheet.Cells[string.Format("O{0}", row)].Value = item.net_amount;
                        Sheet.Cells[string.Format("P{0}", row)].Value = item.invoice_value;
                        Sheet.Cells[string.Format("Q{0}", row)].Value = item.reverse_charge;
                        Sheet.Cells[string.Format("R{0}", row)].Value = item.Party_Name;
                        Sheet.Cells[string.Format("S{0}", row)].Value = item.gst_no;
                        Sheet.Cells[string.Format("T{0}", row)].Value = item.gst_state_name;
                        Sheet.Cells[string.Format("U{0}", row)].Value = item.gst_state_name;
                        Sheet.Cells[string.Format("V{0}", row)].Value = item.inter_or_intra;

                        flag = 1;
                        mergered = false;
                        srno++;
                    }
                    else
                    {


                        if (mergered == false)
                        {
                            int previousRow = row - 1;
                            int lastRow = row + rowspan - 2;

                            Sheet.Cells["B" + (row - 1) + ":B" + (row + rowspan - 2)].Merge = true;
                            Sheet.Cells["C" + (row - 1) + ":C" + (row + rowspan - 2)].Merge = true;
                            Sheet.Cells["D" + (row - 1) + ":D" + (row + rowspan - 2)].Merge = true;
                            Sheet.Cells["B" + previousRow + ":D" + lastRow].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                            mergered = true;
                        }
                        Sheet.Cells[string.Format("E{0}", row)].Value = item.hsn_code;
                        Sheet.Cells[string.Format("F{0}", row)].Value = item.description;
                        Sheet.Cells[string.Format("G{0}", row)].Value = item.total_quantity;
                        Sheet.Cells[string.Format("H{0}", row)].Value = item.gross_amount;
                        Sheet.Cells[string.Format("I{0}", row)].Value = item.igst_12_amount;
                        Sheet.Cells[string.Format("J{0}", row)].Value = item.igst_5_amount;
                        Sheet.Cells[string.Format("K{0}", row)].Value = item.purchase_account_name;
                        Sheet.Cells[string.Format("L{0}", row)].Value = item.total_tax_per;
                        Sheet.Cells[string.Format("M{0}", row)].Value = item.igst;
                        Sheet.Cells[string.Format("N{0}", row)].Value = item.TCS;
                        Sheet.Cells[string.Format("O{0}", row)].Value = item.net_amount;
                        Sheet.Cells[string.Format("P{0}", row)].Value = item.invoice_value;
                        Sheet.Cells[string.Format("Q{0}", row)].Value = item.reverse_charge;
                        Sheet.Cells[string.Format("R{0}", row)].Value = item.Party_Name;
                        Sheet.Cells[string.Format("S{0}", row)].Value = item.gst_no;
                        Sheet.Cells[string.Format("T{0}", row)].Value = item.gst_state_name;
                        Sheet.Cells[string.Format("U{0}", row)].Value = item.gst_state_name;
                        Sheet.Cells[string.Format("V{0}", row)].Value = item.inter_or_intra;


                    }
                }
                else
                {
                    flag = 0;
                    Sheet.Cells[string.Format("A{0}", row)].Value = srno;
                    Sheet.Cells[string.Format("B{0}", row)].Value = item.invoice_number;
                    Sheet.Cells[string.Format("C{0}", row)].Value = item.invoice_date;
                    Sheet.Cells[string.Format("D{0}", row)].Value = item.invoice_value;
                    Sheet.Cells[string.Format("E{0}", row)].Value = item.hsn_code;
                    Sheet.Cells[string.Format("F{0}", row)].Value = item.description;
                    Sheet.Cells[string.Format("G{0}", row)].Value = item.total_quantity;
                    Sheet.Cells[string.Format("H{0}", row)].Value = item.gross_amount;
                    Sheet.Cells[string.Format("I{0}", row)].Value = item.igst_12_amount;
                    Sheet.Cells[string.Format("J{0}", row)].Value = item.igst_5_amount;
                    Sheet.Cells[string.Format("K{0}", row)].Value = item.purchase_account_name;
                    Sheet.Cells[string.Format("L{0}", row)].Value = item.total_tax_per;
                    Sheet.Cells[string.Format("M{0}", row)].Value = item.igst;
                    Sheet.Cells[string.Format("N{0}", row)].Value = item.TCS;
                    Sheet.Cells[string.Format("O{0}", row)].Value = item.net_amount;
                    Sheet.Cells[string.Format("P{0}", row)].Value = item.invoice_value;
                    Sheet.Cells[string.Format("Q{0}", row)].Value = item.reverse_charge;
                    Sheet.Cells[string.Format("R{0}", row)].Value = item.Party_Name;
                    Sheet.Cells[string.Format("S{0}", row)].Value = item.gst_no;
                    Sheet.Cells[string.Format("T{0}", row)].Value = item.gst_state_name;
                    Sheet.Cells[string.Format("U{0}", row)].Value = item.gst_state_name;
                    Sheet.Cells[string.Format("V{0}", row)].Value = item.inter_or_intra;

                    srno++;
                    mergered = false;

                }
                row++;
            }
            Sheet.Cells["A:AZ"].AutoFitColumns();
            ViewBag.FileName = DateTime.Now.Ticks.ToString() + ".xlsx";
            string path = Path.Combine(_env.WebRootPath, "Temp1");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            System.IO.File.WriteAllBytes(path + "/" + ViewBag.FileName, Ep.GetAsByteArray());
            return PartialView("Partial/_GetPurchaseList", list);
        }
        [HttpPost]
        public PartialViewResult _GetSalesList(tbl_sales_viewmodel req)
        {
            var list = _iAdmin.GetSalesListById(req);

            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            ExcelPackage Ep = new ExcelPackage();
            ExcelWorksheet Sheet = Ep.Workbook.Worksheets.Add("Report");
            Sheet.Cells["A1"].Value = "Sr.No.";
            Sheet.Cells["B1"].Value = "Party Name";
            Sheet.Cells["C1"].Value = "City Name";
            Sheet.Cells["D1"].Value = "GST No";
            Sheet.Cells["E1"].Value = "Bill No";
            Sheet.Cells["F1"].Value = "Bill Date";
            Sheet.Cells["G1"].Value = "HSN Code";
            Sheet.Cells["H1"].Value = "UD Amount-2";
            Sheet.Cells["I1"].Value = "Total Quantity";
            Sheet.Cells["J1"].Value = "Total Tax(%)";
            Sheet.Cells["K1"].Value = "Total CGST Amount";
            Sheet.Cells["L1"].Value = "Total SGST Amount";
            Sheet.Cells["M1"].Value = "GST Sale 12%";
            Sheet.Cells["N1"].Value = "CGST Output 6%";
            Sheet.Cells["O1"].Value = "SGST Output 6%";
            Sheet.Cells["P1"].Value = "GST Sale 5%";
            Sheet.Cells["Q1"].Value = "CGST Output 2.5%";
            Sheet.Cells["R1"].Value = "SGST Output 2.5%";
            Sheet.Cells["S1"].Value = "Net Amount";
            Sheet.Cells["T1"].Value = "Frieght";
            Sheet.Cells["U1"].Value = "Bill Amount";

            int row = 2;
            byte flag = 0;
            int srno = 1;
            bool mergered = false;

            foreach (var item in list)
            {
                int rowspan = list.Where(w => w.vendor_id == item.vendor_id && w.BILL_NO == item.BILL_NO).Count();

                if (rowspan > 1)
                {
                    if (flag == 0)
                    {
                        Sheet.Cells[string.Format("A{0}", row)].Value = srno;
                        Sheet.Cells[string.Format("B{0}", row)].Value = item.vendor_name;
                        Sheet.Cells[string.Format("C{0}", row)].Value = item.city_name;
                        Sheet.Cells[string.Format("D{0}", row)].Value = item.GST_NO;
                        Sheet.Cells[string.Format("E{0}", row)].Value = item.BILL_NO;
                        Sheet.Cells[string.Format("F{0}", row)].Value = item.BILL_DATEe;
                        Sheet.Cells[string.Format("G{0}", row)].Value = item.hsn_code;
                        Sheet.Cells[string.Format("H{0}", row)].Value = item.UD_AMOUNT_2;
                        Sheet.Cells[string.Format("I{0}", row)].Value = item.TOTAL_QUANTITY;
                        Sheet.Cells[string.Format("J{0}", row)].Value = item.TOTAL_TAX;
                        Sheet.Cells[string.Format("K{0}", row)].Value = item.TOTAL_CGST_AMOUNT;
                        Sheet.Cells[string.Format("L{0}", row)].Value = item.TOTAL_SGST_AMOUNT;
                        Sheet.Cells[string.Format("M{0}", row)].Value = item.TOTAL_TAX == 12 ? item.TOTAL_CGST_AMOUNT + item.TOTAL_SGST_AMOUNT : "";
                        Sheet.Cells[string.Format("N{0}", row)].Value = item.TOTAL_TAX == 12 ? item.TOTAL_CGST_AMOUNT : "";
                        Sheet.Cells[string.Format("O{0}", row)].Value = item.TOTAL_TAX == 12 ? item.TOTAL_SGST_AMOUNT : "";
                        Sheet.Cells[string.Format("P{0}", row)].Value = item.TOTAL_TAX == 5 ? item.TOTAL_CGST_AMOUNT + item.TOTAL_SGST_AMOUNT : ""; ;
                        Sheet.Cells[string.Format("Q{0}", row)].Value = item.TOTAL_TAX == 5 ? item.TOTAL_CGST_AMOUNT : "";
                        Sheet.Cells[string.Format("R{0}", row)].Value = item.TOTAL_TAX == 5 ? item.TOTAL_SGST_AMOUNT : "";
                        Sheet.Cells[string.Format("S{0}", row)].Value = item.UD_AMOUNT_2 + item.TOTAL_CGST_AMOUNT + item.TOTAL_SGST_AMOUNT;
                        Sheet.Cells[string.Format("T{0}", row)].Value = item.FRIEGHT;
                        Sheet.Cells[string.Format("U{0}", row)].Value = item.BILL_AMOUNT;

                        flag = 1;
                        mergered = false;
                        srno++;
                    }
                    else
                    {

                        if (mergered == false)
                        {
                            int previousRow = row - 1;
                            int lastRow = row + rowspan - 2;
                            Sheet.Cells["A" + previousRow + ":A" + lastRow].Merge = true;
                            Sheet.Cells["B" + (row - 1) + ":B" + (row + rowspan - 2)].Merge = true;
                            Sheet.Cells["C" + (row - 1) + ":C" + (row + rowspan - 2)].Merge = true;
                            Sheet.Cells["D" + (row - 1) + ":D" + (row + rowspan - 2)].Merge = true;
                            Sheet.Cells["E" + (row - 1) + ":E" + (row + rowspan - 2)].Merge = true;
                            Sheet.Cells["F" + (row - 1) + ":F" + (row + rowspan - 2)].Merge = true;
                            Sheet.Cells["T" + (row - 1) + ":T" + (row + rowspan - 2)].Merge = true;
                            Sheet.Cells["U" + (row - 1) + ":U" + (row + rowspan - 2)].Merge = true;

                            mergered = true;
                        }
                        Sheet.Cells[string.Format("G{0}", row)].Value = item.hsn_code;
                        Sheet.Cells[string.Format("H{0}", row)].Value = item.UD_AMOUNT_2;
                        Sheet.Cells[string.Format("I{0}", row)].Value = item.TOTAL_QUANTITY;
                        Sheet.Cells[string.Format("J{0}", row)].Value = item.TOTAL_TAX;
                        Sheet.Cells[string.Format("K{0}", row)].Value = item.TOTAL_CGST_AMOUNT;
                        Sheet.Cells[string.Format("L{0}", row)].Value = item.TOTAL_SGST_AMOUNT;
                        Sheet.Cells[string.Format("M{0}", row)].Value = item.TOTAL_TAX == 12 ? item.TOTAL_CGST_AMOUNT + item.TOTAL_SGST_AMOUNT : "";
                        Sheet.Cells[string.Format("N{0}", row)].Value = item.TOTAL_TAX == 12 ? item.TOTAL_CGST_AMOUNT : "";
                        Sheet.Cells[string.Format("O{0}", row)].Value = item.TOTAL_TAX == 12 ? item.TOTAL_SGST_AMOUNT : "";
                        Sheet.Cells[string.Format("P{0}", row)].Value = item.TOTAL_TAX == 5 ? item.TOTAL_CGST_AMOUNT + item.TOTAL_SGST_AMOUNT : ""; ;
                        Sheet.Cells[string.Format("Q{0}", row)].Value = item.TOTAL_TAX == 5 ? item.TOTAL_CGST_AMOUNT : "";
                        Sheet.Cells[string.Format("R{0}", row)].Value = item.TOTAL_TAX == 12 ? item.TOTAL_SGST_AMOUNT : "";
                        Sheet.Cells[string.Format("S{0}", row)].Value = item.UD_AMOUNT_2 + item.TOTAL_CGST_AMOUNT + item.TOTAL_SGST_AMOUNT;

                    }
                }
                else
                {
                    flag = 0;
                    Sheet.Cells[string.Format("A{0}", row)].Value = srno;
                    Sheet.Cells[string.Format("B{0}", row)].Value = item.vendor_name;
                    Sheet.Cells[string.Format("C{0}", row)].Value = item.city_name;
                    Sheet.Cells[string.Format("D{0}", row)].Value = item.GST_NO;
                    Sheet.Cells[string.Format("E{0}", row)].Value = item.BILL_NO;
                    Sheet.Cells[string.Format("F{0}", row)].Value = item.BILL_DATEe;
                    Sheet.Cells[string.Format("G{0}", row)].Value = item.hsn_code;
                    Sheet.Cells[string.Format("H{0}", row)].Value = item.UD_AMOUNT_2;
                    Sheet.Cells[string.Format("I{0}", row)].Value = item.TOTAL_QUANTITY;
                    Sheet.Cells[string.Format("J{0}", row)].Value = item.TOTAL_TAX;
                    Sheet.Cells[string.Format("K{0}", row)].Value = item.TOTAL_CGST_AMOUNT;
                    Sheet.Cells[string.Format("L{0}", row)].Value = item.TOTAL_SGST_AMOUNT;
                    Sheet.Cells[string.Format("M{0}", row)].Value = item.TOTAL_TAX == 12 ? item.TOTAL_CGST_AMOUNT + item.TOTAL_SGST_AMOUNT : "";
                    Sheet.Cells[string.Format("N{0}", row)].Value = item.TOTAL_TAX == 12 ? item.TOTAL_CGST_AMOUNT : "";
                    Sheet.Cells[string.Format("O{0}", row)].Value = item.TOTAL_TAX == 12 ? item.TOTAL_SGST_AMOUNT : "";
                    Sheet.Cells[string.Format("P{0}", row)].Value = item.TOTAL_TAX == 5 ? item.TOTAL_CGST_AMOUNT + item.TOTAL_SGST_AMOUNT : "";
                    Sheet.Cells[string.Format("Q{0}", row)].Value = item.TOTAL_TAX == 5 ? item.TOTAL_CGST_AMOUNT : "";
                    Sheet.Cells[string.Format("R{0}", row)].Value = item.TOTAL_TAX == 5 ? item.TOTAL_SGST_AMOUNT : ""; ;
                    Sheet.Cells[string.Format("S{0}", row)].Value = item.TOTAL_TAX == 5 ? item.TOTAL_CGST_AMOUNT + item.TOTAL_SGST_AMOUNT : ""; ;
                    Sheet.Cells[string.Format("T{0}", row)].Value = item.FRIEGHT;
                    Sheet.Cells[string.Format("U{0}", row)].Value = item.BILL_AMOUNT;

                    srno++;
                    mergered = false;
                }

                row++;
            }
            Sheet.Cells["A:AZ"].AutoFitColumns();
            ViewBag.FileName = DateTime.Now.Ticks.ToString() + ".xlsx";

            string path = Path.Combine(_env.WebRootPath, "Temp");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
;
            System.IO.File.WriteAllBytes(path + "/" + ViewBag.FileName, Ep.GetAsByteArray());
            return PartialView("Partial/_GetSalesList", list);
        }
        public IActionResult ListState()
        {
            var list = _iAdmin.GetStateList();
            return View(list);
        }
        [HttpPost]
        public IActionResult _AddUpdateStateMaster(Tbl_StateViewModel req)
        {
            return PartialView("Partial/_AddUpdateStateMaster", req);
        }
        [HttpPost]
        public IActionResult UpdateStateMaster(Tbl_StateViewModel req)
        {
            req.entry_by = Convert.ToInt32(_session.GetInt32("emp_id"));
            return Json(_iAdmin.UpdateState(req));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteData(tbl_delete req)
        {
            req.entry_by = Convert.ToInt32(_session.GetInt32("emp_id"));
            var res = new Response();
            await Task.Run(() =>
            {
                res = _iAdmin.DeleteData(req);
            });

            return Json(res);
        }
        public IActionResult ListCity()
        {
            var list = _iAdmin.GetCityList();
            return View(list);
        }
        [HttpPost]
        public PartialViewResult _UpdateCity(Tbl_cityViewModel req)
        {
            req.state_list = _iAdmin.GetState();
            return PartialView("Partial/_UpdateCity", req);
        }
        [HttpPost]
        public IActionResult UpdateCity(Tbl_cityViewModel req)
        {
            req.entry_by = Convert.ToInt32(_session.GetInt32("emp_id"));
            var res = _iAdmin.UpdateCity(req);
            return Json(res);
        }
        public IActionResult AddUpdateSalesDetails(int sales_id)
        {
            tbl_sales_viewmodel req = new tbl_sales_viewmodel();
            if (sales_id == 0)
            {
                req.vendor_list = _iAdmin.GetVendor();
                req.state_list = _iAdmin.GetState();
            }
            else
            {
                req = _iAdmin.EditSales(sales_id);
                var data = req.BILL_NO;
                req.vendor_list = _iAdmin.GetVendor();
                req.state_list = _iAdmin.GetState();
            }
            return View(req);
        }
        

        
        public IActionResult GetSalesDetails(tbl_sales_viewmodel req)
        {
            req.vendor_list = _iAdmin.GetVendor();
            return View(req);
        }
        [HttpPost]
        public PartialViewResult _GetSalesDetails(tbl_sales_viewmodel req)
        {
            req.entry_by = Convert.ToInt32(_session.GetInt32("emp_id"));
            req.role_id = Convert.ToInt32(_session.GetInt32("role_id"));
            var list = _iAdmin.GetSalesDetails(req);
            return PartialView("Partial/_GetSalesDetails", list);
        }
        //[HttpPost]
        //public PartialViewResult _GetDetailsByBillNo(tbl_sales_viewmodel req)
        //{
        //    var list = _iAdmin._GetDetailsByBillNo(req);
        //    return PartialView("Partial/_GetDetailsByBillNo", list);
        //}

        //[HttpPost]
        //public IActionResult _UpdateBillDetails(tbl_sales_viewmodel req)
        //{
        //    return PartialView("Partial/_UpdateBillDetails", req);
        //}

        [HttpPost]
        public JsonResult AddUpdateBillDetails(tbl_sales_viewmodel req)
        {
            req.entry_by = Convert.ToInt32(_session.GetInt32("emp_id"));
            var res = _iAdmin.UpdateBillDetail(req);
            return Json(res);
        }
    }
}
