$(document).ready(function () {
    $('#example').DataTable({
        "proccessing": true,
        "serverSide": true,
        "filter": false,
        "ordering": false,
        "ajax": {
            url: "https://azcheapfunctionsuk.azurewebsites.net/api/GetServerSideData?code=6QTHUPyRvjCprL870alyRMzBElRzfIsVEVjgxoROY1U4mkRMZ8aEdA==",
            type: "POST",
            contentType: "application/json",
            data: function (d) {
                return JSON.stringify(d);
            }
        }
    });
});