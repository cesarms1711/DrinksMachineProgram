
$(document).on("keypress", "[data-field='product'] input, [data-field='coin'] input", function (event) {
   
    let validValue =
        (event.which > 95 && event.which < 106) ||
        (event.which > 47 && event.which < 58) ||
        (event.which === 13);

    if (validValue === false) event.preventDefault();
 
});

var availableDrinks = true;

$("[data-field='product'] input").change(function () {
    let row = $(this).parent().data("row");
    let quantityAvailable = parseInt($(`[name="Products[${row}].Product.QuantityAvailable"]`).val());
    let cost = parseInt($(`[name="Products[${row}].Product.Cost"]`).val());
    let quantity = parseInt($(this).val());
    let total = quantity * cost;

    $("#txtOrderTotal").text(total);

    availableDrinks = quantity <= quantityAvailable;

    if (availableDrinks === false) alert("Insufficient drinks.");

    validateData();
});

$("[data-field='coin'] input").change(function () {
    validateData();
});

function validateData() {

    if (availableDrinks === false) {
        $("#btnGetDrinks").attr("disabled", true);

        return;
    }

    let totalProducts = 0;

    $("[data-field='product'] input").each(function () {
        let productQuantity = parseInt($(this).val());

        totalProducts += productQuantity;
    });

    let totalCoins = 0;

    $("[data-field='coin'] input").each(function () {
        let coinQuantity = parseInt($(this).val());

        totalCoins += coinQuantity;
    });

    if (totalProducts > 0 && totalCoins > 0) {
        $("#btnGetDrinks").removeAttr("disabled");
    }
    else {
        $("#btnGetDrinks").attr("disabled", true);
    }

};

$("#btnGetDrinks").click(function () {

});