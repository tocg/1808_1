
var baseUrl = 'http://localhost:60834/';

function getQueryVariable(variable) {
    var query = window.location.search.substring(1);
    var vars = query.split("&");
    for (var i = 0; i < vars.length; i++) {
        var pair = vars[i].split("=");
        if (pair[0] == variable) { return pair[1]; }
    }
    return (false);
}

function post(url, data, callback) {
    $.ajax({
        url: url,
        type: 'post',
        data: JSON.stringify(data),
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        success: callback
    });
}


function postFile(url, data, callback) {
    console.log('test post file');
    $.ajax({
        url: url,
        type: 'post',
        data: data,
        async: true,
        contentType: false,
        processData: false,
        success: callback
    });
}