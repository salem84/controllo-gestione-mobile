/// <reference path="generic/jquery-1.6.2-vsdoc.js" />
/// <reference path="globals.js" />


function firma() {
    var s_username = localStorage.getItem('username');
    var s_password = localStorage.getItem('password');

    var user = {
        Username: s_username,
        Password: s_password
    };

    $.ajax({
        type: "POST",
        url: "/Home/Firma",
        data: $.postify(user)
    }).done(function (data) {
        var divLogin = $('#logindisplay');
        var divResult;
        if (data == 'OK') {
            divResult = $('<div>Firmato</div>');
        }
        else {
            divResult = $('<div>Errore</div>').addClass('warning');
        }
        divResult.appendTo(divLogin).delay(4000).hide('slow');
        
    });
}

$(function () {
    /*var color = ['#0101c3', '#0101c3', '#0101c3'];
    var links = $('.listview a');
    links.each(function (index) {
        $(this).css('background-color', color[index]);
    });*/

    var s_username = localStorage.getItem('username');
    var s_password = localStorage.getItem('password');
    if (s_username && s_password) {
        $('#logindisplay').addClass('success').text('Benvenuto ' + s_username);
        Globals.IsValidConfig = true;

        // Verifico la cache
        $.ajax({
            type: "GET",
            url: "/Home/IsValidCache"
        }).done(function (data) {
            if (data == 'ERR' || data != s_username) {
                var user = {
                    Username: s_username,
                    Password: s_password
                };

                $.ajax({
                    type: "POST",
                    url: "/Home/Configura",
                    data: $.postify(user)
                });
            }
        });
    }
    else {
        $('#logindisplay').addClass('warning').text('Configurazione non valida');
        Globals.IsValidConfig = false;
    }

    if (Globals.IsValidConfig == false) {
        $('.listview a:first').hide();
        $('.listview a:last').hide();
    }
});