// Forms functions

$(function () {

    $.fn.hasData = function (key) {
        return typeof $(this).data(key) !== "undefined";
    };

});

// Catalogues functions

function loadTable(urlTable, tableSelector) {
    $.ajax({
        type: "POST",
        url: urlTable,
        dataType: "json",
        contentType: "application/json",
        success: function (response) {

            if (response.Success) {
                $(tableSelector).html(response.Data);

                $(`${tableSelector} table`).DataTable({
                    responsive: true,
                    lengthMenu: [[15, 25, 50], [15, 25, 50]]
                });

            } else {
                $(tableSelector).empty();

                Swal.fire({
                    icon: "error",
                    title: "Oops...",
                    text: response.Message,
                })
            }

        }
    });
}

function onlySaveData(data, dataForm, callback) {
    $.ajax({
        type: "POST",
        url: dataForm.attr("action"),
        dataType: "json",
        contentType: "application/json",
        data: data,
        success: function (response) {

            if (response.Success) {
                Swal.fire({
                    icon: "success",
                    title: "Success...",
                    text: response.SuccessMessage,
                })

                callback();
            } else {
                Swal.fire({
                    icon: "error",
                    title: "Oops...",
                    text: response.ErrorMessage,
                })
            }

        }
    });
}

function getFormData(form) {
    var serializedArray = form.serializeArray();
    var json = {};

    $.map(serializedArray,
        function (value) {
            json[value["name"]] = value["value"];
        });

    return JSON.stringify(json);
}

function saveData(dataForm, modalWindow, callback, callbackParameters) {
    var data = this.getFormData(dataForm);

    $.ajax({
        type: "POST",
        url: dataForm.attr("action"),
        dataType: "json",
        contentType: "application/json",
        data: data,
        success: function (response) {
            processResponse(response, dataForm, modalWindow, callback, callbackParameters);
        }
    });
}

function processResponse(response, dataForm, modalWindow, callback, callbackParameters) {

    if (response.Success) {
        Swal.fire({
            icon: "success",
            title: "Success...",
            text: response.SuccessMessage,
        })

        modalWindow.modal("hide");

        callback(callbackParameters);
    } else {
        dataForm.children("[data-input='true']").removeClass("has-error");

        showValidationSummary(response.ErrorList, dataForm);
    }

}

function processSimpleResponse(response, callback) {

    if (response.Success) {
        bootbox.alert({
            message: response.SuccessMessage,
            size: "small",
            centerVertical: true
        });

        callback();
    } else {
        Swal.fire({
            icon: "error",
            title: "Oops...",
            text: response.ErrorMessage,
        })
    }

}

function deleteData(action, data, confirmationMessage, yesText, cancelText, callback, callbackParameters) {
    applyProcess(action, data, confirmationMessage, yesText, cancelText, callback, callbackParameters);
}

function applyProcess(action, data, confirmationMessage, yesText, cancelText, callback, callbackParameters) {
    Swal.fire({
        title: "Confirmation",
        text: confirmationMessage,
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: yesText,
        cancelButtonText: cancelText
    }).then((result) => {

        if (result.isConfirmed) {
            $.ajax({
                type: "POST",
                url: action,
                dataType: "json",
                contentType: "application/json",
                data: data,
                success: function (response) {

                    if (response.Success) {
                        Swal.fire({
                            icon: "success",
                            title: "Success...",
                            text: response.SuccessMessage,
                        }).then(() => {
                            callback(callbackParameters);
                        });
                    } else {
                        Swal.fire({
                            icon: "error",
                            title: "Oops...",
                            text: response.ErrorMessage,
                        })
                    }

                }
            });
        }

    });
}

function showValidationSummary(errorList, dataForm) {
    var $validationSummary = dataForm.children("#ValidationSummary");

    $validationSummary.empty();

    $(".invalid-feedback").remove();

    $.each(errorList, function (index, error) {
        var inputValidated = dataForm.find(`#${error.property}`);

        createErrorMessages($validationSummary, inputValidated, error.messages);
    });
}

function createErrorMessages(validationSummary, inputValidated, messages) {
    var $closeButton = $("<button>", { "class": "btn-close", type: "button", "data-bs-dismiss": "alert" });

    inputValidated.addClass("is-invalid");

    var $errorList = $("<ul>", { "class": "simple-list p-0 mb-1" });

    $.each(messages, function (index, message) {
        var $errorItem = $("<li>").html(message);

        $errorList.append($errorItem);

        var $alert = $("<div>", { "class": "alert alert-danger alert-dismissible mb-2", role: "alert" });

        $alert.append($closeButton);
        $alert.append(message);

        validationSummary.append($alert);
    });

    var $validationMessage = $("<div>", { "class": "invalid-feedback" });

    $validationMessage.append($errorList);

    inputValidated.after($validationMessage);
}

form.prototype = {
    getFormData: function () {
        return getFormData(this.dataForm);
    },
    showModal: function (data) {
        var modalWindowSelector = this.modalWindowId || "ModalWindow";
        var modalContainerSelector = this.modalContainerId || "ModalContainer";

        this.modalWindow = $(`#${modalWindowSelector}`);
        this.modalContainer = $(`#${modalContainerSelector}`);

        this.modalContainer.html(data);

        var self = this;

        $("#SaveButton").click(function () {
            self.save();
        });

        this.modalWindow.modal("show");
    },
    hideModal: function () {
        this.modalWindow.modal("hide");
    },
    create: function () {
        var self = this;

        $.get(this.createUrl,
            function (data) {
                self.showModal(data);
            });
    },
    edit: function edit(id) {
        var self = this;

        $.get(this.editUrl,
            { id },
            function (data) {
                self.showModal(data);
            });
    },
    detail: function detail(id) {
        var self = this;

        $.get(this.detailUrl,
            { id },
            function (data) {
                self.showModal(data);
            });
    },
    save: function () {

        if (this.saveFunction === undefined) {
            var dataFormSelector = this.dataFormId || "DataForm";

            this.dataForm = $(`#${dataFormSelector}`);

            saveData(this.dataForm, this.modalWindow, this.reloadList, this);
        } else {
            this.saveFunction();
        }

    },
    delete: function (id) {
        var data = JSON.stringify({ Id: id });

        deleteData(this.deleteUrl,
            data,
            this.deleteMessage,
            this.yesMessage,
            this.noMessage,
            this.reloadList,
            this);
    },
    processResponse: function (response) {
        processResponse(response, this.dataForm, this.modalWindow, this.reloadList);
    },
    processSimpleReponse: function (response) {
        processSimpleResponse(response, this.reloadList);
    },
    reloadList: function (form) {

        if (form === undefined || form === null) {
            form = this;
        }

        var tableSelector = form.tableContainerId === undefined ?
            "#TableContainer" : `#${form.tableContainerId}`;

        loadTable(form.tableUrl, tableSelector);
    }
};

function form() {
    var newForm = Object.create(form.prototype);

    // Action Url's

    newForm.tableUrl = "";
    newForm.createUrl = "";
    newForm.editUrl = "";
    newForm.detailUrl = "";
    newForm.deleteUrl = "";

    // Messages

    newForm.deleteMessage = "";
    newForm.yesMessage = "";
    newForm.noMessage = "";

    // Custom functions

    newForm.saveFunction = undefined;

    // Object Id's

    newForm.dataFormId = undefined;
    newForm.tableContainerId = undefined;
    newForm.modalContainerId = undefined;
    newForm.modalWindowId = undefined;

    newForm.modalContainer = null;
    newForm.modalWindow = null;
    newForm.dataForm = null;

    return newForm;
}