using System.ComponentModel.DataAnnotations;

namespace ExpenseManager.Core.Enum
{
    public enum Currency
    {
        [Display(Name = "Euro")]
        Euro,

        [Display(Name = "Real")]
        Real
    }
}
