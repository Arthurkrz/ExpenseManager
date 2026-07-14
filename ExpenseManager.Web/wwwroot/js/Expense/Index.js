$(document).ready(function () {
    if (typeof jQuery === "undefined") {
        console.error("jQuery is not loaded!");
        return;
    }

    const totalInReais = parseFloat
        ('@ViewBag.TotalInReais.ToString("F2", System.Globalization.CultureInfo.InvariantCulture)') || 0;
    const totalInEuros = parseFloat
        ('@ViewBag.TotalInEuros.ToString("F2", System.Globalization.CultureInfo.InvariantCulture)') || 0;
    const totalAmountElement = document.getElementById('totalAmount');
    const currencySelect = document.getElementById('currencySelect');

    function updateTotalDisplay(currency) {
        if (!totalAmountElement) return;
        if (currency === 'BRL') {
            totalAmountElement.innerHTML =
                `Total value - R$ ${totalInReais.toFixed(2)}`;
        } else if (currency === 'EUR') {
            totalAmountElement.innerHTML =
                `Total value - € ${totalInEuros.toFixed(2)}`;
        }
    }

    if (currencySelect) {
        currencySelect.value = 'BRL';
        updateTotalDisplay('BRL');
        currencySelect.addEventListener('change', function () {
            updateTotalDisplay(this.value);
        });
    }

    $('#createExpenseModal').on('hidden.bs.modal', function () {
        setTimeout(function () {
            $("#addExpenseButton").focus();
        }, 100);
    });
});

function enableValidation() {
    var form = $("#createExpenseForm");
    if (form.length === 0) {
        console.error("Form not found.");
    }
    $.validator.unobtrusive.parse(form);
}

enableValidation();

$(document).on("click", "#saveExpenseButton", function (e) {
    e.preventDefault();

    let form = $("#createExpenseForm");
    if (form.length === 0) {
        console.error("Form not found.");
        return
    }

    form.validate();

    if (!form.valid()) {
        console.warn("Form validation failed.");
        return;
    }

    $.ajax({
        url: "/Expense/Create",
        type: "POST",
        data: form.serialize(),
        success: function (response) {
            if (response.success) {
                $("#createExpenseModal").modal("hide");
                location.reload();
            } else {
                let errorContainer = $("#errorMessages");
                if (response.errors && response.errors.length > 0) {
                    errorContainer.html(response.errors.join("<br>"));
                    errorContainer.removeClass("d-none").show();
                } else {
                    errorContainer.html("Unknown error.");
                    errorContainer.removeClass("d-none").show();
                }
            }
        },
        error: function () {
            let errorContainer = $("#errorMessages");
            errorContainer.html("Error when processing request.");
            errorContainer.removeClass("d-none").show();
        }
    });
});

$(document).on("click", "[id^='saveEditButton-']", function (e) {
    e.preventDefault();

    let buttonId = $(this).attr("id");
    let expenseId = buttonId.replace("saveEditButton-", "");

    let form = $("#editExpenseForm-" + expenseId);

    form.validate();

    if (!form.valid()) {
        return;
    }

    $.ajax({
        url: "/Expense/Update",
        type: "POST",
        data: form.serialize(),
        success: function (response) {
            if (response.success) {
                $("#editModal-" + expenseId).modal("hide");
                location.reload();
            } else {
                $("#editErrorMessages-" + expenseId)
                    .html(response.errors.join("<br>"))
                    .removeClass("d-none").show();
            }
        },
        error: function () {
            $("#editErrorMessages-" + expenseId)
                .html("Error when processing request.")
                .removeClass("d-none").show();
        }
    });
});