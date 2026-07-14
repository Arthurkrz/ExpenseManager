document.addEventListener("DOMContentLoaded", function () {
    const dateLabel = document.getElementById("dateLabel");
    const valueLabel = document.getElementById("valueLabel");
    const dateSidebar = document.getElementById("dateSidebar");
    const valueSidebar = document.getElementById("valueSidebar");

    function positionSidebar(label, sidebar) {
        const rect = label.getBoundingClientRect();
        const parentRect = label.closest(".card").getBoundingClientRect();

        sidebar.style.position = "absolute";
        sidebar.style.top = `${rect.top - parentRect.top + label.offsetHeight + 5}px`;
        sidebar.style.left = `${rect.left - parentRect.left + 10}px`;
        sidebar.classList.remove("d-none");
    }

    function hideSidebar(sidebar) {
        setTimeout(() => {
            if (!sidebar.matches(":hover")) {
                sidebar.classList.add("d-none");
            }
        }, 200);
    }

    dateLabel.addEventListener("mouseenter", () => positionSidebar(dateLabel, dateSidebar));
    valueLabel.addEventListener("mouseenter", () => positionSidebar(valueLabel, valueSidebar));

    dateSidebar.addEventListener("mouseenter", () => dateSidebar.classList.remove("d-none"));
    valueSidebar.addEventListener("mouseenter", () => valueSidebar.classList.remove("d-none"));

    dateLabel.addEventListener("mouseleave", () => hideSidebar(dateSidebar));
    dateSidebar.addEventListener("mouseleave", () => hideSidebar(dateSidebar));

    valueLabel.addEventListener("mouseleave", () => hideSidebar(valueSidebar));
    valueSidebar.addEventListener("mouseleave", () => hideSidebar(valueSidebar));

});

$(document).ready(function () {
    $("#filterSearchButton").on("click", function (e) {
        e.preventDefault();

        let filterData = {
            NameContains: $("#NameContains").val(),
            SourceContains: $("#SourceContains").val(),
            Currency: $("#Currency").val(),
            Type: $("#Type").val(),
            Month: $("#Month").val(),
            DateRangeStart: $("#DateRangeStart").val(),
            DateRangeEnd: $("#DateRangeEnd").val(),
            ValueStringRangeStart: $("#ValueStringRangeStart").val(),
            ValueStringRangeEnd: $("#ValueStringRangeEnd").val()
        };

        $.ajax({
            url: "/Expense/Filter",
            type: "POST",
            data: filterData,
            success: function (html) {
                $("#filteredResults").html(html);
                $("#errorMessages").addClass("d-none").empty();
            },

            error: function (xhr) {
                let errorContainer = $("#errorMessages");

                if (xhr.responseJSON
                    && xhr.responseJSON.errors
                    && xhr.responseJSON.errors.length > 0) {
                    errorContainer.html(xhr.responseJSON.errors.join("<br>"));
                } else {
                    errorContainer.html("An unexpected error occurred.");
                }

                errorContainer.removeClass("d-none").show();
            }
        });
    });
});