using Microsoft.Identity.Client;
using System.ComponentModel;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Invoice
    {
        public int Sno { get; set; }
        public DateTime Date { get; set; }
        public string GemInvoiceNo { get; set; }
        [StringLength(100)]
        public string CompanyName { get; set; }
        public string Items { get; set; } 
        public int Qty { get; set; }
        public decimal Rate { get; set; }

        public decimal Amount { get; set; }
        public string AmountInwords { get; set; }
        public decimal CGST { get; set; } 

        public decimal SGST { get; set; }

        public decimal CGSTAmount { get; set; }
       
            
        public decimal SGSTAmount { get; set; }

        public decimal TotalAmount { get; set; }
        public string TotalAmountInWords { get; set; }
    }
}