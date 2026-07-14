using ExpenseManager.Web.Models.Enum;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace ExpenseManager.Web.Models
{
    public class ExpenseViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Expense name must have more than 3 characters.")]
        [DisplayName("Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Currency is required.")]
        [DisplayName("Currency")]
        public CurrencyVM Currency { get; set; }

        [DisplayName("Value")]
        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        public decimal Value
        {
            get
            {
                string valueString = ValueString.Replace(',', '.');
                decimal.TryParse(valueString,
                    NumberStyles.Currency, 
                    CultureInfo.InvariantCulture, out var value);

                return value;
            }
        }

        [Required(ErrorMessage = "Value is required.")]
        public string ValueString { get; set; }

        [Required(ErrorMessage = "Expense category is required.")]
        [DisplayName("Category")]
        public ExpenseTypeVM Type { get; set; }

        [Required(ErrorMessage = "Source of expense is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Expense source must have more than 2 characters.")]
        [DisplayName("Source of expense")]
        public string Source { get; set; }

        [Required(ErrorMessage = "Date of expense is required.")]
        [DisplayName("Date of expense")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ExpenseDate { get; set; }
    }
}