using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ManasMarketting.Models
{
    public class LoginViewMoodel
    {
        public int login_id { get; set; }
        public string u_id { get; set; }
        public int role_id { get; set; }
        public string emp_name { get; set; }
        public int emp_id { get; set; }
        public string email { get; set; }
        public string mobile { get; set; }
        //public int user_id { get; set; }
        public string password { get; set; }
        public string NewPassword { get; set; }
        public string otp { get; set; }
        public bool status { get; set; }
        public int user_type { get; set; }
        //public string permission_setting { get; set; }
        public string role_name { get; set; }
    }
    public class Response
    {
        public int status { get; set; }
        public string msg { get; set; }
    }

   
    public class tbl_vendor_viewmodel
    {
        public int vendor_id { get; set; }
        public int hsn_id { get; set; }
        public int address_id { get; set; }
        public int sales_id { get; set; }
        public int city_id { get; set; }
        public int state_id { get; set; }
        public int entry_by { get; set; }
        public string state_name { get; set; }
        public string city_name { get; set; }
        public string vendor_name { get; set; }
        public string gst_no { get; set; }
        public string entry_date { get; set; }
        public string hsn_code { get; set; }
        public decimal sales_amount { get; set; }
        public bool status { get; set; }
        //public string file_excel { get; set; }
        //public IFormFile document { get; set; }
        public List<Tbl_cityViewModel> city_list { get; set; }
        public List<Tbl_StateViewModel> state_list { get; set; }


    }
    public class Tbl_cityViewModel
    {
        public int city_id { get; set; }
        public string city_name { get; set; }
        public int state_id { get; set; }
        public string state_name { get; set; }
        public bool status { get; set; }
        public List<Tbl_StateViewModel> state_list { get; set; }
        public int entry_by { get; set; }

    }
    public class Tbl_StateViewModel
    {
        public int state_id { get; set; }
        public string? state_name { get; set; }
        public string? state_code { get; set; }
        public bool status { get; set; }
        public int entry_by { get; set; }
        public int modify_by { get; set; }

    }
    public class tbl_sales_viewmodel
    {
        public int sales_id { get; set; }
        public int hsn_id { get; set; }
        public int vendor_id { get; set; }
        public string hsn_code { get; set; }
        public string vendor_name { get; set; }
        public string city_name { get; set; }
        public int entry_by { get; set; }
        public int city_id { get; set; }
        public int state_id { get; set; }
        public bool status { get; set; }
        public string entry_date { get; set; }
        public string GST_NO { get; set; }
        public string BILL_NO { get; set; }
        public string BILL_DATE { get; set; }
        public string state_name { get; set; }
        public decimal UD_AMOUNT_2 { get; set; }
        public string BILL_DATEe { get; set; }
        public int TOTAL_QUANTITY { get; set; }
        public decimal TOTAL_TAX { get; set; }
        public decimal TOTAL_CGST_AMOUNT { get; set; }
        public decimal TOTAL_SGST_AMOUNT { get; set; }
        public decimal FRIEGHT { get; set; }
        public decimal GST_SALE_12 { get; set; }
        public decimal SGST_OUTPUT_6 { get; set; }
        public decimal CGST_OUTPUT_6 { get; set; }
        public decimal GST_SALE_5 { get; set; }
        public decimal CGST_OUTPUT_2 { get; set; }
        public decimal NET_AMOUNT { get; set; }
        public decimal BILL_AMOUNT { get; set; }
        public List<Tbl_cityViewModel> city_list { get; set; }
        public List<Tbl_StateViewModel> state_list { get; set; }
        public List<tbl_vendor_viewmodel> vendor_list { get; set; }
        public int role_id { get; set; }
        public string datefrom { get; set; }
        public string dateto { get; set; }
        public int sales_detail_id { get; set; }
        //public List<tbl_sales_viewmodel>? sales_list { get; set; }
    }
    public class tbl_Otp
    {
        public int otp_id { get; set; }
        public int emp_id { get; set; }
        public string email { get; set; }
        public string Otp { get; set; }
        public string password { get; set; }
    }

    public class Dashboard
    {
        public int vendor_count { get; set; }
        public decimal total_sales { get; set; }
    }
    public class tbl_delete
    {
        public string table_name { get; set; }
        public string id { get; set; }
        public int? entry_by { get; set; }

    }
}
