using System;
using System.ComponentModel.DataAnnotations;

namespace GroupBuyCRM
{
    ///Define customers info entity
    public class CustomersInfo
    {
        [Key]
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Address { get; set; }
        public string Contact1 { get; set; }
        public string Contact2 { get; set; }
        public string Remarks { get; set; }
    }
}
