$(document).ready(function () {
    $('#example').DataTable({
        "proccessing": true,
        "serverSide": true,
        "filter": false,
        "ordering": false,
        "ajax": {
            url: "https://azcheapfunctionsus.azurewebsites.net/api/GetServerSideData?code=RUI1TR61Q7AoaTOQgP8au/iXIUbng5d1HhCogCQeJOTxqTMUzNVPgw==",
            type: "POST",
            contentType: "application/json",
            data: function (d) {
                return JSON.stringify(d);
            }
        }
    });
});