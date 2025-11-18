using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BookStoreApi.Enums
{
    public enum InvoiceStatus
    {
        [Display(Name = "صادر شده")]
        [Description("صادر شده")]
        Confirmed = 1,

        [Display(Name = "پیش فاکتور")]
        [Description("پیش فاکتور")]
        Pending = 2,

        [Display(Name = "ابطال شده")]
        [Description("ابطال شده")]
        Rejected = 3,
    }
}
