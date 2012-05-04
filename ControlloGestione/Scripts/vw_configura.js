/// <reference path="generic/jquery-1.6.2-vsdoc.js" />

$(function () {
    $('#btnSave').click(function () {
        var s_username = $('#config_form #username').val().toLowerCase();
        var s_password = $('#config_form #password').val();

        var user = {
            Username: s_username,
            Password: s_password
        };

        $('#message').removeClass().addClass('loading').text('Salvataggio in corso');

        $.ajax({
            type: "POST",
            url: "/Home/Configura",
            data: $.postify(user)
        }).done(function (data) {
            if (data == 'OK') {
                saveStorage(s_username, s_password);
                $('#message').removeClass().addClass('success').text('Salvato');
                $('#fields').hide();
                $('#btnSave').hide();
            }
            else {
                $('#message').removeClass().addClass('warning').text('Errore');
            }
        });
    });


    function readStorage(username, password) {
        //var storage = window['localStorage'];

        //if (!window[type + 'Storage']) return;

        var s_username = localStorage.getItem('username');
        if (s_username) {
            username.val(s_username);
        }

        var s_password = localStorage.getItem('password');
        if (s_password) {
            password.val(s_password);
        }
    }

    function saveStorage(s_username, s_password) {
        localStorage.setItem('username', s_username);
        localStorage.setItem('password', s_password);
    }

    readStorage($('#config_form #username'), $('#config_form #password'));
});