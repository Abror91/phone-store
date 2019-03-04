$(document).ready(function () {
    loadData();
    $('.datepicker').datepicker({
        dateFormat: 'yy-mm-dd',
        changeMonth: true,
        changeYear: true,
        yearRange: '-5: +5'
    });
})
function loadData() {
    $.ajax({
        url: '/Phones/Phone/PhonesList',
        type: 'GET',
        contentType: 'application/json;charset=utf-8;',
        dataType: 'json',
        success: function (result) {
            var html = '';
            $.each(result, function (key, item) {
                html += '<tr><td>' + item.Id + '</td>';
                html += '<td>' + item.Model + '</td>';
                html += '<td>' + item.CompanyName + '</td>';
                html += '<td>' + item.ReleaseDate + '</td>';
                html += '<td><a href="#" class="btn btn-primary" onclick="getById(' + item.Id + ')">Edit ' +
                    '</a>  <a href="#" class="btn btn-success" onclick="deletePhone(' + item.Id + ')">Delete</a></td></tr>';
            });
            $('#tblBody').empty();
            $('#tblBody').append(html);
        },
        error: function(error){
            alert(error.responseText);
    }
    })
}

function getById(phoneId) {
    $.ajax({
        url: '/Phones/Phone/GetPhone/' + phoneId,
        type: 'GET',
        contentType: 'application/json;charset=utf-8;',
        dataType: 'json',
        success: function (result) {
            $('#phoneId').val(result.Id);
            $('#phoneModel').val(result.Model);
            $('#companyName').val(result.CompanyName);
            $('#releaseDate').val(result.ReleaseDate);
            $('#phoneModal').modal('show');
            $('#btnAdd').hide();
            $('#btnEdit').show();
        },
        error: function (error) {
            alert(error.responseText);
        }
    })
}

function addPhone() {
    var res = isValid();
    if (res == false)
        return false;
    var phone = {
        Model: $('#phoneModel').val(),
        CompanyName: $('#companyName').val(),
        ReleaseDate: $('#releaseDate').val()
    };
    $.ajax({
        url: '/Phones/Phone/AddPhone',
        data: JSON.stringify(phone),
        type: 'POST',
        contentType: 'application/json;charset=utf-8',
        dataType: 'json',
        success: function (result) {
            $('#message').append(result);
            loadData();
            clearBoxes();
            $('#phoneModal').modal('hide');
        },
        error: function (error) {
            alert(error.responseText);
        }
    });
}

function editPhone() {
    var res = isValid();
    if (res == false)
        return false;
    var phone = {
        Id: $('#phoneId').val(),
        Model: $('#phoneModel').val(),
        CompanyName: $('#companyName').val(),
        ReleaseDate: $('#releaseDate').val()
    }
    $.ajax({
        url: '/Phones/Phone/EditPhone',
        data: JSON.stringify(phone),
        type: 'POST',
        contentType: 'application/json;charset=utf-8',
        dataType: 'json',
        success: function (result) {
            $('#message').append(result);
            loadData();
            clearBoxes();
            $('#phoneModal').modal('hide');
        },
        error: function (error) {
            alert(error.responseText);
        }
    })
}

function deletePhone(phoneId) {
    var answer = confirm("Are you sure you want to delete this item?");
    if (answer) {
        $.ajax({
            url: '/Phones/Phone/DeletePhone/' + phoneId,
            type: 'POST',
            contentType: 'application/json; charset=utf-8;',
            dataType: 'json',
            success: function (result) {
                $('#message').append(result);
                loadData();
            },
            error: function (error) {
                alert(error.responseText);
            }
        })
    }
}


function clearBoxes() {
    $('#phoneId').val("");
    $('#phoneModel').val("");
    $('#companyName').val("");
    $('#releaseDate').val();
    $('#phoneModel').css('border-color', 'lightgrey');
    $('#companyName').css('border-color', 'lightgrey');
    $('#releaseDate').css('border-color', 'lightgrey');
    $('#btnAdd').show();
    $('#btnEdit').hide();
}

function isValid() {
    var valid = true;
    if ($('#phoneModel').val().trim() == "") {
        $('#phoneModel').css('border-color', 'red');
        valid = false;
    }
    if ($('#companyName').val().trim() == "") {
        $('#companyName').css('border-color', 'red');
        valid = false;
    }
    if($('#releaseDate').val().trim() == ""){
        $('#releaseDate').css('border-color', 'red');
        valid = false;
    }
    return valid;
}