/// <reference path="../Scripts/typings/jquery/jquery.d.ts" />
/// <reference path="../Scripts/typings/jquery.dataTables/jquery.dataTables.d.ts" />
/// <reference path="../Scripts/typings/bootstrap/index.d.ts" />
/// <reference path="../Scripts/typings/toastr/toastr.d.ts" />

class ManageLinks {

    initDataTable() {
      var dt = $('#list-of-links-datatable').DataTable({
            "serverSide": true,
            "ajax": {
                "type": "POST",
                "url": '/api/Links/DataHandler',
                "contentType": 'application/json; charset=utf-8',
                'data': function (data) {
                    data['GroupId'] = $('#GroupId').val();
                    data['ShowAll'] = $('#showAllLinks').is(':checked'); 
                    return data = JSON.stringify(data);
                }
            },
            "columns": [
                { "data": "Id", "visible": false, "searchable": false },
                { "data": "GroupId", "visible": false, "searchable": false },
                { "data": "Name" },
                { "data": "Url" },
                { "data": "DateOfCreation" },
                { "data": "Actions", "searchable": false, "orderable": false }
            ],
            "order": [0, "asc"],

            "drawCallback": function (settings) {
                $('.switch-presence').off().on('click', function (evt) {
                    var target = evt.target;
                    $.ajax({
                        type: 'POST',
                        url: '/api/Links/SwitchLinkPresenceInGroup/' + $(target).data('id') + '/' + $('#GroupId').val()                
                    }).done(function (response) {
                        dt.ajax.reload();
                    }).fail(function (response) {
                        toastr.error(JSON.parse(response.responseText).Message);
                    })
                });
            }
        });

      dt.on('xhr.dt', function (e, settings, json) {
          if (!Common.IsNullOrUndef(json) && !Common.IsNullOrUndef(json.data) && json.data.length > 0) {
              for (var i = 0; i < json.data.length; i++) {
                  json.data[i]['DateOfCreation'] = LocalDateCreator.getLocalDate(json.data[i]['DateOfCreation']);
                  var id = json.data[i]['Id'];
                  json.data[i]['Actions'] = '<input type="button" class="btn btn-default edit-link" data-id="' + id + '" id="editLink" value="Edit link" />' +
                      '<input type="button" class="btn btn-default switch-presence" data-id="' + id + '" id="switch-presence" value="' + (parseInt(json.data[i]['GroupId']) == $('#GroupId').val() ? "Exclude from group" : "Include to group") + '"/>';
              }      
          }
      });      

      return dt;
    }

    editLink(dt) {
        $.ajax({
            type: 'POST',
            url: '/api/Links/EditLink/',
            data: {
                Id: $('#modal-edit-link #Id').val(),
                Name: $('#name').val(),
                Url: $('#url').val(),
                GroupId: $('#GroupId').val(),
                DateOfCreate: null
            }
        }).done(function (response) {
            dt.ajax.reload();
            $('#modal-edit-link').modal('hide');
            $('#modal-edit-link #name').val('');
            $('#modal-edit-link #url').val('');
        }).fail(function (response) {
            toastr.error(JSON.parse(response.responseText).Message);
        })
    }
    
}

window.onload = () => {
    var manageLinks = new ManageLinks();
    var dt = manageLinks.initDataTable();
    $('#add-link').click(() => { $('#modal-edit-link').modal('show'); $('#modal-edit-link #Id').val('0');});
    $('#add-link-btn').click(() => { manageLinks.editLink(dt); });
    $('#showAllLinks').click(() => { dt.ajax.reload(); });    
    $('#editLink').click(() => { });
    $('#modal-edit-link').on('hidden.bs.modal', function () {
        $('#modal-edit-link #Id').val('');
    });
}