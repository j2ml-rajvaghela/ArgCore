using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arg.DataModels
{
    [Dapper.Contrib.Extensions.Table("[ArgInvoices.BalanceDues]")]
    public class ArgInvoices_BalanceDues
    {
        [Key]
        public int InvoiceBDId { get; set; }
        public int CompanyId { get; set; }
        public string Region { get; set; }

        [Column("Invoice#")]
        public string InvoiceNo { get; set; }

        public DateTime InvoiceDate { get; set; }
        public string CustomerId { get; set; }
        public string CustomerLocationCode { get; set; }
        public string BookingId { get; set; }

        [Column("Bol#")]
        public string BOLNo { get; set; }

        public DateTime BolExecutionDate { get; set; }
        public decimal AmountDueUSD { get; set; }
        public decimal AmountDueLocalCurrency { get; set; }
        public string LocalCurrency { get; set; }
        public string BalanceDueInvoice { get; set; }

        [Computed]
        public string Company { get; set; }

        [Computed]
        public DateTime DueDate { get; set; }

        [Computed]
        public string InvoiceStatus { get; set; }

        [Computed]
        public string Customer { get; set; }

        [Computed]
        public string OriginLocationCode { get; set; }

        [Computed]
        public string DestinationLocationCode { get; set; }

        [Computed]
        public string CustomerName { get; set; }

        [Computed]
        public string Bol { get; set; }

        [Computed]
        public string InvoiceType { get; set; }

        [Computed]
        public string Currency { get; set; }

        [Computed]
        public decimal RevenueRecovered { get; set; }

        [Computed]
        public string CurrencyConvertedTo { get; set; }

        [Computed]
        public int ConversionRate { get; set; }

        //[Ignore]
        public decimal RevenueRecoveredUSD
        {
            get
            {
                if (Currency == "USD")
                {
                    return RevenueRecovered;
                }
                return RevenueRecovered * ConversionRate;
            }
        }
    }
}
