using BusinessLogic.Repository;
using System.Drawing;
using System.Data.SqlClient;
using ManasMarketting.Models;
using System.Data;

namespace BusinessLogic.Concrete
{
    public class AdminConcrete : IAdmin
    {
        readonly IConnectionString _IConn;
        readonly ICommonFunction _ICommom;
        readonly IDAL _IDAL;
        readonly IWebHostEnvironment _env;
        readonly IHttpContextAccessor _accesssor;
        readonly ISession _session;
        public AdminConcrete(IHttpContextAccessor accesssor, IWebHostEnvironment env)
        {
            _IConn = new ConnectionStr(accesssor, env);
            _ICommom = new CommonFunctionConcrete();
            _IDAL = new DAL(_IConn.GetConnectionString());
            _session = accesssor.HttpContext.Session;
        }

        public List<tbl_vendor_viewmodel> GetVendorList()
        {
            SqlParameter[] param =
            {

            };
            List<tbl_vendor_viewmodel> res = new List<tbl_vendor_viewmodel>();
            var dt = _IDAL.GetByProcedureAdapter("proc_getlist_vendor", param);
            if (dt != null && dt.Rows.Count > 0)
            {
                res = _ICommom.ConvertDataTable<tbl_vendor_viewmodel>(dt);
            }
            return res;
        }

        public List<tbl_supplier_viewmodel> GetSupplierList()
        {
            SqlParameter[] param =
            {

            };
            List<tbl_supplier_viewmodel> res = new List<tbl_supplier_viewmodel>();
            var dt = _IDAL.GetByProcedureAdapter("proc_getlist_supplier", param);
            if (dt != null && dt.Rows.Count > 0)
            {
                res = _ICommom.ConvertDataTable<tbl_supplier_viewmodel>(dt);
            }
            return res;
        }

        public List<Tbl_cityViewModel> GetCity(int state_id)
        {
            SqlParameter[] param = {
                new SqlParameter("@state_id", state_id),

            };

            var dt = _IDAL.GetByProcedureAdapter("proc_get_tbl_city", param);
            List<Tbl_cityViewModel> res = new List<Tbl_cityViewModel>();
            if (dt != null && dt.Rows.Count > 0)
            {
                res = _ICommom.ConvertDataTable<Tbl_cityViewModel>(dt);
            }
            return res;
        }

        public string GetGST(int supplier_id)
        {
            SqlParameter[] param = {
                new SqlParameter("@supplier_id", supplier_id),

            };
            string gst = null;
            var res = _IDAL.GetByProcedureAdapter("proc_get_tbl_gstbyid", param);
            if (res != null && res.Rows.Count > 0)
            {
                // Iterate through the rows of the DataTable
                foreach (DataRow row in res.Rows)
                {
                    // Check if the "gst_no" column exists and is not null or empty for the current row
                    if (row["gst_no"] != DBNull.Value && !string.IsNullOrEmpty(row["gst_no"].ToString()))
                    {
                        // Extract the value of the "gst_no" column and store it in the string variable
                        gst = row["gst_no"].ToString();
                        // If you only need the first non-null GST number, you can break the loop here
                        break;
                    }
                }
            }

            return gst;
        }

        public List<Tbl_StateViewModel> GetState()
        {
            SqlParameter[] param = {

            };

            var dt = _IDAL.GetByProcedureAdapter("proc_get_tbl_State", param);
            List<Tbl_StateViewModel> res = new List<Tbl_StateViewModel>();
            if (dt != null && dt.Rows.Count > 0)
            {
                res = _ICommom.ConvertDataTable<Tbl_StateViewModel>(dt);
            }
            return res;
        }

        public Response UpdateVendor(tbl_vendor_viewmodel req)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@vendor_id",req.vendor_id),
                new SqlParameter("@vendor_name",req.vendor_name),
                new SqlParameter("@gst_no",req.gst_no),
                new SqlParameter("@entry_by",req.entry_by),
                new SqlParameter("@status",req.status),
                new SqlParameter("@city_id",req.city_id),
                new SqlParameter("@state_id",req.state_id)
            };

