/// <reference path="../Scripts/typings/jquery/jquery.d.ts" />
/// <reference path="../Scripts/typings/bootstrap/index.d.ts" />
/// <reference path="../Scripts/typings/toastr/toastr.d.ts" />

class ManageSN {

    showEditCredentialsPopup(id){
        $.ajax({
            type: 'POST',
            url: '/api/Credentials/GetUserCredentials/' + id
        }).done(function (response) {
            if (response != null) {
                $('#login').val(response.Login);
                $('#password').val(response.Password);
            }
            else {
                $('#login').val("");
                $('#password').val("");
            }
            $('#SNId').val(id);
            $('#modalEditCredentials').modal('show');
        }).fail(function (response) {
            toastr.error(JSON.parse(response.responseText).Message);
        });        
    }

    saveCredentials() {
        $.ajax({
            type: 'POST',
            url: '/api/Credentials/SaveCredentials',
            data: {
                SocialNetworkId: $('#SNId').val(),
                Login: $('#login').val(),
                Password: $('#password').val()
            }
        }).done(function (response) {
            if (parseInt(response) > 0)
                toastr.info("Credentials was saved");
            $('#modalEditCredentials').modal('hide');
        })
    }

    managePagesGroups() {

    }

}

window.onload = () => {
    var manageSN: ManageSN = new ManageSN();
    $('.manageCredentialsBtn').click((evt) => { manageSN.showEditCredentialsPopup($(evt.target).closest('tr').attr('id')); });
    $('.manageGroupsOfLinks').click((evt) => { location.href = '/GroupsOfLinks/Index/' + $(evt.target).closest('tr').attr('id') });
    $('#saveCredentialsBtn').click(() => { manageSN.saveCredentials(); });
};