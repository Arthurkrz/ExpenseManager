using System.ComponentModel.DataAnnotations;

namespace ExpenseManager.Web.Models.Enum
{
    public enum CurrencyVM
    {
        [Display(Name = "American Dollars ($)")]
        USD,

        [Display(Name = "Euros (€)")]
        EUR,

        [Display(Name = "Brazilian Reais (R$)")]
        BRL,

        [Display(Name = "British Pounds (£)")]
        GBP,

        [Display(Name = "Canadian Dollars ($)")]
        CAD,

        [Display(Name = "Japanese Yen (¥)")]
        JPY
    }
}
