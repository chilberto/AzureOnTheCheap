$(document).ready(function () {
    $('#example').DataTable({        
        ajax: {
            url: "tests",
            dataType: 'json'
        }
    });
});