const uri = 'api/todo';
let todos = null;
let dogslist = ["IMG_4241.JPG", "IMG_4242.JPG"];

function getCount(data) {
    const el = $('#counter');
    let name = 'to-do';
    if (data) {
        if (data > 1) {
            name = 'to-dos';
        }
        el.text(data + ' ' + name);
    } else {
        el.html('No ' + name);
    }
}

$(document).ready(function () {
    getDogs();
});

function getDogs() {
    $.ajax({
        function(dogslist) {
            console.log(dogslist);
            $('#dogs').empty();
            $.each(dogslist, function (item) {
                $('<tr>' +
                    '<td><img src="' + item + '" style="max-width: 100%; height: auto;"></td>' +
                    '</tr>').appendTo($('#dogs'));
            });
        }
    });
}

function addItem() {
    const item = {
        'name': $('#add-name').val(),
        'isComplete': false,
        'dateDue': $('#add-date').val()
    };

    $.ajax({
        type: 'POST',
        accepts: 'application/json',
        url: uri,
        contentType: 'application/json',
        beforeSend: function (request) {
            request.setRequestHeader("CanAccess", "true");
        },
        data: JSON.stringify(item),
        error: function (jqXHR, textStatus, errorThrown) {
            alert('Either name is missing or Date is invalid or not in the future');
        },
        success: function (result) {
            getData();
            $('#add-name').val(''), $('#add-dateDue').val('');
        }
    });
}

function deleteItem(id) {
    $.ajax({
        url: uri + '/' + id,
        beforeSend: function (request) {
            request.setRequestHeader("CanAccess", "true");
        },
        type: 'DELETE',
        success: function (result) {
            getData();
        }
    });
}

function editItem(id) {
    $.each(todos, function (key, item) {
        if (item.id === id) {
            $('#edit-name').val(item.name);
            $('#edit-id').val(item.id);
            $('#edit-isComplete').val(item.isComplete);
            $('#edit-date').val(item.dateDue.substring(0,10));
        }
    });
    $('#spoiler').css({ 'display': 'block' });
}

$('.my-form').on('submit', function () {
    const item = {
        'name': $('#edit-name').val(),
        'isComplete': $('#edit-isComplete').is(':checked'),
        'id': $('#edit-id').val(),
        'dateDue': $('#edit-date').val()
    };

    $.ajax({
        url: uri + '/' + $('#edit-id').val(),
        beforeSend: function (request) {
            request.setRequestHeader("CanAccess", "true");
        },
        type: 'PUT',
        accepts: 'application/json',
        contentType: 'application/json',
        data: JSON.stringify(item),
        success: function (result) {
            getData();
        }
    });

    closeInput();
    return false;
});

function closeInput() {
    $('#spoiler').css({ 'display': 'none' });
}