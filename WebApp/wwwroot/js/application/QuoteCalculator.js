$(function () {
    QuoteCalculator.initialize();
});

let QuoteCalculator = function () {
    let quoteCalculateEndpoint = "/home/QuoteCalculate";
    let quoteDetailEndpoint = "/home/QuoteDetail";

    let btnSubmit = "#btnSubmit";
    let btnEditDetails = "#btnEditDetails";
    let btnQuoteDetailModalSave = "#btnQuoteDetailModalSave";

    let blackList = [];

    return {
        stepper: null,
        initialize: function () {
            App.ionRangeSlider("#sliderAmount", 2000, 100000, 1000, "$");

            $(btnSubmit).on("click", function () {
                QuoteCalculator.quoteCalculate();
            });

            this.stepper = new Stepper(document.querySelector('.bs-stepper'));
            
            QuoteCalculator.prepareBlockList();
        },
        prepareBlockList: function () {
            let url = "/home/GetBlackList";

            App.addBoxSpinner("#card-quote-detail");
            App.ajaxGet(url
                , "json"
                , function (response) {
                    if (response.isSuccessful) {
                        blackList = response.data
                            .map(item => item.value || item.name)
                            .filter(item => item);
                    }
                }
            );
            App.removeBoxSpinner("#card-quote-detail");
        },
        prepareEditDetailsModal: function (quoteId) {
            App.showPreloader();
            App.ajaxGet(quoteDetailEndpoint + quoteId
                , "html"
                , function (data) {
                    QuoteCalculator.initializeModalContent(data);

                    
                    $(btnQuoteDetailModalSave).on("click", function () {
                        let quoteId = $("#hdnQuoteId");
                        QuoteCalculator.quoteEditCalculate(quoteId);
                    });

                    App.hidePreloader();
                },
                function (err) {
                    App.hidePreloader();
                }
            );

            $("#modalQuoteDetail").modal("show");
        },
        clearValidation: function () {
            $("#selectProduct").removeClass("is-invalid");
            $("#sliderAmount").removeClass("is-invalid");
            $("#selectTerm").removeClass("is-invalid");
            $("#txtTitle").removeClass("is-invalid");
            $("#txtFirstName").removeClass("is-invalid");
            $("#txtLastName").removeClass("is-invalid");
            $("#dpDateOfBirth").removeClass("is-invalid");
            $("#txtEmail").removeClass("is-invalid");
            $("#txtMobile").removeClass("is-invalid");
        },
        createModel: function () {
            let model = {};
            model.Product = $("#selectProduct").val();
            model.Amount = $("#sliderAmount").val();
            model.Term = $("#selectTerm").val();
            model.Title = $("#txtTitle").val();
            model.FirstName = $("#txtFirstName").val();
            model.LastName = $("#txtLastName").val();
            model.DateOfBirth = $("#dpDateOfBirth").val();
            model.Email = $("#txtEmail").val();
            model.Mobile = $("#txtMobile").val();

            return model;
        },
        validateModel: function (model) {
            QuoteCalculator.clearValidation();

            let validationMessages = [];

            validationMessages = App.requiredTextValidator(model.Title, "Title is required", $("#txtTitle"), validationMessages);
            validationMessages = App.requiredTextValidator(model.FirstName, "FirstName is required", $("#txtFirstName"), validationMessages);
            validationMessages = App.requiredTextValidator(model.LastName, "LastName is required", $("#txtLastName"), validationMessages);
            validationMessages = App.requiredTextValidator(model.DateOfBirth, "Date Of Birth is required", $("#dpDateOfBirth"), validationMessages);
            validationMessages = App.requiredTextValidator(model.Mobile, "Mobile is required", $("#txtMobile"), validationMessages, blackList);

            validationMessages = App.emailAddressValidator(model.Email, "Email is required", $("#txtEmail"), validationMessages, blackList);

            if (validationMessages.length > 0) {
                App.showValidationMessage(validationMessages);
                return false;
            }

            return true;
        },

        quoteCalculate: function () {
            let model = QuoteCalculator.createModel();
            if (QuoteCalculator.validateModel(model)) {
                App.showPreloader();
                App.ajaxPost(quoteCalculateEndpoint
                    , JSON.stringify(model)
                    , "html"
                    , function (data) {
                        QuoteCalculator.stepper.to(2);
                        $("#quotationDetailContainer").html(data);

                        App.alert("success", `Quote successfully created`);

                        $(btnEditDetails).on("click", function () {
                            let quoteId = $("#hdnQuoteId");
                            QuoteCalculator.prepareEditDetailsModal(quoteId);
                        });

                        App.hidePreloader();
                        
                    }
                    , function (err) {
                        App.hidePreloader();
                    }
                );
            }

            quoteEditCalculate: function () {
                let model = QuoteCalculator.createModel();
                if (QuoteCalculator.validateModel(model)) {
                    App.showPreloader();
                    App.ajaxPost(quoteCalculateEndpoint
                        , JSON.stringify(model)
                        , "html"
                        , function (data) {
                            QuoteCalculator.stepper.to(2);
                            $("#quotationDetailContainer").html(data);

                            App.alert("success", `Quote successfully created`);

                            $(btnEditDetails).on("click", function () {
                                let quoteId = $("#hdnQuoteId");
                                QuoteCalculator.prepareEditDetailsModal(quoteId);
                            });

                            App.hidePreloader();

                        }
                        , function (err) {
                            App.hidePreloader();
                        }
                    );
                }
        }
    }
}();