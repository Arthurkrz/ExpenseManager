using System.ComponentModel.DataAnnotations;

namespace ExpenseManager.Core.Enum
{
    public enum ExpenseType
    {
        [Display(Name = "Food")]
        Food,

        [Display(Name = "Transport")]
        Transport,

        [Display(Name = "House")]
        House,

        [Display(Name = "Fun")]
        Fun,

        [Display(Name = "Services")]
        Services,

        [Display(Name = "Electronics")]
        Electronics,

        [Display(Name = "Debts")]
        Debts,

        [Display(Name = "Miscellaneous")]
        Miscellaneous
    }
}
