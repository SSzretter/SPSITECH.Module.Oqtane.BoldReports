/* Module Script */
var SPSITECH = SPSITECH || {};

SPSITECH.BoldReports = {

    // Interop file to render the Blazor Report Designer component with properties.
    myReportDesginerOptions: null,
    RenderDesigner: function (elementID, reportDesignerOptions) {
        myReportDesginerOptions = reportDesignerOptions;
        $("#" + elementID).boldReportDesigner({
            serviceUrl: reportDesignerOptions.serviceURL,
            ajaxBeforeLoad: this.onBRAjaxRequest
            // serviceAuthorizationToken: reportDesignerOptions.authorizationToken
        });
        //var designerObj = $("#" + elementID).data("boldReportDesigner");
        //designerObj.openReport("company-sales");
    },

    onBRAjaxRequest(args) {
        args.headers.push({ Key: 'Authorization', Value: 'Bearer ' + myReportDesginerOptions.authorizationToken });
    },

    RenderReportViewer(reportPathInfo, elementID, reportDesignerOptions) {
        myReportDesginerOptions = reportDesignerOptions;
        document.getElementById(elementID).boldReportViewer({
            reportServiceUrl: reportDesignerOptions.serviceURL,
            reportPath: reportPathInfo,
            ajaxBeforeLoad: this.onBRAjaxRequest
        });
    },

};