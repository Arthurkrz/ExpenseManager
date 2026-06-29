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
            DateRangeStart: $("#DateRangeStart").val(),
            DateRangeEnd: $("#DateRangeEnd").val(),
            ValueStringRangeStart: $("#ValueStringRangeStart").val(),
            ValueStringRangeEnd: $("#ValueStringRangeEnd").val()
        };

        $.ajax({
            url: "/Filter/Expense",
            type: "POST",
            data: filterData,
            success: function (response) {
                if (response.success) {
                    $("#filteredResults").html(response);
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
                $("#errorMessages")
                    .html("Error when processing request.")
                    .removeClass("d-none").show();
            }
        });
    });
});