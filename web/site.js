$(document).ready(function () {
    $('#example').DataTable({
        "processing": true,
        "serverSide": true,        
        "ajax": {
            "url": "https://azcheapfunctionssea.azurewebsites.net/api/GetServerSideData?code=Wj9gMoAngDQH1AU0b8bvoVqNvZ8oXumOhs4hTgyZEAVyCaxQFMtfcQ==",
            "crossDomain": true,
            "dataType": "jsonp"
        }
    });
});