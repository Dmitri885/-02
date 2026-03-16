using System;

namespace Kursych.Forms.Products
{
    public class ProductData
    {
        public int ProductID { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int CategoryID { get; set; }
        public string Description { get; set; }
        public int SupplierID { get; set; }
        public int StockQuantity { get; set; }
        public string ImagePath { get; set; }
    }
}