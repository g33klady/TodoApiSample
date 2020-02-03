const uri = 'api/todo';
let todos = null;

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
    getData();
});

function getData() {
    $.ajax({
        type: 'GET',
        url: uri,
        beforeSend: function (request) {
            request.setRequestHeader("CanAccess", "true");
        },
        success: function (data) {
            $('#todos').empty();
            getCount(data.length);
            $.each(data, function (key, item) {
                const checked = item.isComplete ? 'checked' : '';
                $('<tr><td><input disabled="true" type="checkbox" ' + checked + '></td>' +
                    '<td>' + item.name + '</td>' +
                    '<td>' + item.dateDue.substring(0, 10) + '</td>' +
                    '<td><button onclick="editItem(' + item.id + ')">Edit</button></td>' +
                    '<td><button onclick="deleteItem(' + item.id + ')">Delete</button></td>' +
                    '</tr>').appendTo($('#todos'));
            });

            todos = data;
        }
    });
}

function addItem() {
    const item = {
        'name': $('#add-name').val(),
        'isComplete': false,
        'dateDue': $('#add-date').val()
    };
    item.name = escapeOutput(item.name);

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
            item.name, $('#add-dateDue').val('');
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
    item.name = escapeOutput(item.name);

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

function escapeOutput(toOutput) {
    return toOutput.replace(/\&/g, '&amp;')
        .replace(/\</g, '&lt;')
        .replace(/\>/g, '&gt;')
        .replace(/\"/g, '&quot;')
        .replace(/\'/g, '&#x27')
        .replace(/\//g, '&#x2F');
}