
using ManasMarketting.Models;
using System.Data;

namespace BusinessLogic.Repository

{
    public interface IAdmin
    {
        //--------------Add & Update------------------------------------------
        Response UpdateVendor(tbl_vendor_viewmodel req);
        Response UpdateSupplier(tbl_supplier_viewmodel req);
        Response UploadExcelFile(DataTable tbl);
        Response UploadSalesExcel(DataTable tbl);
        Response UpdateSales(tbl_sales_viewmodel req);
        Response UpdatePurchase(tbl_purchase_viewmodel req);

        Response UpdatePurchaseDetails(tbl_purchase_viewmodel req);
        tbl_sales_viewmodel EditSales(int? sales_id);
        Response UpdateState(Tbl_StateViewModel req);
        Response DeleteData(tbl_delete req);
        Response UpdateSalesDetails(tbl_sales_viewmodel req);
        Response UpdateBillDetail(tbl_sales_viewmodel req);
        Response UpdateCity(Tbl_cityViewModel req);
        //--------------List------------------------------------------
        List<tbl_vendor_viewmodel> GetVendorList();

       
        List<tbl_supplier_viewmodel> GetSupplierList();
        
        List<Tbl_StateViewModel> GetState();
        List<Tbl_cityViewModel> GetCity(int state_id);
        string GetGST(int supplier_id);
        List<tbl_vendor_viewmodel> GetVendor();

        List<tbl_supplier_viewmodel> GetSupplier();
        List<tbl_sales_viewmodel> GetSalesListById(tbl_sales_viewmodel req);

        List<tbl_purchase_viewmodel> GetPurchaseListById(tbl_purchase_viewmodel req);
       

        List<Tbl_StateViewModel> GetStateList();
        List<Tbl_cityViewModel> GetCityList();
        List<tbl_sales_viewmodel> GetSalesDetails(tbl_sales_viewmodel req);
        List<tbl_sales_viewmodel> _GetDetailsByBillNo(tbl_sales_viewmodel req);
       
    }
}
