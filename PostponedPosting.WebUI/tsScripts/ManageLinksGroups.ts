/// <reference path="../Scripts/typings/jquery/jquery.d.ts" />
/// <reference path="../Scripts/typings/jquery.dataTables/jquery.dataTables.d.ts" />
/// <reference path="../Scripts/typings/bootstrap/index.d.ts" />
/// <reference path="../Scripts/typings/toastr/toastr.d.ts" />

class ManageGroupsOfLinks {

    table: any = null;

    initLocalDates() {
        var dates = $("#groupsOfLinksTable tr > td:nth-child(2)");
        for (var i = 0; i < dates.length; i++) {
            $($(dates)[i]).html(LocalDateCreator.getLocalDate($($(dates)[i]).html()));
        }
    }

    editGroup() {
        var temp = location.href.split('/');
        var snId = temp[temp.length - 1].trim();
        $.ajax({
            type: 'POST',
            url: '/api/GroupsOfLinks/EditGroupOfLinks/' + snId,
            data: {
                Id: $('#modal-edit-group #Id').val(),
                Name: $('#name').val()
            }
        }).done(function (response) {
            location.reload();
            toastr.info(response);
        }).fail(function (response) {
                toastr.error(JSON.parse(response.responseText).Message);
            })
    }

}

window.onload = () => {
    $('#create-group').click(function (evt) {
        $('#modal-edit-group .modal-title').html('Create group');
        $('#modal-edit-group #Id').val(0);
        $('#modal-edit-group').modal('show');
    });
    
    var manageGroupsOfLinks = new ManageGroupsOfLinks();
    manageGroupsOfLinks.initLocalDates();
    manageGroupsOfLinks.table = $("#pagesGroupsTable").DataTable();
    
    $('.edit-group-of-links').click(function (evt) {
        $('#modal-edit-group .modal-title').html('Edit group');
        $('#modal-edit-group #Id').val($(this).data("id"));
        $('#modal-edit-group #name').val($(this).closest('tr').find('td').html());
        $('#modal-edit-group').modal('show');
    });

    $('.edit-links-from-group-of-links').click((evt) => { location.href = "/Links/Index/" + $(evt.target).data('id'); })
    $('#edit-group-form').submit((evt) => { evt.preventDefault(); manageGroupsOfLinks.editGroup(); })
    $('#edit-group-btn').click(() => { manageGroupsOfLinks.editGroup(); });


};