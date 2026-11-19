using System.ComponentModel.DataAnnotations;

namespace ExpenseManager.Core.Enum
{
    public enum Currency
    {
        [Display(Name = "American Dollar")]
        USD,

        [Display(Name = "Euro")]
        EUR,

        [Display(Name = "Brazilian Real")]
        BRL,

        [Display(Name = "British Pound")]
        GBP,

        [Display(Name = "Canadian Dollar")]
        CAD,

        [Display(Name = "Japanese Yen")]
        JPY
    }
}
