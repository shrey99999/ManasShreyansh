namespace ManasMarketting.Models
{
    public class tbl_supplier_viewmodel
    {
        public int supplier_id { get; set; }
        public string? supplier_name { get; set; }

        public int hsn_id { get; set; }
        public int address_id { get; set; }
        public int purchase_id { get; set; }
        public int city_id { get; set; }
        public int state_id { get; set; }
        public int entry_by { get; set; }
        public string? state_name { get; set; }
        public string? city_name { get; set; }
        public bool status { get; set; }

        public string gst_no { get; set; }
        public string entry_date { get; set; }
        public string hsn_code { get; set; }
        public decimal purchase_amount { get; set; }

        //public string file_excel { get; set; }
        //public IFormFile document { get; set; } 

        public List<tbl_supplier_viewmodel> supplier_list { get; set; }
        public List<Tbl_cityViewModel> city_list { get; set; }
        public List<Tbl_StateViewModel> state_list { get; set; }
    }
}
