$(document).ready(function () {
    $('#example').DataTable({        
        ajax: {
            url: "https://azcheapfunctionssea.azurewebsites.net/api/GetServerSideData?code=2Rxwq/JjdttZwX4477wm99aLggxJu71g34dUvswyfiQOo6/aB7SXTw==",
            dataType: 'json'
        }
    });
});
