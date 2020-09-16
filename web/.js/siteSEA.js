$(document).ready(function () {
    $('#example').DataTable({
        "proccessing": true,
        "serverSide": true,
        "filter": false,
        "ordering": false,
        "ajax": {
            url: "https://azcheapfunctionssea.azurewebsites.net/api/GetServerSideData?code=2Rxwq/JjdttZwX4477wm99aLggxJu71g34dUvswyfiQOo6/aB7SXTw==",
            type: "POST",
            contentType: "application/json",
            data: function (d) {
                return JSON.stringify(d);
            }
        }
    });
});