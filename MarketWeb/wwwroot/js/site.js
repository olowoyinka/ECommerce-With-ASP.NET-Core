// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function modifyTitle(id) {
    //$("#divloader").show();

    var options = {};
    options.url = "/ShoppingCart/OrderNow/" + id;
    options.type = "GET";
    options.contentType = "application/json";
    options.dataType = "json";
    
    $.ajax(options);

    $.ajax({
        success: function (option) {
            location.reload(true);
            //$("#divloader").hide();
            alert('Cart Added');
        },
    })

    
}
