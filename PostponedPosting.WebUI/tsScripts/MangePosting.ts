/// <reference path="../Scripts/typings/jquery/jquery.d.ts" />
/// <reference path="../Scripts/typings/bootstrap.v3.datetimepicker/bootstrap.v3.datetimepicker-3.0.0.d.ts"/>

class ManagePosting {

    static PostsDT = null;
    static GroupsOfLinksDT = null;
    static GroupsIds = [];

    createNewPost(data): void {
        $.ajax({
            type: 'POST',
            url: '/api/Posting/EditPost',
            data: data
        }).done(function (response) {
            toastr.info('Saved');
            $('#modalCreatePost').modal('hide');
            ManagePosting.PostsDT.ajax.reload();
        }).fail(function (response) {
            toastr.error(JSON.parse(response.responseText).Message);
            });
    }

    validatePostData() {

        var form = $('#createPostForm');
        var data = {
            Id: $(form).find('#PostId').val(),
            Name: $(form).find('#Name').val(),
            DateOfCreation: null,
            Content: $(form).find('#Content').val(),
            DateOfPublish: $(form).find('#DateOfPublish').val(),
            StatusOfSending: null,
            Status: null,
            GroupsIds: ManagePosting.GroupsIds
             //$('#groups-of-links-table-dt tr:has(input:checked) span').map(function (index, item) { return $(item).data('id') })            
        };

        if (Common.IsNullOrEmpty(data.Name)) {
            toastr.error('Name field can`t be empty');
            return;
        }

        if (Common.IsNullOrEmpty(data.Content)) {
            toastr.error('Content field can`t be empty');
            return;
        }

        if (data.GroupsIds.length == 0) {
            toastr.error('Select at least one from groups of links');
            return;
        }

        var dateOfPublish = new Date(data.DateOfPublish);
        var currentDate = new Date();

        if (Common.IsNullOrEmpty(data.DateOfPublish) || dateOfPublish <= currentDate) {
            toastr.error('Date of publish can`t be less then current date');
            return;
        }

        this.createNewPost(data);
    }

    postCreating(evt) {
        var target = evt.target;
        if ($(target).is('input')) {
            if ($('#post-main-options').hasClass('active')) {
                $($('[data-toggle="tab"]')[1]).click();
                $('#create-post-continue-btn').attr('value', 'Save');
                //$('#create-post-continue-btn').addClass('disabled');
                setTimeout(() => {
                    this.initSelectedGroupsTable();
                }, 200);
            }
            else
                this.validatePostData();
        }
        else if ($(target).attr('href') == "#post-main-options") {
            $('#create-post-continue-btn').attr('value', 'Next >');
            $('#create-post-continue-btn').removeClass('disabled');
        }
        else {
            $('#create-post-continue-btn').attr('value', 'Save');
            //$('#create-post-continue-btn').addClass('disabled');
            setTimeout(() => {
                this.initSelectedGroupsTable();
            }, 200);
        }
    }

    initPostsTable() {
        ManagePosting.PostsDT = $('#posts-dt').DataTable({
            "serverSide": true,
            "ajax": {
                "type": "POST",
                "url": '/api/Posting/PostsDataHandler',
                "contentType": 'application/json; charset=utf-8',
                'data': function (data) {
                    data['SocialNetworkId'] = $('#SocialNetworkId').val();
                    return data = JSON.stringify(data);
                }
            },
            "columns": [
                { "data": "Id", "visible": false, "searchable": false },
                { "data": "Name" },
                { "data": "DateOfCreation", "className": 'text-center' },
                { "data": "DateOfPublish", "className": 'text-center' },
                { "data": "StatusOfSending", "className": 'text-center' },
                { "data": "Status", "className": 'text-center' },
                { "data": "Actions" }
            ],
            "order": [0, "asc"],

            "drawCallback": function (settings) {
                $('.editPost').off().on('click', function (evt) {
                    var target = evt.target;
                    $.ajax({
                        type: 'POST',
                        url: '/api/Posting/GetPostData/' + $(target).data('id')
                    }).done(function (response) {
                        var form = $('#createPostForm');
                        $(form).find('#PostId').val(response.Id);
                        $(form).find('#Name').val(response.Name);
                        $(form).find('#Content').val(response.Content);
                        $(form).find('#DateOfPublish').val(LocalDateCreator.getLocalDate(response.DateOfPublish));

                        ManagePosting.GroupsIds = [];

                        for (var i = 0; i < response.GroupsIds.length; i++)
                            ManagePosting.GroupsIds.push(response.GroupsIds[i]);

                        $('#modalCreatePost').modal("show");

                    }).fail(function (response) {
                        toastr.error(JSON.parse(response.responseText).Message);
                    });
                });

                $('.switchSendingStatus').off().on('click', function (evt) {
                    var target = evt.target;
                    $.ajax({
                        type: 'POST',
                        url: '/api/Posting/SwitchSendingStatus/' + $(target).data('id')
                    }).done(function (response) {
                        if (response > 0)
                            ManagePosting.PostsDT.ajax.reload();
                    }).fail(function (response) {
                        toastr.error(JSON.parse(response.responseText).Message);
                    });
                });
            }
        });
        
        ManagePosting.PostsDT.on('xhr.dt', function (e, settings, json) {
            if (!Common.IsNullOrUndef(json) && !Common.IsNullOrUndef(json.data) && json.data.length > 0) {
                for (var i = 0; i < json.data.length; i++) {
                    var statusOfSending = json.data[i]["StatusOfSending"];
                    json.data[i]['DateOfCreation'] = LocalDateCreator.getLocalDate(json.data[i]['DateOfCreation']);
                    json.data[i]['DateOfPublish'] = LocalDateCreator.getLocalDate(json.data[i]['DateOfPublish']);
                    json.data[i]['Actions'] = '<input type="button" class="btn btn-default editPost" data-id="' + json.data[i]["Id"] + '" value="Edit"/>' +
                        (statusOfSending != "Ready" ? '<input type="button" data-id="' + json.data[i]["Id"] + '" class="btn btn-default switchSendingStatus" value="' + (statusOfSending != "Suspended" ? "Suspend" : "Run") + '">' : '') + 
                        '<input type="button" data-id="' + json.data[i]["Id"] + '" class="btn btn-default sendNow" value="Send now" />' +
                        '<input type="button" data-id="' + json.data[i]["Id"] + '" class="btn btn-default delete" value="Delete">';
                }
            }
        });  

        $('#modalCreatePost').on('hidden.bs.modal', function () {
            (<HTMLFormElement>document.getElementById('createPostForm')).reset();
            ManagePosting.GroupsIds = [];
        });        
    }
        
