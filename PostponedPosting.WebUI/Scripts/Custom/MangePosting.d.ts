/// <reference path="../typings/jquery/jquery.d.ts" />
/// <reference path="../typings/bootstrap.v3.datetimepicker/bootstrap.v3.datetimepicker-3.0.0.d.ts" />
declare class ManagePosting {
    static PostsDT: any;
    static GroupsOfLinksDT: any;
    static GroupsIds: any[];
    createNewPost(data: any): void;
    validatePostData(): void;
    postCreating(evt: any): void;
    initPostsTable(): void;
    initSelectedGroupsTable(): void;
}
