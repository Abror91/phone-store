$(document).ready(function () {
    loadData();
})
function loadData() {
    $.ajax({
        url: '/Orders/OrdersList',
        type: 'GET',
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (result) {
            var html = '';
            $.each(result, function (key, item) {
                html += '<tr><td>' + item.Id + '</td>';
                html += '<td>' + item.OrderDate + '</td>';
                html += '<td>' + item.UserEmail + '</td>';
                html += '<td>' + item.PhoneName + '</td>';
                html += '<td><a html="#" class="btn btn-success" onclick="getById(' + item.Id + ')">Edit</a>' +
                '<a href="#" class="btn btn-danger" onclick="deleteOrder(' + item.Id + ')">Delete</a></td></tr>';
            });
            $('#tblBody').append(html);
        },
        error: function (error) {
            alert(error.responseText);
        }
    })
}