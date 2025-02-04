namespace AssesmentV4.Models
{
    public class ProductViewModel
    {
        public List<Product> Products { get; set; }
        public List<string> VisibleColumns { get; set; }
        public string SortColumn { get; set; }
        public string SortDirection { get; set; }
        public ProductSearchCriteria SearchCriteria { get; set; }
        public string Source { get; set; }
    }
}