            var dt = _IDAL.GetByProcedureAdapter("proc_AddUpdate_tbl_Vendor", param);

            Response res = new Response();
            if (dt != null && dt.Rows.Count > 0)
            {
                res = _ICommom.ConvertDataTable<Response>(dt)[0];
            }
            return res;
        }

        public Response UpdateSupplier(tbl_supplier_viewmodel req)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@supplier_id",req.supplier_id),
                new SqlParameter("@supplier_name",req.supplier_name),
                new SqlParameter("@gst_no",req.gst_no),
                new SqlParameter("@entry_by",req.entry_by),
                new SqlParameter("@status",req.status),
                new SqlParameter("@city_id",req.city_id),
                new SqlParameter("@state_id",req.state_id)
            };

            var dt = _IDAL.GetByProcedureAdapter("proc_AddUpdate_tbl_Supplier", param);

            Response res = new Response();
            if (dt != null && dt.Rows.Count > 0)
            {
                res = _ICommom.ConvertDataTable<Response>(dt)[0];
            }
            return res;
        }

        public Response UploadSalesExcel(DataTable tbl)
        {
            SqlParameter[] param = {
                new SqlParameter("@tbl", tbl),
                new SqlParameter("@entry_by", Convert.ToInt32(_session.GetInt32("emp_id"))),
            };
            var dt = _IDAL.GetByProcedureAdapter("proc_sales_excel", param);
            Response res = new Response();
            if (dt != null && dt.Rows.Count > 0)
            {
                res = _ICommom.ConvertDataTable<Response>(dt)[0];
            }
            return res;
        }

        public Response UploadExcelFile(DataTable tbl)
        {
            SqlParameter[] param = {
                new SqlParameter("@tbl", tbl),
                new SqlParameter("@entry_by", Convert.ToInt32(_session.GetInt32("emp_id"))),
            };
            var dt = _IDAL.GetByProcedureAdapter("proc_upload_purchase_excel", param);
            Response res = new Response();
            if (dt != null && dt.Rows.Count > 0)
            {
                res = _ICommom.ConvertDataTable<Response>(dt)[0];
            }
            return res;
        }
        public List<tbl_supplier_viewmodel> GetSupplier() /*tbl_supplier_viewmodel*/
        {
            SqlParameter[] param = {

            };
            var dt = _IDAL.GetByProcedureAdapter("proc_get_supplier_name_list", param);
            List<tbl_supplier_viewmodel> res = new List<tbl_supplier_viewmodel>();
            if (dt != null && dt.Rows.Count > 0)
            {
                res = _ICommom.ConvertDataTable<tbl_supplier_viewmodel>(dt);
            }
            return res;

        }
        public List<tbl_vendor_viewmodel> GetVendor()
        {
            SqlParameter[] param = {

            };

            var dt = _IDAL.GetByProcedureAdapter("proc_get_vendor_name_list", param);
            List<tbl_vendor_viewmodel> res = new List<tbl_vendor_viewmodel>();
            if (dt != null && dt.Rows.Count > 0)
            {
                res = _ICommom.ConvertDataTable<tbl_vendor_viewmodel>(dt);
            }
            return res;
        }


        public Response UpdateSales(tbl_sales_viewmodel req)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@vendor_id",req.vendor_id),
                new SqlParameter("@hsn_id",req.hsn_id),
                new SqlParameter("@hsn_code",req.hsn_code),
                new SqlParameter("@entry_by",req.entry_by),
                new SqlParameter("@city_id",req.city_id),
                new SqlParameter("@state_id",req.state_id),
                new SqlParameter("@UD_AMOUNT_2",req.UD_AMOUNT_2),
                new SqlParameter("@GST_NO",req.GST_NO),
                new SqlParameter("@BILL_NO",req.BILL_NO),
                new SqlParameter("@TOTAL_QUANTITY",req.TOTAL_QUANTITY),
                new SqlParameter("@TOTAL_TAX",req.TOTAL_TAX),
                new SqlParameter("@TOTAL_CGST_AMOUNT",req.TOTAL_CGST_AMOUNT),
                new SqlParameter("@TOTAL_SGST_AMOUNT",req.TOTAL_SGST_AMOUNT),
                new SqlParameter("@FRIEGHT",req.FRIEGHT),
                new SqlParameter("@GST_SALE_12",req.GST_SALE_12),
                new SqlParameter("@SGST_OUTPUT_6",req.SGST_OUTPUT_6),
                new SqlParameter("@CGST_OUTPUT_6",req.CGST_OUTPUT_6),
                new SqlParameter("@GST_SALE_5",req.GST_SALE_5),
                new SqlParameter("@CGST_OUTPUT_2",req.CGST_OUTPUT_2),
                new SqlParameter("@NET_AMOUNT",req.NET_AMOUNT),
                new SqlParameter("@BILL_AMOUNT",req.BILL_AMOUNT),

            };
            var dt = _IDAL.GetByProcedureAdapter("proc_update_tbl_sales", param);
            Response res = new Response();
            if (dt != null && dt.Rows.Count > 0)
            {
                res = _ICommom.ConvertDataTable<Response>(dt)[0];
            }
            return res;
        }

        public Response UpdatePurchase(tbl_purchase_viewmodel req)
        {
            SqlParameter[] param =
              {
                new SqlParameter("@invoice_number",req.invoice_number),
                new SqlParameter("@invoice_date",req.invoice_date),
                new SqlParameter("@state_id",req.state_id),
                new SqlParameter("@reverse_charge",req.reverse_charge),
                new SqlParameter("@invoice_type",req.invoice_type),
                new SqlParameter("@city_id",req.city_id) ,
                new SqlParameter("@supplier_id",req.supplier_id),
                 new SqlParameter("@gst_no",req.gst_no)
     };
            var dt = _IDAL.GetByProcedureAdapter("proc_update_tbl_purchase", param);
            Response res = new Response();
            if (dt != null && dt.Rows.Count > 0)
            {
                res = _ICommom.ConvertDataTable<Response>(dt)[0];
            }
            return res;
        }



        public List<tbl_purchase_viewmodel> GetPurchaseListById(tbl_purchase_viewmodel req)
        {
            SqlParameter[] param = {
                new SqlParameter("@date_from",req.datefrom),
                new SqlParameter("@date_to",req.dateto),
                new SqlParameter("@supplier_id",req.supplier_id)

            };


            var dt = _IDAL.GetByProcedureAdapter("proc_get_list_tbl_purchaseById", param);
            List<tbl_purchase_viewmodel> res = new List<tbl_purchase_viewmodel>();
            if (dt != null && dt.Rows.Count > 0)
            {
                res = _ICommom.ConvertDataTable<tbl_purchase_viewmodel>(dt);
            }
            return res;
        }



        public List<tbl_sales_viewmodel> GetSalesListById(tbl_sales_viewmodel req)
        {
            SqlParameter[] param = {
                new SqlParameter("@date_from",req.datefrom),
                new SqlParameter("@date_to",req.dateto),
                new SqlParameter("@vendor_id",req.vendor_id),

            };

            var dt = _IDAL.GetByProcedureAdapter("proc_get_list_tbl_salesById", param);
            List<tbl_sales_viewmodel> res = new List<tbl_sales_viewmodel>();
            if (dt != null && dt.Rows.Count > 0)
            {
                res = _ICommom.ConvertDataTable<tbl_sales_viewmodel>(dt);
            }
            return res;
        }

        public tbl_sales_viewmodel EditSales(int? sales_id)
        {
            SqlParameter[] param = {
                new SqlParameter("@sales_id",sales_id),
            };
            tbl_sales_viewmodel res = new tbl_sales_viewmodel();
            var dt = _IDAL.GetByProcedureAdapter("proc_update_tbl_sales_byId", param);
            if (dt != null && dt.Rows.Count > 0)
            {
                res = _ICommom.ConvertDataTable<tbl_sales_viewmodel>(dt)[0];
            }
            return res;
        }
        public List<Tbl_StateViewModel> GetStateList()
        {
            SqlParameter[] param = {


            };

            var dt = _IDAL.GetByProcedureAdapter("proc_list_tbl_state", param);
            List<Tbl_StateViewModel> res = new List<Tbl_StateViewModel>();
            if (dt != null && dt.Rows.Count > 0)
            {
                res = _ICommom.ConvertDataTable<Tbl_StateViewModel>(dt);
            }
            return res;
        }
        public Response UpdateState(Tbl_StateViewModel req)
        {
            SqlParameter[] param = {
                new SqlParameter("@state_id",req.state_id),
                new SqlParameter("@state_name",req.state_name),
                new SqlParameter("@state_code",req.state_code),
                new SqlParameter("@status",req.status),
                //new SqlParameter("@entry_by", Convert.ToInt32(_session.GetInt32("emp_id"))),
                new SqlParameter("@entry_by", req.entry_by),
                new SqlParameter("@modify_by", req.entry_by)
            };
            var dt = _IDAL.GetByProcedureAdapter("proc_update_tbl_state", param);
            Response res = new Response();
            if (dt != null && dt.Rows.Count > 0)
            {
                res = _ICommom.ConvertDataTable<Response>(dt)[0];
            }
            return res;
        }
        public Response DeleteData(tbl_delete req)
        {
            SqlParameter[] param = {
                new SqlParameter("@id", req.id),
                new SqlParameter("@entry_by", req.entry_by),
                new SqlParameter("@table_name", req.table_name),
            };

            var dt = _IDAL.GetByProcedureAdapter("proc_delete_data", param);
            Response res = new Response();
            if (dt != null && dt.Rows.Count > 0)
            {
                res = _ICommom.ConvertDataTable<Response>(dt)[0];
            }
            return res;
        }
        public List<Tbl_cityViewModel> GetCityList()
        {
            SqlParameter[] param = {

            };

            var dt = _IDAL.GetByProcedureAdapter("proc_list_tbl_city", param);
            List<Tbl_cityViewModel> res = new List<Tbl_cityViewModel>();
            if (dt != null && dt.Rows.Count > 0)
            {
                res = _ICommom.ConvertDataTable<Tbl_cityViewModel>(dt);
            }
            return res;
        }
        public Response UpdateSalesDetails(tbl_sales_viewmodel req)
        {
            SqlParameter[] param ={

                new SqlParameter("@sales_id",req.sales_id),
                new SqlParameter("@vendor_id",req.vendor_id),
                new SqlParameter("@gst_no",req.GST_NO),
                new SqlParameter("@state_id",req.state_id),
                new SqlParameter("@city_id",req.city_id),
                new SqlParameter("@bill_no",req.BILL_NO),
                new SqlParameter("@bill_date",req.BILL_DATE),
                new SqlParameter("@frieght",req.FRIEGHT),
                new SqlParameter("@bill_amount",req.BILL_AMOUNT),
                new SqlParameter("@entry_by",req.entry_by),
            };
            var dt = _IDAL.GetByProcedureAdapter("proc_update_tbl_sales_details", param);
            Response res = new Response();
            if (dt != null && dt.Rows.Count > 0)
            {
                res = _ICommom.ConvertDataTable<Response>(dt)[0];
            }
            return res;
        }

        public Response UpdatePurchaseDetails(tbl_purchase_viewmodel req)
        {
            SqlParameter[] param ={

                new SqlParameter("@gross_amount",req.gross_amount),
                 new SqlParameter("total_tax_per",req.total_tax_per),
                  new SqlParameter("@igst",req.igst),
                        new SqlParameter("@description",req.description),
                        new SqlParameter("@hsn_code",req.hsn_code),
                        new SqlParameter("@total_quantity",req.total_quantity),
                          new SqlParameter("@tcs",req.TCS),
                            new SqlParameter("@purchase_id",req.purchase_id)

            };
            var dt = _IDAL.GetByProcedureAdapter("proc_update_tbl_purchase_details", param);
            Response res = new Response();
            if (dt != null && dt.Rows.Count > 0)
            {
                res = _ICommom.ConvertDataTable<Response>(dt)[0];
            }
            return res;
        }
        public List<tbl_sales_viewmodel> GetSalesDetails(tbl_sales_viewmodel req)
        {
            SqlParameter[] param = {
                new SqlParameter("@vendor_id",req.vendor_id),
                new SqlParameter("@role_id",req.role_id),
                new SqlParameter("@emp_id",req.entry_by),
                new SqlParameter("@datefrom",req.datefrom),
                new SqlParameter("@dateto",req.dateto),

            };

            var dt = _IDAL.GetByProcedureAdapter("proc_get_list_sales_details", param);
            List<tbl_sales_viewmodel> res = new List<tbl_sales_viewmodel>();
            if (dt != null && dt.Rows.Count > 0)
            {
                res = _ICommom.ConvertDataTable<tbl_sales_viewmodel>(dt);
            }
            return res;
        }

        public List<tbl_sales_viewmodel> _GetDetailsByBillNo(tbl_sales_viewmodel req)
        {
            SqlParameter[] param = {
                new SqlParameter("@bill_no",req.BILL_NO)

            };

            var dt = _IDAL.GetByProcedureAdapter("proc_get_details_by_bill_no", param);
            List<tbl_sales_viewmodel> res = new List<tbl_sales_viewmodel>();
            if (dt != null && dt.Rows.Count > 0)
            {
                res = _ICommom.ConvertDataTable<tbl_sales_viewmodel>(dt);
            }
            return res;
        }
        public Response UpdateBillDetail(tbl_sales_viewmodel req)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@sales_detail_id",req.sales_detail_id),
                new SqlParameter("@sales_id",req.sales_id),
                new SqlParameter("@hsn_code",req.hsn_code),
                new SqlParameter("@UD_AMOUNT_2",req.UD_AMOUNT_2),
                new SqlParameter("@total_quantity",req.TOTAL_QUANTITY),
                new SqlParameter("@total_tax",req.TOTAL_TAX),
                new SqlParameter("@total_cgst_amount",req.TOTAL_CGST_AMOUNT),
                new SqlParameter("@total_sgst_amount",req.TOTAL_SGST_AMOUNT),
                new SqlParameter("@entry_by",req.entry_by),

            };
            var dt = _IDAL.GetByProcedureAdapter("proc_update_tbl_sales_detail_bill_no", param);
            Response res = new Response();
            if (dt != null && dt.Rows.Count > 0)
            {
                res = _ICommom.ConvertDataTable<Response>(dt)[0];
            }
            return res;
        }
        public Response UpdateCity(Tbl_cityViewModel req)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@city_id",req.city_id),
                new SqlParameter("@city_name",req.city_name),
                new SqlParameter("@state_id",req.state_id),
                new SqlParameter("@entry_by",req.entry_by),
                new SqlParameter("@status",req.status),
                new SqlParameter("@modify_by",req.entry_by),
            };
            var dt = _IDAL.GetByProcedureAdapter("proc_update_tbl_city", param);
            Response res = new Response();
            if (dt != null && dt.Rows.Count > 0)
            {
                res = _ICommom.ConvertDataTable<Response>(dt)[0];
            }
            return res;
        }




    }


}
