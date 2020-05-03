$(document).ready(function () {
    $('#example').DataTable({        
        ajax: {
            url: "https://azcheapfunctionsuk.azurewebsites.net/api/GetServerSideData?code=6QTHUPyRvjCprL870alyRMzBElRzfIsVEVjgxoROY1U4mkRMZ8aEdA==",
            dataType: 'json'
        }
    });
});
