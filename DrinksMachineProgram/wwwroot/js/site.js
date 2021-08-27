
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
    let quantity = parseInt($(this).val());

    calculateTotal();

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

function calculateTotal() {
    let total = 0;

    $("[data-field='product'] input").each(function () {
        let row = $(this).parent().data("row");
        let cost = parseFloat($(`[name="Products[${row}].Product.Cost"]`).val());
        let productQuantity = parseInt($(this).val());

        total += productQuantity * cost;
    });

    $("#txtOrderTotal").text(total);
};

$("#btnGetDrinks").click(function () {
    let data = {
        "Products": [],
        "Coins": [],
    };

    $("[data-field='coin']").each(function (index) {
        let coin = {
            "Coin": {
                "Id": $(`[name="Coins[${index}].Coin.Id"]`).val(),
                "Name": $(`[name="Coins[${index}].Coin.Name"]`).val(),
                "Value": $(`[name="Coins[${index}].Coin.Value"]`).val(),
                "QuantityAvailable": $(`[name="Coins[${index}].Coin.QuantityAvailable"]`).val()
            },
            "Quantity": $(this).find("input").val()
        }

        data.Coins.push(coin);
    });

    $("[data-field='product']").each(function (index) {
        let product = {
            "Product": {
                "Id": $(`[name="Products[${index}].Product.Id"]`).val(),
                "Name": $(`[name="Products[${index}].Product.Name"]`).val(),
                "Cost": $(`[name="Products[${index}].Product.Cost"]`).val(),
                "QuantityAvailable": $(`[name="Products[${index}].Product.QuantityAvailable"]`).val()
            },
            "Quantity": $(this).find("input").val()
        }

        data.Products.push(product);
    });

    $.ajax({
        type: "POST",
        url: $("#DataForm").attr("action"),
        dataType: "json",
        contentType: "application/json",
        data: JSON.stringify(data),
        success: function (response) {

            if (response.Success) {
                $("#ModalBody").html(response.Data);
                $("#ModalWindow").modal("show");

                var modal = document.getElementById('ModalWindow')

                modal.addEventListener("hidden.bs.modal", function () {
                    $.each(response.DataObject.Products, function (index, item) {
                        $(`[name="Products[${index}].Product.QuantityAvailable"]`).val(item.Product.QuantityAvailable);
                        $(`[name="Products[${index}].QuantityAvailable"]`).text(item.Product.QuantityAvailable);
                        $(`[name="Products[${index}].Quantity"]`).val(0);
                    });

                    $.each(response.DataObject.Coins, function (index, item) {
                        $(`[name="Coins[${index}].Coin.QuantityAvailable"]`).val(item.Coin.QuantityAvailable);
                        $(`[name="Coins[${index}].Quantity"]`).val(0);
                    });
                })

            }
            else {
                Swal.fire({
                    icon: "error",
                    title: "Oops...",
                    text: response.ErrorMessage,
                })
            }

        }
    })
});