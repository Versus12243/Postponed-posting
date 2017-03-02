﻿/// <reference path="../Scripts/typings/jquery/jquery.d.ts" />
/// <reference path="../Scripts/typings/bootstrap.v3.datetimepicker/bootstrap.v3.datetimepicker-3.0.0.d.ts"/>

class ManagePosting {

    static PostsDT = null;
    static GroupsOfLinksDT = null;
    static GroupsIds = [];
    static ReplaceLinksInterval = null;
    static TimeToReplaceLinks = 5;
    static TimeToReplaseStartTimer = 0;
    static valid = [];
    static countds = 0;
    static biggestImage = null;
    static biggestSize = 0;

    getAccessToken() {
        //POST / oauth / access_token HTTP/ 1.1
        //Host: api - ssl.bitly.com
        //Content - Type: application / x - www - form - urlencoded

        //client_id = YOUR_CLIENT_ID & client_secret=YOUR_CLIENT_SECRET & code=CODE & redirect_uri=REDIRECT_URI

        //$.ajax({
        //    type: "POST",
        //    url: "https://api-ssl.bitly.com/oauth/access_tokenHTTP/1.1"
        //    data: {
        //        client_id: "",
        //        client_secret: ""
        //    }
        //})

    }

    createNewPost(data): void {
        $.ajax({
            type: 'POST',
            url: '/api/Posting/EditPost',
            data: data
        }).done(function (response) {
            toastr.info('Saved');
            $('#modalCreatePost').modal('hide');
            ManagePosting.PostsDT.ajax.reload();
            $('#PostId').val('0');
            ManagePosting.GroupsIds = [];
        }).fail(function (response) {
            toastr.error(JSON.parse(response.responseText).Message);
            $('#PostId').val('0');
            ManagePosting.GroupsIds = [];
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
            GroupsIds: ManagePosting.GroupsIds,
            SendAfterSaving: $('input[name="date-of-sending-rbtn"]:checked').data('value'),
            SocialNetworkId: $('#SocialNetworkId').val()
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

        if (!data.SendAfterSaving) {
            var dateOfPublish = new Date(data.DateOfPublish);
            var currentDate = new Date();

            if (Common.IsNullOrEmpty(data.DateOfPublish) || dateOfPublish <= currentDate) {
                toastr.error('Date of publish can`t be less then current date');
                return;
            }

            data.DateOfPublish = dateOfPublish.toISOString();

        }

        this.createNewPost(data);
    }

    postCreating(evt) {
        var target = evt.target;
        if ($(target).is('input')) {
            if ($('#post-main-options').hasClass('active')) {
                $($('[data-toggle="tab"]')[1]).click();
                return;
            }
            else if ($('#post-preview').hasClass('active')) {
                $($('[data-toggle="tab"]')[2]).click();
                return;
            }
            else
                this.validatePostData();
        }
        else if ($(target).attr('href') == "#post-main-options" || $(target).attr('href') == "#post-preview") {
            $('#create-post-continue-btn').attr('value', 'Next >');
            $('#create-post-continue-btn').removeClass('disabled');
        }
        else {
            $('#create-post-continue-btn').attr('value', 'Save');
            setTimeout(() => {
                this.initSelectedGroupsTable();
            }, 200);
        }
    }

    initPostsTable() {

        $('div.form-group:has(input[name="date-of-sending-rbtn"])').click(function (evt) {
            var target = $(evt.target);

            if ($(target).is('div')) {
                target = $(target).find('input');
            }
            else if ($(target).is('label')) {
                target = $(target).parent().find('input');
            }

            $(target).prop('checked', true);

            if ($(target).data('value')) {
                $('div.input-group.date').addClass('hidden');
            }
            else {
                $('div.input-group.date').removeClass('hidden');
            }
        })

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
                { "data": "Actions", "searchable": false, "orderable": false, "className": 'text-center'  }
            ],
            "order": [1, "asc"],

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
                        $(form).find('input[name="date-of-sending-rbtn"][data-value="false"]').click();
                        ManagePosting.GroupsIds = [];

                        for (var i = 0; i < response.GroupsIds.length; i++)
                            ManagePosting.GroupsIds.push(response.GroupsIds[i]);

                        $('#modalCreatePost').modal("show");

                    }).fail(function (response) {
                        toastr.error(JSON.parse(response.responseText).Message);
                    });
                });

                $('.deletePost').off().on('click', function (evt) {
                    var target = evt.target;
                    $.ajax({
                        type: 'DELETE',
                        url: '/api/Posting/DeletePost/' + $(target).data('id')
                    }).done(function (response) {
                        toastr.info("Post was removed");
                        ManagePosting.PostsDT.ajax.reload();
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
                        (json.data[i]['Status'] == "Active" ? '<input type="button" data-id="' + json.data[i]["Id"] + '" class="btn btn-default deletePost" value="Delete">' : '');
                }
            }
        });  

        $('#modalCreatePost').on('hidden.bs.modal', function () {
            (<HTMLFormElement>document.getElementById('createPostForm')).reset();
            ManagePosting.GroupsIds = [];
            $($('[data-toggle="tab"]')[0]).click();
            $('#PostId').val('0');
            $('input[name="date-of-sending-rbtn"][data-value="true"]').click();
            if (ManagePosting.ReplaceLinksInterval != null) {
                window.clearInterval(ManagePosting.ReplaceLinksInterval);
                ManagePosting.ReplaceLinksInterval = null;
            }
            ManagePosting.TimeToReplaceLinks = 5;
            ManagePosting.TimeToReplaseStartTimer = 0;

            ManagePosting.valid = [];
            ManagePosting.countds = 0;
            ManagePosting.biggestImage = null;
            ManagePosting.biggestSize = 0;
        });        
    }
        
    initSelectedGroupsTable() {
        if (ManagePosting.GroupsOfLinksDT != null) {
            ManagePosting.GroupsOfLinksDT.destroy()
        }
            ManagePosting.GroupsOfLinksDT = $('#groups-of-links-table-dt').DataTable({
                "scrollY": "225px",
                "serverSide": true,
                "ajax": {
                    "type": "POST",
                    "url": '/api/Posting/SelectetGroupsDataHandler',
                    "contentType": 'application/json; charset=utf-8',
                    'data': function (data) {
                        var postId = $('#PostId').val();
                        data['SocialNetworkId'] = $('#SocialNetworkId').val();
                        data['PostId'] = postId;
                        //data['NeedGroupsIds'] = ManagePosting.GroupsIds.length == 0 && postId != 0;
                        data['GroupsIds'] = ManagePosting.GroupsIds;
                        return data = JSON.stringify(data);
                    }
                },
                "columns": [
                    { "data": "Id", "visible": false, "searchable": false },
                    { "data": "Expand_Collapse", "searchable": false, "orderable": false, "className": 'details-control' },
                    { "data": "Name" },
                    { "data": "Selection", "className": 'text-center', "searchable": false, "orderable": parseInt($('#PostId').val()) > 0 }
                ],
                "order": [2, "asc"],
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
                        ManagePosting.GroupsOfLinksDT.ajax.reload();
                    });
                }
            });

            ManagePosting.GroupsOfLinksDT.on('xhr.dt', function (e, settings, json) {
                if (Common.IsNullOrUndef(ManagePosting.GroupsIds)) {
                    ManagePosting.GroupsIds = [];
                }
                if (!Common.IsNullOrUndef(json) && !Common.IsNullOrUndef(json.data) && json.data.length > 0) {
                    for (var i = 0; i < json.data.length; i++) {
                        json.data[i]['Expand_Collapse'] = '<span data-id="' + json.data[i]["Id"] + '"></span>';
                        json.data[i]['Selection'] = (ManagePosting.GroupsIds.indexOf(json.data[i]['Id']) > -1) ? '<input class="cbx_group" type="checkbox" data-id="' + json.data[i]["Id"] + '" checked />' : '<input class="cbx_group" type="checkbox" data-id="' + json.data[i]["Id"] + '"/>';                      
                    }
                }
            });                  
    }

    startInitTimerToReplaceLinks() {
    if (ManagePosting.ReplaceLinksInterval != null) {
        window.clearInterval(ManagePosting.ReplaceLinksInterval);
        ManagePosting.ReplaceLinksInterval = null;
    }
        ManagePosting.TimeToReplaseStartTimer = 0;
        ManagePosting.ReplaceLinksInterval = window.setInterval(function () {
            ManagePosting.TimeToReplaseStartTimer += 1;
            if (ManagePosting.TimeToReplaseStartTimer == ManagePosting.TimeToReplaceLinks) {
                ManagePosting.replaceLinks();
            }
        }, 100); 
    }

    static getAllWordsFromContent() {
        var content = $('#Content').val();
        var splitByRow = content.split('\n');
        var words = [];

        for (var i = 0; i < splitByRow.length; i++) {
            words = words.concat(splitByRow[i].split(' '));
        }

        words = Array.from(new Set(words));
        return words;
    }

    static replaceLinks() {
      //  API Address: https://api-ssl.bitly.com
      //  GET / v3 / shorten ? access_token = ACCESS_TOKEN & longUrl=http % 3A% 2F% 2Fgoogle.com % 2F

        var words = ManagePosting.getAllWordsFromContent();
        var tempLinks = [];
        var defaultImageExist = ManagePosting.biggestImage != null;
        var t = 0;

        for (var i = 0; i < words.length; i++) {
            var re = /(https?:\/\/)?([\da-z\.-]+)\.([a-z\.]{2,6})([\/\w \.-]*)*\/?/i;
            if (re.test(words[i]) && words[i].indexOf("bit.ly") < 0) {
                if (!defaultImageExist && t == 0) {
                    ManagePosting.replaceLink(words[i], true)
                }
                ManagePosting.replaceLink(words[i], false);   
                t++;             
            }
        }

        if (t == 0) {
            ManagePosting.valid = [];
            ManagePosting.countds = 0;
            ManagePosting.biggestImage = null;
            ManagePosting.biggestSize = 0;
        }
    }

    static replaceLink(link, isSharing) {
        $.ajax({
            type: "GET",
            url: "/api/Posting/ShortedLink",
            data: { url: encodeURIComponent(link) }
        }).done(function (response) {
            var text = $('#Content').val();
            var newUrl = response.data.url;
            if (parseInt(response.status_code) == 200) {
                $('#Content').val(text.replace(link, newUrl));
                if (isSharing) {
                    ManagePosting.requestDebugInfoAboutPost(newUrl);
                }
            }         
            }).fail(function (response) {
                console.log(response);
            });
    }

    static debugInfoForLink = null;
    
    static requestDebugInfoAboutPost(id) {
        
        //add method for receiving access token
        $.ajax({
            type: "GET",
            url: "/api/Posting/GetDebugInfoAboutSharedLink",
            data: { url: encodeURIComponent(id) }
        }).done(function (response) {    

            ManagePosting.debugInfoForLink = response;

            if (typeof response.image != 'undefined') {
                for (var i = 0; i < response.image.length; i++) {
                    var image = new Image();

                    ManagePosting.evL(image, i, response.image.length);

                    image.src = response.image[i].url;
                }
            }
        });
    }
    
    static evL(image, id, l) {
        image.addEventListener("load", function (e) {
            if (image.width >= 200 && image.height >= 200 && image.width <= 1600 && image.height <= 1600) {
                ManagePosting.valid.push(id);
                var size = image.height * image.width;
                if (size > ManagePosting.biggestSize) {
                    ManagePosting.biggestSize = size;
                    ManagePosting.biggestImage = image;
                }
            }
            ManagePosting.countds++;
            if (ManagePosting.countds == l) {

                var preview = '<div class="panel panel-default"><div class="panel-heading">Text of post</div><div class="panel-body">';
                
                var words = ManagePosting.getAllWordsFromContent();

                for (var i = 0; i < words.length; i++) {
                    var re = /(https?:\/\/)?([\da-z\.-]+)\.([a-z\.]{2,6})([\/\w \.-]*)*\/?/i;
                    if (re.test(words[i])) {
                        preview += '<a href="' + words[i] + '">' + words[i] + '</a>'
                    }
                    else {
                        preview += '<span>' + words[i] + ' ' + '</span>';
                    } 
                }

                preview += '</div></div><div class="panel panel-default"><div class="panel-heading">Attachments preview</div><div class="panel-body">';

                var small = false;

                if (ManagePosting.biggestImage != null) {
                    var r = ManagePosting.biggestImage.width / ManagePosting.biggestImage.height;
                    if (r < 1.5 && r != 1) {
                        preview += '<div class="facebook-img" style="background: url(\'' + ManagePosting.biggestImage.src.trim() + '\'); margin: 0 auto;"></div>';
                    }
                    else {
                        var height = ManagePosting.biggestImage.height;
                        var width = ManagePosting.biggestImage.width;
                        if (Math.abs(height - 158) < Math.abs(width - 158)) {
                            var p = 15800 / height;
                            var nW = width * p / 100;
                            var bpx = (nW - 158) / 2;
                            preview += '<div class="facebook-small-img" style="background: url(\'' + ManagePosting.biggestImage.src.trim() + '\'); background-size: ' + r * 100 + '% !important; background-position-x: -' + bpx + 'px; display: inline-block"></div>';
                        }
                        else {
                            var p = 15800 / width;
                            var nH = height * p / 100;
                            var bpy = (nH - 158) / 2;
                            preview += '<div class="facebook-small-img" style="background: url(\'' + ManagePosting.biggestImage.src.trim() + '\'); background-size: ' + r * 100 + '% !important; background-position-y: -' + bpy + 'px; display: inline-block"></div>';
                        }
                        var small = true;
                    }
                }
                preview += '<div style="display: inline-block; vertical-align: top;"><div style="' + (small ? "height: 146px" : "") + '">';
                preview += '<div class="f-s-18 m-b-10">' + ManagePosting.debugInfoForLink.title + '</div>';
                preview += '<div class="f-s-12 m-b-10" style="line-height: 16px;">' + ManagePosting.debugInfoForLink.description + '</div>';
                var url = ManagePosting.debugInfoForLink.url.toLowerCase();
                url = (url.indexOf('www.') >= 0 ? url.substring(url.indexOf('www.') + 4).toUpperCase() : (url.indexOf('//') >= 0 ? url.substring(url.indexOf('//') + 2).toUpperCase() : url.toUpperCase()));
                if (url[url.length - 1] == '/') {
                   url = url.slice(0, url.length - 1);
                }
                preview += '</div><div class="f-s-11" style="color: #90949c;">' + url + '</div></div>';
                preview += '</div>';
                $('#preview-container').html(preview);
            }
        });
    }

}

window.onload = () => {
    var managePosting: ManagePosting = new ManagePosting();
    $(".date, #DateOfPublish").datetimepicker();
    $('#create-post-continue-btn, #createPostForm .nav li').click((evt) => { managePosting.postCreating(evt); });
    managePosting.initPostsTable();
    $('textarea#Content').keyup(() => { managePosting.startInitTimerToReplaceLinks() });
};