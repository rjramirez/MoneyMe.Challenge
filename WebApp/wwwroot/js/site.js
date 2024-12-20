$(document).ready(function () {
    App.initialize();
    if (window.localStorage.getItem("toastrMsg") !== null) {
        let toastrMsg = JSON.parse(window.localStorage.getItem("toastrMsg"));
        App.alert(toastrMsg.type, toastrMsg.message, toastrMsg.title);
        window.localStorage.clear();
    }
});

const AppConstant = {
    jsonDateFormat: "YYYY-MM-DD",
    dateFormat: "MM/DD/YYYY",
    queryStringDateTimeFormat: "D MMMM YYYY hh:mm:ss A",
    createTitle: "Create ",
    updateTitle: "Update ",
    deleteTitle: "Delete ",
    deactivateTitle: "Deactivate ",
    createConfirmationMessage: "Would you like to save this record?",
    updateConfirmationMessage: "Do you want to update this record? You cannot undo this action.",
    deleteConfirmationMessage: "Are you sure you want to delete this data? You cannot undo this action.",
    deactivateConfirmationMessage: "Are you sure you want to deactivate this data? You cannot undo this action.",
    statusOnHold: 3,
    checklistStatusReturned: 1
};

var App = function () {
    return {
        initialize: function () {
            toastr.options = {
                "closeButton": true,
                "debug": false,
                "newestOnTop": false,
                "progressBar": true,
                "positionClass": "toast-top-right",
                "preventDuplicates": false,
                "onclick": null,
                "showDuration": "500",
                "hideDuration": "1000",
                "timeOut": "10000",
                "extendedTimeOut": "1000",
                "showEasing": "swing",
                "hideEasing": "linear",
                "showMethod": "fadeIn",
                "hideMethod": "fadeOut"
            }

            App.multiSelect2(".multiSelect");
        },
        ajaxPost: function (_url, _data, _dataType, _successFn, _errorFn, _boxSpinner) {
            App.ajaxCall('POST', _url, _data, _dataType, _successFn, _errorFn, _boxSpinner);
        },
        ajaxPut: function (_url, _data, _dataType, _successFn, _errorFn, _boxSpinner) {
            App.ajaxCall('PUT', _url, _data, _dataType, _successFn, _errorFn, _boxSpinner);
        },
        ajaxDelete: function (_url, _dataType, _successFn, _errorFn, _boxSpinner) {
            $.ajax({
                url: _url,
                type: 'DELETE',
                dataType: _dataType,
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    App.ajaxSuccess(_successFn, data);
                },
                error: function (jqXHR, textStatus) {
                    App.ajaxError(jqXHR, textStatus, _errorFn, _boxSpinner);
                }
            });
        },
        ajaxGet: function (_url, _dataType, _successFn, _errorFn) {
            $.ajax({
                url: _url,
                type: 'GET',
                dataType: _dataType,
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    App.ajaxSuccess(_successFn, data);
                },
                error: function (jqXHR, textStatus) {
                    App.ajaxError(jqXHR, textStatus, _errorFn);
                }
            });
        },
        ajaxCall: function (_methodType, _url, _data, _dataType, _successFn, _errorFn, _boxSpinner) {
            $.ajax({
                url: _url,
                type: _methodType,
                data: _data,
                dataType: _dataType,
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    App.ajaxSuccess(_successFn, data);
                },
                error: function (jqXHR, textStatus) {
                    App.ajaxError(jqXHR, textStatus, _errorFn, _boxSpinner);
                }
            });
        },
        ajaxPostFileUpload: function (_url, _data, _dataType, _successFn, _errorFn, _boxSpinner) {
            App.ajaxCallFileUpload('POST', _url, _data, _dataType, _successFn, _errorFn, _boxSpinner);
        },
        ajaxPutFileUpload: function (_url, _data, _dataType, _successFn, _errorFn, _boxSpinner) {
            App.ajaxCallFileUpload('PUT', _url, _data, _dataType, _successFn, _errorFn, _boxSpinner);
        },
        ajaxCallFileUpload: function (_methodType, _url, _data, _dataType, _successFn, _errorFn, _boxSpinner) {
            $.ajax({
                url: _url,
                type: _methodType,
                data: _data,
                contentType: false,
                processData: false,
                cache: false,
                success: function (data) {
                    App.ajaxSuccess(_successFn, data);
                },
                error: function (jqXHR, textStatus) {
                    App.ajaxError(jqXHR, textStatus, _errorFn, _boxSpinner);
                }
            });
        },
        ajaxSuccess: function (_successFn, _data,) {
            if (_successFn != undefined) {
                _successFn(_data);
            }
        },
        ajaxError: function (_jqXHR, _textStatus, _errorFn, _boxSpinner) {
            App.hidePreloader();
            if (_errorFn != undefined) {
                _errorFn(_jqXHR);
            }

            if (_textStatus == 'parsererror') {
                App.alert("error", 'Unexpected error occur', 'Ooops!');
            }
            else {
                let errorMessage = $.parseJSON(_jqXHR.responseText);
                if (errorMessage.message != null) {
                    App.alert("error", errorMessage.message, 'Error');
                }
                else {
                    App.alert("error", errorMessage.traceId, 'Error');
                }
            }

            if (_boxSpinner != undefined) {
                App.removeBoxSpinner($(_boxSpinner));
            }
        },
        alert: function (_type, _message, _title, _urlRedirect) {
            /*_type: value should be any of the following: success, warning, info, error
             _title: optional if you dont want to show title for the alert box
             _urlRedirect: used to delay the message after redirect to URL*/
            if (_urlRedirect != undefined) {
                localStorage.setItem("toastrMsg",
                    JSON.stringify({
                        type: _type,
                        title: _title,
                        message: _message
                    }));

                window.location.replace(_urlRedirect);
            }
            else {
                if (_title != undefined) {
                    window.toastr[_type](_message, _title);
                }
                else {
                    window.toastr[_type](_message);
                }
            }
        },
        showValidationMessage: function (validationMessage) {
            if (App.itemHasLength(validationMessage)) {
                let message = "";

                for (let value of validationMessage) {
                    message += value + "<br/>";
                }

                App.alert("error", message, "Validation Summary");
            }
        },
        confirmDialogueModal: function (_modalHeaderMessage, _modalBodyMessage, _bgClass, _onConfirmationFn, _onDenyFn, _onConfirmationParam) {
            $("#modal-confirmDialogue").modal("show");

            $("#modal-confirmDialogue-header").find("h4").text(_modalHeaderMessage);
            $("#modal-confirmDialogue-header").addClass(_bgClass);
            $("#modal-confirmDialogue-body").find("p").text(_modalBodyMessage);

            $("#modal-confirmDialogue-btnConfirm").on("click", function (e) {
                e.preventDefault();

                if (_onConfirmationFn != undefined) {

                    if (_onConfirmationParam != undefined) {
                        _onConfirmationFn(_onConfirmationParam);
                    }
                    else {
                        _onConfirmationFn();
                    }

                    $("#modal-confirmDialogue").modal("hide");
                }

                resetParams();
            });

            $("#modal-confirmDialogue-btnCancel").on("click", function (e) {
                e.preventDefault();

                if (_onDenyFn != undefined) {
                    _onDenyFn();
                }

                resetParams();
            });

            function resetParams() {
                _onConfirmationFn = undefined;
                _onDenyFn = undefined;
                _onConfirmationParam = undefined;
            }
        },
        totalFileSizeValidator: function (files, message, controlSelector, validationMessages) {
            var totalSize = 0;
            for (const file of files) {
                totalSize += (file.size / 1024 / 1024);
            }

            if (totalSize > 25) {
                validationMessages.push(message);
                controlSelector.addClass("is-invalid");
            }

            return validationMessages;
        },
        showPreloader: function () {
            let preloader = $("#divPreloader");
            preloader.removeAttr("style");
            preloader.children().removeAttr("style");
        },
        hidePreloader: function () {
            setTimeout(function () {
                let preloader = $("#divPreloader");

                if (preloader) {
                    preloader.css('height', 0);
                    setTimeout(function () {
                        preloader.children().hide();
                    }, 200);
                }
            }, 200);
        },
        getCheckboxValue: function (controlSelector) {
            return $(controlSelector).prop("checked");
        },
        setCheckboxValue: function (controlSelector, isChecked) {
            $(controlSelector).prop("checked", isChecked);
        },
        getRadioValue: function (controlSelector) {
            return $(`${controlSelector}:checked`).val();
        },
        dateSinglePicker: function (controlSelector, disablePastDates = false, hasDefault = true) {
            var date = undefined;

            if (disablePastDates && disablePastDates != undefined) {
                date = new Date();
            }

            if (!hasDefault || hasDefault == undefined) {
                hasDefault = false;
            }

            $(controlSelector).daterangepicker({
                singleDatePicker: true,
                showDropdowns: true,
                minYear: 1901,
                minDate: date,
                autoUpdateInput: hasDefault
            });

            $(controlSelector).on('apply.daterangepicker', function (ev, picker) {
                $(this).val(picker.startDate.format(AppConstant.dateFormat));
            });
        },
        dateRangePicker: function (controlSelector) {
            $(controlSelector).daterangepicker({
                timePicker: false,
                autoUpdateInput: false,
                locale: {
                    cancelLabel: 'Clear',
                    format: AppConstant.dateFormat
                },
                ranges: {
                    'This Month': [moment().startOf('month'), moment().endOf('month')],
                    'Last Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')],
                    'This Year': [moment().startOf('year'), moment().endOf('year')]
                }
            });
            $(controlSelector).on('apply.daterangepicker', function (ev, picker) {
                $(this).val(picker.startDate.format(AppConstant.dateFormat) + ' - ' + picker.endDate.format(AppConstant.dateFormat));
            });
        },
        multiSelect2: function (controlSelector) {
            $(controlSelector).select2({
                theme: 'bootstrap4',
                width: '100%',
            });

            $(controlSelector).on("select2:unselect", function (evt) {
                if (!evt.params.originalEvent) {
                    return;
                }

                evt.params.originalEvent.stopPropagation();
            });
        },
        singleSelect2: function (controlSelector, hideSearchBox, dropdownParentElement) {
            let select2Options = {
                theme: 'bootstrap4',
                width: '100%'
            }

            if (hideSearchBox) {
                select2Options.minimumResultsForSearch = 'Infinity';
            }
            if (dropdownParentElement) {
                select2Options.dropdownParent = dropdownParentElement;
            }

            $(controlSelector).select2(select2Options);
        },
        ionRangeSlider: function (selector, min, max, step) {
            // Initialize the ion range slider
            $(selector).ionRangeSlider({
                min: min, //100
                max: max, //10000
                step: step, //50
                prefix: "$",
                grid: true,
                skin: "big"
            });
        },
        requiredTextValidator: function (value, message, controlSelector, validationMessages) {

            if (value != null) {
                value = value.trim();
            }

            if (value == null || value == "") {
                validationMessages.push(message);
                controlSelector.addClass("is-invalid");
            }
            return validationMessages;
        },
        requiredSingleSelectValidator: function (value, message, controlSelector, validationMessages) {
            if (value == "" || value == 0) {
                validationMessages.push(message);
                controlSelector.addClass("is-invalid");
            }
            return validationMessages;
        },
        requiredMultiSelectValidator: function (value, message, controlSelector, validationMessages) {
            if (value.length == 0) {
                validationMessages.push(message);
                controlSelector.addClass("is-invalid");
            }
            return validationMessages;
        },
        requiredFileInputValidator: function (value, message, controlSelector, validationMessages) {
            if (value.files.length == 0) {
                validationMessages.push(message);
                controlSelector.addClass("is-invalid");
            }
            return validationMessages;
        },
        emailAddressValidator: function (value, message, controlSelector, validationMessages) {
            var regex = /^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$/;
            
            if ((value == "" || !regex.test(value))) {
                validationMessages.push(message);
                controlSelector.addClass("is-invalid");
            }

            return validationMessages;
        },
        contactNumberValidator: function (value, message, controlSelector, validationMessages) {
            var regex = /^(\+)\d{8,16}$/;

            if (value == "" || !regex.test(value)) {
                validationMessages.push(message);
                controlSelector.addClass("is-invalid");
            }
            return validationMessages;
        },
        existingTextValidator: function (value, message, controlSelector, validationMessages) {
            if (value !== undefined) {
                validationMessages.push(message);
                controlSelector.addClass("is-invalid");
            }
            else {
                controlSelector.addClass("is-valid");
            }
            return validationMessages;
        },
        dateLessThanTodayValidator: function (value, message, controlSelector, validationMessages) {
            let dt = new Date();
            let selectedDate = new Date(value);
            let currentDate = new Date(dt.toDateString());
            if (selectedDate < currentDate) {
                validationMessages.push(message);
                controlSelector.addClass("is-invalid");
            }
            return validationMessages;
        },
        fileExtensionValidator: function (file, message, controlSelector, validationMessages, templateOnly = 0) {
            let allowedExtensions = ['xls', 'xlsx', 'doc', 'docx', 'pdf', 'jpg', 'jpeg', 'png', 'msg'];

            //In case there are more than 1 format of template, add as if statement (or change to switch-case if too many)
            if (templateOnly == 1) {
                allowedExtensions = ['xlsx'];
            }

            let filelength = file.length;
            for (let i = 0; i < filelength; i++) {
                let fileExtension = file[i].name.split('.').pop();
                if (!allowedExtensions.includes(fileExtension)) {
                    validationMessages.push(message);
                    controlSelector.addClass("is-invalid");
                }
            }
            return validationMessages;
        },
        fileSizeValidator: function (file, message, controlSelector, validationMessages) {
            let filelength = file.length;
            for (let i = 0; i < filelength; i++) {
                let size = (file[i].size / 1024 / 1024);
                if (size > 25) {
                    validationMessages.push(message);
                    controlSelector.addClass("is-invalid");
                }
            }
            return validationMessages;
        },
        requiredNumericValidator: function (value, message, controlSelector, validationMessages) {
            if (value == null || value == "" || value == 0) {
                validationMessages.push(message);
                controlSelector.addClass("is-invalid");
            }
            return validationMessages;
        },
        requiredTextPairValidator: function (value1, value2, message, controlSelector1, controlSelector2, validationMessages) {
            if ((value1 == null || value1 == "") && (value2 == null || value2 == "")) {
                validationMessages.push(message);
                controlSelector1.addClass("is-invalid");
                controlSelector2.addClass("is-invalid");
            }

            return validationMessages;
        },
        addBoxSpinner: function (boxId) {
            let boxSpinnerElement = '<div class="overlay"><span class="fa fa-sync-alt fa-spin"></span></div>';
            $(boxId).prepend(boxSpinnerElement);
        },
        removeBoxSpinner: function (boxid) {
            $(boxid).find('.overlay:first-child').remove();
        },
        initBootstrapToggle: function () {
            $('[data-toggle="tooltip"]').tooltip({ boundary: 'window' });
            $('[data-toggle="popover"]').popover();
        },
        localDateString: function (value) {
            let date = new Date(value);
            return date.toLocaleDateString('en-US', {
                year: 'numeric',
                month: '2-digit',
                day: '2-digit'
            });
        },
        controlHasLength: function (controlSelector) {
            if ($(controlSelector).length > 0) {
                return true;
            }
            return false;
        },
        itemHasLength: function (item) {
            if (item != undefined && item.length > 0) {
                return true;
            }
            return false;
        },
        fileInputCustomFileLabel: function (controlSelector) {
            $(controlSelector).on('change', function () {
                $(this).next('.custom-file-label').html($(this).val());
            });
        },
        validateDateInputCurrentOrLater: function (date) {
            var inputDate = new Date(date);

            var currentDate = new Date();
            currentDate.setHours(0, 0, 0, 0);

            if (inputDate >= currentDate) {

                return true;
            }
            else {
                return false;
            }
        },
        isDateWithinBeforeRange: function (checkDate, baseDate, range) {
            var checkDateObj = new Date(checkDate);
            var baseDateObj = new Date(baseDate);
            var timeDiff = checkDateObj.getTime() - baseDateObj.getTime();
            var dayDiff = timeDiff / (1000 * 3600 * 24);
            console.log(dayDiff <= 0 && dayDiff >= -range)
            console.log(checkDateObj);
            console.log(baseDateObj);
            return dayDiff <= 0 && dayDiff >= -range;
        },
        checkDateDifference: function (checkDate, baseDate, range) {
            var checkDateObj = new Date(checkDate);
            var baseDateObj = new Date(baseDate);
            // Normalize the time part of both dates to 00:00:00
            checkDateObj.setHours(0, 0, 0, 0);
            baseDateObj.setHours(0, 0, 0, 0);

            var timeDiff = checkDateObj.getTime() - baseDateObj.getTime();
            var dayDiff = timeDiff / (1000 * 3600 * 24);
            console.log(dayDiff);
            console.log(range);
            return dayDiff <= range;
        },
        initializePagingButton: function (pagingFn, btnClassSelector) {
            let btnClassName = ".btnSearchResultPaging";

            if (btnClassSelector != undefined) {
                btnClassName = "." + btnClassSelector;
            }

            $(btnClassName).on("click", function () {
                let pageNumber = $(this).data("pagenumber");
                pagingFn(pageNumber);
            });
        },
        initializePagingButton2: function (SearchPage) {
            $("#txtPageClickPrevious").click(function () {
                var PageClickEvent = $("#txtPageClickEvent").val();
                var CurrentPage = $("#txtCurrentPage").val();
                var x = parseInt(CurrentPage, 10);

                SearchPage.changePage(x - 1);
            });

            $("#txtPageClickNext").click(function () {
                var PageClickEvent = $("#txtPageClickEvent").val();
                var CurrentPage = $("#txtCurrentPage").val();
                var x = parseInt(CurrentPage, 10);

                SearchPage.changePage(x + 1);
            });

            let buttons = document.querySelectorAll(".txtPageClick");
            for (var i = 0; i < buttons.length; i++) {
                buttons[i].addEventListener("click", function () {
                    var PageClickEvent = $("#txtPageClickEvent").val();

                    SearchPage.changePage(this.value);
                });
            }
        }

    }
}();








