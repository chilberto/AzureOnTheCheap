$(document).ready(function () {
    $('#example').DataTable({        
        ajax: {
            url: "https://azcheapfunctionsus.azurewebsites.net/api/GetServerSideData?code=RUI1TR61Q7AoaTOQgP8au/iXIUbng5d1HhCogCQeJOTxqTMUzNVPgw==",
            dataType: 'json'
        }
    });
});
