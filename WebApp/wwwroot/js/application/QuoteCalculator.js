
$(document).ready(function () {
    QuoteCalculator.initialize();
});

let QuoteCalculator = function () {
    let searchEndPoint = "/error/search";

    return {
        initialize: function () {
            App.ionRangeSlider("#sliderAmount", 2000, 100000, 1000, "$");
            
        },

        changePage: function (pageId) {
            errorSearchPagIndex = pageId;
            QuoteCalculator.executeSearch();
        },
        executeSearch: function () {
            let startDate = $("#txtDateRange").val() != "" ? $("#txtDateRange").data('daterangepicker').startDate.format(AppConstant.queryStringDateTimeFormat) : "";
            let endDate = $("#txtDateRange").val() != "" ? $("#txtDateRange").data('daterangepicker').endDate.format(AppConstant.queryStringDateTimeFormat) : "";
            let searchKeyword = $("#txtSearchKeyword").val();

            let url = searchEndPoint + `?PageNumber=${errorSearchPageIndex}`;
            url += `&StartDate=${startDate}&EndDate=${endDate}`;
            url += `&SearchKeyword=${searchKeyword}`;

            let searchurl = window.location.origin + url;
            window.location.replace(searchurl);
        },
        clearInputs: function () {
            $("#txtDateRange").val('');
            $("#txtSearchKeyword").val('');
        }
    }
}();