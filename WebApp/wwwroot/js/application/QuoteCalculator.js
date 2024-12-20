$(function () {
    QuoteCalculator.initialize();
});

let QuoteCalculator = function () {
    let quoteCalculateEndpoint = "/home/QuoteCalculate";
    let quoteDetailEndpoint = "/home/QuoteDetail";

    let btnSubmit = "#btnSubmit";
    let btnEditDetails = "#btnEditDetails";

    return {
        stepper: null,
        initialize: function () {
            App.ionRangeSlider("#sliderAmount", 2000, 100000, 1000, "$");

            $(btnSubmit).on("click", function () {
                QuoteCalculator.quoteCalculate();
            });

            this.stepper = new Stepper(document.querySelector('.bs-stepper'));
            

            $(btnEditDetails).on("click", function () {
                let quoteId = $("#hdnQuoteId");
                QuoteCalculator.prepareEditDetailsModal(quoteId);
            });


        },
        prepareEditDetailsModal: function (quoteId) {
            App.showPreloader();
            App.ajaxGet(quoteDetailEndpoint + quoteId
                , "html"
                , function (data) {
                    QuoteCalculator.initializeModalContent(data);
                    App.hidePreloader();
                },
                function (err) {
                    App.hidePreloader();
                }
            );

            $("#modalQuoteDetail").modal("show");
        },
        initializeModalContent: function (data) {
            
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
            validationMessages = App.requiredTextValidator(model.Mobile, "Mobile is required", $("#txtMobile"), validationMessages);

            validationMessages = App.emailAddressValidator(model.Email, "Email is required", $("#txtEmail"), validationMessages);

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