using System.ComponentModel.DataAnnotations;

namespace BookStoreApi.Enums
{
    public enum PaymentStatus
    {
        [Display(Name = "تراکنش موفق")]
        Completed = 1,

        [Display(Name = "تراکنش ناموفق")]
        Failed = 2,

        [Display(Name = "ترامنش در حال بررسی")]
        Initiated = 3,
    }
}
