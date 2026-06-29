using System.ComponentModel.DataAnnotations;

namespace ExpenseManager.Web.Models.Enum
{
    public enum CurrencyVM
    {
        [Display(Name = "Euro")]
        Euro,

        [Display(Name = "Real")]
        Real
    }
}
