$(document).ready(function () {
    $('#example').DataTable({        
        ajax: {
            url: "test",
            dataType: 'json'
        }
    });
});