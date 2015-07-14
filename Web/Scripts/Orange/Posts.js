$(document).ready(function () {
    $('#btnSubmitComment').click(function () {
        SubmitComment();
    });
    $('#btnApproveAll').click(function () {
        ApproveAll();
    });
    $('#btnApproveSelected').click(function () {
        ApproveSelected();
    });
    $('#btnDiscardSelected').click(function () {
        DiscardSelected();
    });
});

function SubmitComment() {
    var username = $('#txbxUsername').val();
    var comment = $('#txtareaComment').val();
    var postId = $('#hdnPostId').val();
    var derping = GetBasePath() + 'SubmitComment';
    $.ajax({
        type: "POST",
        async: false,
        url: derping,
        dataType: "html",
        data: JSON.stringify({ postId: postId, username: username, comment: comment }),
        contentType: "application/json",
        beforeSend: function () {
            //$.blockUI({ message: 'Hauling up the treasure!..', css: { backgroundColor: '#6F1623', color: '#FFF' } });
        },
        complete: function () {
            //$.unblockUI();
        },
        success: function (data) {
            $('#notification').html(data);
            GetPendingComments();
            GetApprovedComments();
            ClearCommentForm();
        },
        error: function (e) {
            //alert("We've scrapped the reef!");
        }
    });
}

function GetApprovedComments() {
    var postId = $('#hdnPostId').val();
    $.ajax({
        type: "POST",
        async: false,
        url: GetBasePath() + 'GetApprovedComments',
        dataType: "html",
        data: JSON.stringify({ postId: postId }),
        contentType: "application/json",
        beforeSend: function () {
            //$.blockUI({ message: 'Hauling up the treasure!..', css: { backgroundColor: '#6F1623', color: '#FFF' } });
        },
        complete: function () {
            //$.unblockUI();
        },
        success: function (data) {
            $('#approvedComments').html(data);
            // get comments (maybe the admin commented)
            //ClearCommentForm();
        },
        error: function (e) {
            //alert("We've scrapped the reef!");
        }
    });
}

function GetPendingComments() {
    var postId = $('#hdnPostId').val();
    $.ajax({
        type: "POST",
        async: false,
        url: GetBasePath() + 'GetPendingComments',
        dataType: "html",
        data: JSON.stringify({ postId: postId }),
        contentType: "application/json",
        beforeSend: function () {
            //$.blockUI({ message: 'Hauling up the treasure!..', css: { backgroundColor: '#6F1623', color: '#FFF' } });
        },
        complete: function () {
            //$.unblockUI();
        },
        success: function (data) {
            $('#pendingComments').html(data);
            // get comments (maybe the admin commented)
            //ClearCommentForm();
        },
        error: function (e) {
            //alert("We've scrapped the reef!");
        }
    });
}

function ApproveAll() {
    // TODO: get all elements by LIKE
    var comments = document.getElementsByClassName('ckbxApproval');
    var ckbxList = [];
}

function ApproveSelected() {
    
    
}

function DiscardSelected() {

}

function ClearCommentForm() {
    $('#txtareaComment').val('');
}

/* spin off into its own file? */
function SavePost() {
    var title = $('#txbxTitle').val();
    var effectiveDate = $('#txbxEffectiveDate').val();
    var publiclyVisible = $('#ckbxPublic').is(':checked');
    var body = $('#txtAreaBody').val();
    var url = '/Posts/SavePost'
    $.ajax({
        type: "POST",
        async: false,
        url: url,
        dataType: "html",
        data: JSON.stringify({ title: title, effectiveDate: effectiveDate, publiclyVisible: publiclyVisible, body: body }),
        contentType: "application/json",
        beforeSend: function () {
            //$.blockUI({ message: 'Hauling up the treasure!..', css: { backgroundColor: '#6F1623', color: '#FFF' } });
        },
        complete: function () {
            //$.unblockUI();
        },
        success: function (data) {
            $('#notification').html(data);
            //GetPendingComments();
            //GetApprovedComments();
            //ClearCommentForm();
        },
        error: function (e) {
            //alert("We've scrapped the reef!");
        }
    });
}