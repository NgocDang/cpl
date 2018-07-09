var PricePrediction = {
    init: function () {
        PricePrediction.bindLoadPredictionResult();
    },
    bindLoadPredictionResult: function () {
        var progressConnection = new signalR.HubConnection("/preditedUserProgress");
        progressConnection
            .start()
            .catch(() => {
                console.log("Error while establishing connection");
            });

        progressConnection.on("preditedUserProgress", (up, down) => {
            if (up !== undefined && down !== undefined) 
                this.setProgress(up, down);
        });
    },
    setProgress: function (up, down) {
        $("#up-bar").css({ "width": up + "%" })
            .attr("aria-valuenow", up)
            .html("<i class='fas fa-arrow-up'></i>" + up + "%");
        $("#down-bar").css({ "width": down + "%" })
            .attr("aria-valuenow", down)
            .html("<i class='fas fa-arrow-down'></i>" + down + "%");
    }
}

$(document).ready(function () {
    PricePrediction.init();
});