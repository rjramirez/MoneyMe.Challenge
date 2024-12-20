$(function () {
    QuoteCalculator.initialize();
});

let QuoteCalculator = function () {
    let quoteCalculateEndpoint = "/home/QuoteCalculate";
    let btnSubmit = "#btnSubmit";

    return {
        initialize: function () {
            App.ionRangeSlider("#sliderAmount", 2000, 100000, 1000, "$");
            $(btnSubmit).on("click", function () {
                QuoteCalculator.quoteCalculate();
            });
        },
        quoteCalculate: function () {
            let amount = $("#sliderAmount").val();
            let title = $("#txtTitle").val();
            let firstName = $("#txtFirstName").val();
            let lastName = $("#txtLastName").val();
            let email = $("#txtEmail").val();
            let mobileNumber = $("#txtMobile").val();

            let url = quoteCalculateEndpoint + `?Amount=${amount}&Title=${title}&FirstName=${firstName}&LastName=${lastName}&Email=${email}&MobileNumber=${mobileNumber}`;
            let quoteCalculateUrl = window.location.origin + url;
            window.location.replace(quoteCalculateUrl);
        }
    }
}();