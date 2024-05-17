namespace ManasMarketting.Models
{
    public class tbl_purchase_viewmodel
    {
        public int purchase_id { get; set; }
        public string? invoice_number { get; set; }
        public DateTime invoice_date { get; set; }
        public decimal invoice_value { get; set; }
        
             public int state_id { get; set; }
        public int city_id { get; set; }
        public string? invoice_type { get; set; }
        public string? hsn_code { get; set; }
        public int total_quantity { get; set; }
        public decimal gross_amount { get; set; }
        public decimal igst_12_amount { get; set; }
        public decimal igst_5_amount { get; set; }
        public decimal total_tax_per { get; set; }
        public int TCS { get; set; }
        public decimal net_amount { get; set; }
        public string? gst_no { get; set; }
        public string? gst_state_name { get; set; }
        public int gst_state_code { get; set; }

        public string? datefrom { get; set; }
        public string? dateto { get; set; }
        public int supplier_id { get; set; }
        public string? purchase_account_name { get; set; }
        public string? reverse_charge { get; set; }
        public string? inter_or_intra { get; set; }
        public decimal igst { get; set; }
        public string? description { get; set; }
        public string? Party_Name { get; set; }

        public List<Tbl_cityViewModel> city_list { get; set; }
        public List<Tbl_StateViewModel> state_list { get; set; }
        public List<tbl_supplier_viewmodel> supplier_list { get; set; }

    }
}
