/// <reference path="generic/jquery-1.6.2-vsdoc.js" />
/// <reference path="globals.js" />

$(function () {
    //aggiorna();
});


function aggiorna() {

    $('#message').removeClass().addClass('loading').text('Recupero dati in corso');

    $.ajax({
        type: "GET",
        url: "/Home/GetRawCtrlPage"
    }).done(function (data) {
        if (data != "ERR") {
            var html = $(data);
            var result = elabora(html);
            visualizza(result);
            $('#message').removeClass().text('');
        }
        else {
            $('#message').removeClass().addClass('warning').text('Errore recupero');
        }
    });
}

function elabora(html) {
    var result =
        {
            strStartTime: '',
            strRipresaTime: '',
            strOreFatte: '',
            strDaFare: '',
            strCurrentTime: '',
            strSettimanaliTotali: '',
            strSettimanaliDaFare: '',
            raggiuntoMinimoOre : false
        };


    result.strStartTime = html.find('#objFormSigns_lblH1').text();
    result.strRipresaTime = html.find('#objFormSigns_lblH3').text();
    var curTimeStr = html.find('#objFormSigns_lblH4').text();
    result.strSettimanaliTotali = html.find('#objHoursInThisWeek_lblWeekDaLavorare').text();

    if (curTimeStr == '')
        curTimeStr = html.find('#objFormSigns_lblH2').text();

    result.strCurrentTime = curTimeStr;

    var curTime = new Date();
    curTime.setHours(curTimeStr.split('.')[0], curTimeStr.split('.')[1], 0, 0);
    var startTime = new Date();
    startTime.setHours(result.strStartTime.split('.')[0], result.strStartTime.split('.')[1], 0, 0);
    var diff = curTime - startTime;
    diff = isNaN(diff) ? 0 : diff;
    var un_ora = 1000 * 60 * 60;
    var un_min = 1000 * 60;
    var h = Math.floor(diff / un_ora);
    var m = Math.round((diff % un_ora) / un_min);

    //Considero un'ora di pausa pranzo
    if (result.strRipresaTime != '')
        h = h - 1;

    if (h < 10)
        result.strOreFatte = '0' + h;
    else
        result.strOreFatte = h;

    if (m < 10)
        result.strOreFatte = result.strOreFatte + '.0' + m + '.00';
    else
        result.strOreFatte = result.strOreFatte + '.' + m + '.00';
    
    if ((h < 7) || (h == 7 && m < 42))
        result.raggiuntoMinimoOre = false;
    else
        result.raggiuntoMinimoOre = true;

    result.strDaFare = GetHourDiff('07.42.00', result.strOreFatte);
    var hstra = Math.floor(result.strDaFare / un_ora);
    var mstra = Math.round((result.strDaFare % un_ora) / un_min);

    //result.strSettimanaliDaFare = oreSettimanaliTot;
    result.strSettimanaliDaFare = GetHourDiff(result.strSettimanaliTotali, result.strOreFatte);

    return result;

}

function visualizza(result) {
    $('#startTime').text(result.strStartTime);

    $('#settimanaliTotali').text(result.strSettimanaliTotali);
    $('#settimanaliDaFare').text(result.strSettimanaliDaFare);

    if (result.raggiuntoMinimoOre)
        $('#settimanaliDaFare').addClass('ore_positivo');
    else
        $('#settimanaliDaFare').addClass('ore_negativo');

}

function GetHourDiff(pEndHour, pStartHour) {
    var res = "";
    var aTmp = "";
    //Trasformo l'orario di inizio in minuti
    aTmp = pStartHour.split(".");
    var nStartMin = (Number(aTmp[0]) * 60) + Number(aTmp[1]);
    //Trasformo l'orario di fine in minuti
    aTmp = pEndHour.split(".");
    var nEndMin = (Number(aTmp[0]) * 60) + Number(aTmp[1]);

    //Calcolo la differenza
    var nDiff = 0;
    if (nStartMin > nEndMin) {
        nDiff = nStartMin - nEndMin;
    }
    else {
        nDiff = nEndMin - nStartMin;
    }

    //Formatto la stringa di uscita
    var nDiffMin = 0;
    var nDiffHour = 0;
    if (nDiff > 59) {
        nDiffMin = nDiff % 60;
        nDiffHour = (nDiff - nDiffMin) / 60;
    }
    else {
        nDiffMin = nDiff;
    }

    if (nDiffHour < 10) res += "0";
    res += nDiffHour;
    res += ":";
    if (nDiffMin < 10) res += "0";
    res += nDiffMin;
    if (nStartMin < nEndMin)
        return res + '.00';
    else
        return '+' + res + '.00';
}