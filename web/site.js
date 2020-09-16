$(document).ready(function () {
    $('#example').DataTable({    
        "proccessing": true,
        "serverSide": true,
        "ajax": {
            url: "http://localhost:7071/api/GetServerSideData",
            type: "POST",
            contentType: "application/json",            
            data: function (d) {
                return JSON.stringify(d);
            }
        }
    });
});