$(document).ready(function() {

    $(".form-control").on("change", function() {
        $("#saveBudgetButton").prop("disabled", false);
    });

    $(".budgetName").on("keyup", function() {
        $("#saveBudgetButton").prop("disabled", false);
    });

    $("#editBudget #editBudgetForm #saveBudgetButton").on("click", function () {
        debugger;
        var form = $("#editBudgetForm");

        $.ajax(
        {
            url: form.attr("action"),
            type: "post",
            data: form.serialize(),
            success: function(response) {
                $("#editBudget").html(response);
                $("#saveBudgetButton").prop("disabled", true);

            }
        });
    });

    $("#addBudget #editBudgetForm #saveBudgetButton").on("click", function () {
        debugger;
        var form = $("#editBudgetForm");

        $.ajax(
        {
            url: form.attr("action"),
            type: "post",
            data: form.serialize(),
            success: function (response) {
                $("#addBudget").html(response);
                $("#saveBudgetButton").prop("disabled", true);
            }
        });
    });

});