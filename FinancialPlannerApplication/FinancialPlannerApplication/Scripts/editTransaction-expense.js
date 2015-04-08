$(document).ready(function () {
    $(".form-control").on("keyup", function () {
        $("#saveExpenseTransactionButton").prop("disabled", false);
    });

    $(".form-control").on("change", function () {
        $("#saveExpenseTransactionButton").prop("disabled", false);
    });

    $("#addExpenseTransaction #editExpenseTransactionForm #saveExpenseTransactionButton").on("click", function () {
        debugger;
        var form = $("#editExpenseTransactionForm");

        $.ajax({
            url: form.attr("action"),
            type: "post",
            data: form.serialize(),
            success: function (response) {
                $("#addExpenseTransaction").html(response);
                $("#saveExpenseTransactionButton").prop("disabled", true);

            }
        });
    });

    $("#editExpenseTransaction #editExpenseTransactionForm #saveExpenseTransactionButton").on("click", function () {
        debugger;
        var form = $("#editExpenseTransactionForm");

        $.ajax({
            url: form.attr("action"),
            type: "post",
            data: form.serialize(),
            success: function (response) {
                $("#editExpenseTransaction").html(response);
                $("#saveExpenseTransactionButton").prop("disabled", true);
            }
        });
    });

    $("#addExpenseTransaction #editExpenseTransactionForm #doneEditExpenseTransactionButton").on("click", function () {
        var form = $("#editExpenseTransactionForm");

        $.ajax(
        {
            url: form.attr("action") + "?isDone=true",
            type: "post",
            data: form.serialize(),
            success: function (response) {
                $("body").html(response);
                //$("#saveAccountTransactionButton").prop("disabled", true);

            }
        });
    });

    $("#editExpenseTransaction #editExpenseTransactionForm #doneEditExpenseTransactionButton").on("click", function () {
        var form = $("#editExpenseTransactionForm");

        $.ajax(
        {
            url: form.attr("action") + "?isDone=true",
            type: "post",
            data: form.serialize(),
            success: function (response) {
                $("body").html(response);
                //$("#saveAccountTransactionButton").prop("disabled", true);
            }
        });
    });

    $("#SelectedBudgetId").on("change", function () {
        debugger;
        var form = $("#editExpenseTransactionForm");

        var values = form.serializeArray();

        for (var index = 0; index < values.length; ++index) {
            if (values[index].name === "FindingBudgetItems") {
                values[index].value = true;
                break;
            }
        }

        $.ajax({
            url: form.attr("action"),
            type: "post",
            data: values,
            success: function (response) {
                var isEdit = $("#editExpenseTransaction").is(":visible");
                if (isEdit) {
                    $("#editExpenseTransaction").html(response);
                } else {
                    $("#addExpenseTransaction").html(response);
                }

            }
        });
    });

});