//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Domain
{
    using System;
    using System.Collections.Generic;
    
    public partial class Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            this.Business_Product = new HashSet<Business_Product>();
            this.BuyerUser_Product = new HashSet<BuyerUser_Product>();
            this.ProductComment = new HashSet<ProductComment>();
            this.Tasks = new HashSet<Tasks>();
            this.User_Product = new HashSet<User_Product>();
        }
    
        public int ID { get; set; }
        public Nullable<int> Nation { get; set; }
        public Nullable<int> Url_Asin { get; set; }
        public string Url_Asin_Value { get; set; }
        public string ProductDescribe { get; set; }
        public string Label { get; set; }
        public string ProductImg { get; set; }
        public string Title { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<decimal> PriceBefore { get; set; }
        public Nullable<int> Discount { get; set; }
        public Nullable<int> SalesVolume { get; set; }
        public Nullable<int> ProductNumber { get; set; }
        public Nullable<decimal> Commission { get; set; }
        public Nullable<int> OrderType { get; set; }
        public Nullable<int> cmtType { get; set; }
        public Nullable<int> cmtDay { get; set; }
        public Nullable<int> Expired { get; set; }
        public Nullable<System.DateTime> EndTime { get; set; }
        public Nullable<System.DateTime> AddTime { get; set; }
        public string Remark { get; set; }
        public Nullable<int> OperatingMoneyID { get; set; }
        public string Coupon { get; set; }
        public Nullable<decimal> TotalMoney { get; set; }
        public Nullable<int> Status { get; set; }
        public decimal GoodComment { get; set; }
        public Nullable<int> Collections { get; set; }
        public Nullable<int> CmtNum { get; set; }
        public Nullable<int> ProductClassID { get; set; }
        public Nullable<int> Shape { get; set; }
        public Nullable<int> BusinessID { get; set; }
        public Nullable<decimal> TotalCost { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Business_Product> Business_Product { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BuyerUser_Product> BuyerUser_Product { get; set; }
        public virtual ClassiFication ClassiFication { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductComment> ProductComment { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tasks> Tasks { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<User_Product> User_Product { get; set; }
    }
}