    initSelectedGroupsTable() {
        if (ManagePosting.GroupsOfLinksDT != null) {
            ManagePosting.GroupsOfLinksDT.ajax.reload();
        }
        else {
            ManagePosting.GroupsOfLinksDT = $('#groups-of-links-table-dt').DataTable({
                "scrollY": "225px",
                "serverSide": true,
                "ajax": {
                    "type": "POST",
                    "url": '/api/Posting/SelectetGroupsDataHandler',
                    "contentType": 'application/json; charset=utf-8',
                    'data': function (data) {
                        data['SocialNetworkId'] = $('#SocialNetworkId').val();
                        data['PostId'] = $('#PostId').val();
                        return data = JSON.stringify(data);
                    }
                },
                "columns": [
                    { "data": "Id", "visible": false, "searchable": false },
                    { "data": "Expand_Collapse", "searchable": false, "orderable": false, "className": 'details-control' },
                    { "data": "Name" },
                    { "data": "Selection", "className": 'text-center' }
                ],
                "order": [0, "asc"],
                "drawCallback": function (settings) {
                    $('td.details-control').off().on('click', function (evt) {
                        var tr = $(this).closest('tr');
                        var row = ManagePosting.GroupsOfLinksDT.row(tr);

                        if (row.child.isShown()) {
                            // This row is already open - close it
                            row.child.hide();
                            tr.removeClass('shown');
                        }
                        else {
                            var links = '<div><lable class="bold m-b-10">List of links:</lable></div><div><table class="table table-bordered table-hover">' +
                                '<thead>' +
                                '<tr>' +
                                '<th>' +
                                'Name' +
                                '</th>' +
                                '<th>' +
                                'Url' +
                                '</th>' +
                                '</tr>' +
                                '</thead>' +
                                '<tbody>';

                            $.ajax({
                                type: 'POST',
                                url: '/api/GroupsOfLinks/GetAllLinksOfGroup/' + tr.find("span").data("id")
                            }).done(function (response) {
                                for (var i = 0; i < response.length; i++) {
                                    links += '<tr><td>' + response[i]['Name'] + '</td><td>' + response[i]['Url'] + '</td></tr>';
                                }
                                links += '</tbody>' +
                                    '</table></div>';
                                var row = ManagePosting.GroupsOfLinksDT.row(tr);
                                row.child(links).show();
                                tr.addClass('shown');
                            }).fail(function (response) {

                            }) 
                        }
                    });

                    $('.cbx_group').off().on('click', function (evt) {
                        var target = evt.target;
                        var id = $(target).data('id');
                        var index = ManagePosting.GroupsIds.indexOf(id);
                        if ($(target).is(':checked')) { 
                            if (index < 0) {
                                ManagePosting.GroupsIds.push(id);
                            }                                             
                        }
                        else {
                            if (index > -1) {
                                ManagePosting.GroupsIds.splice(index, 1);
                            }
                        }
                    });
                }
            });

            ManagePosting.GroupsOfLinksDT.on('xhr.dt', function (e, settings, json) {
                if (!Common.IsNullOrUndef(json) && !Common.IsNullOrUndef(json.data) && json.data.length > 0) {
                    for (var i = 0; i < json.data.length; i++) {
                        json.data[i]['Expand_Collapse'] = '<span data-id="' + json.data[i]["Id"] + '"></span>';
                        json.data[i]['Selection'] = (json.data[i]['Selection'] || ManagePosting.GroupsIds.indexOf(json.data[i]['Id']) > -1) ? '<input class="cbx_group" type="checkbox" data-id="' + json.data[i]["Id"] + '" checked />' : '<input class="cbx_group" type="checkbox" data-id="' + json.data[i]["Id"] + '"/>';
                    }
                }
            });            
        }
    }
}

window.onload = () => {
    var managePosting: ManagePosting = new ManagePosting();
    $(".date").datetimepicker();
    $('#create-post-continue-btn, #createPostForm .nav li').click((evt) => { managePosting.postCreating(evt); });
    managePosting.initPostsTable();

};