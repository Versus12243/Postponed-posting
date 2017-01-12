/// <reference path="../Scripts/typings/jquery/jquery.d.ts" />
/// <reference path="../Scripts/typings/bootstrap.v3.datetimepicker/bootstrap.v3.datetimepicker-3.0.0.d.ts"/>

class ManagePosting {

    createNewPost(): void {
        $.ajax({
            type: 'PUT',
            url: '/api/Posting',
            data: $('#createPostForm').serialize()
        }).done(function (response) {
            console.log(response);
        })
    }
}

window.onload = () => {
    var managePosting: ManagePosting = new ManagePosting();
    $(".date").datetimepicker();
    $('#createPostBtn').click(() => { managePosting.createNewPost(); });
};