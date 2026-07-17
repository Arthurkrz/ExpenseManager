$(document).ready(function () {
    if (typeof jQuery === "undefined") {
        console.error("jQuery is not loaded!");
        return;
    }

    const totalAmountElement = document.getElementById('totalAmount');
    const currencySelect = document.getElementById('currencySelect');

    function formatCurrency(value, currency) {
        return Number(value).toLocaleString("en-US", {
            style: "currency",
            currency: currency
        });
    }

    function updateTotalDisplay(currency) {
        if (!totalAmountElement) return;

        const total = window.expenseTotals?.[currency] ?? 0;

        totalAmountElement.innerHTML =
            `Total value - ${formatCurrency(total, currency)}`;
    }

    if (currencySelect) {
        currencySelect.value = "USD";
        updateTotalDisplay(currencySelect.value);

        currencySelect.addEventListener("change", function () {
            updateTotalDisplay(this.value);
        });
    }

    $('#createExpenseModal').on('hidden.bs.modal', function () {
        setTimeout(function () {
            $("#addExpenseButton").focus();
        }, 100);
    });

    enableValidation();
    registerCreateHandler();
    registerEditHandler();
});

function enableValidation() {
    var form = $("#createExpenseForm");
    if (form.length === 0) {
        console.error("Form not found.");
    }

    $.validator.unobtrusive.parse(form);
}

function registerCreateHandler() {
    $(document).on("submit", "#createExpenseForm", function (e) {
        e.preventDefault();

        console.log("create form submit triggered.");

        const form = $(this);
        const errorContainer = $("#errorMessages");

        errorContainer.addClass("d-none").empty();

        if ($.validator && !form.valid()) {
            console.warn("create forn valid failed,");
            return;
        }

        $.ajax({
            url: form.attr("action"),
            type: "POST",
            data: form.serialize(),
            success: function (response) {
                if (response.success) {
                    $("#createExpenseModal").modal("hide");
                    location.reload();
                }
            },

            error: function (xhr) {
                const errors = xhr.responseJSON?.errors;

                if (errors && errors.length > 0) {
                    errorContainer.html(errors.join("<br>"));
                } else {
                    errorContainer.html("Error when processing request.");
                }

                errorContainer.removeClass("d-none").show();
            }
        });
    });

}

function registerEditHandler() {
    $(document).on("click", "[id^='saveEditButton-']", function (e) {
        e.preventDefault();

        const buttonId = $(this).attr("id");
        const expenseId = buttonId.replace("saveEditButton-", "");
        
        const form = $("#editExpenseForm-" + expenseId);


        if ($.validator && !form.valid()) {
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
                }
            },

            error: function (xhr) {
                const errors = xhr.responseJSON?.errors;
                const errorContainer = $("#editErrorMessages-" + expenseId);

                if (errors && errors.length > 0) {
                    errorContainer.html(errors.join("<br>"));
                } else {
                    errorContainer.html("Error when processing request.");
                }

                errorContainer.removeClass("d-none").show();
            }
        });
    });
}