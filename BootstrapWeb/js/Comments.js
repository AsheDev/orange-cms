// TODO
function ApproveComment(elementId) {
    var commentId = GetTrailingId(elementId);
    $('#alertRow-' + commentId).removeClass("hidden");
    
    // call controller action to approve comment

    // disable the deny comment button
    document.getElementById("deny-" + commentId).disabled = true;

    // unhide the reply button
}

// TODO
function DenyComment(elementId) {
    var commentId = GetTrailingId(elementId);
    $('#alertRow-' + commentId).removeClass("hidden");

    // call controller action to deny comment

    // disable the approve comment button
    document.getElementById("approve-" + commentId).disabled = true;
}


// TODO
function ToggleReply(elementId) {
    var commentId = elementId.substr(elementId.indexOf("-") + 1);
    // TODO: rename the btn that's using this as I don't like just passing in the Id
    if ($('#replyRow-' + commentId).hasClass("hidden")) {
        $('#replyRow-' + commentId).removeClass("hidden");
    } else {
        $('#replyRow-' + commentId).addClass("hidden");
    }
}

// TODO
function ShowReplies(elementId) {
    var commentId = elementId.substr(elementId.indexOf("-") + 1);
    var targetElement = "comments-" + commentId;

    var gradients = document.getElementsByClassName("gradient");
    if (gradients.length > 0) {
        var horizontalId = "horizontal-" + GetTrailingId(gradients[0].id);
        $("#" + horizontalId).removeClass("gradient");
    }

    // check if the replies have already been loaded
    if ($('#' + targetElement).children().length > 0) {
        if ($('#' + targetElement).hasClass("hidden")) {
            $('#' + targetElement).removeClass("hidden");
            ToggleReplyText(elementId, false);
            ToggleGradient(elementId, true);

        } else {
            $('#' + targetElement).addClass("hidden");
            ToggleReplyText(elementId, true);
            ToggleGradient(elementId, false);
        }
    } else {
        // if they're already visible hide them?
        var baseUrl = GetBasePath() + 'GetReplies';
        $.ajax({
            type: "POST",
            async: false,
            url: baseUrl,
            dataType: "html",
            data: JSON.stringify({ commentId: commentId }),
            contentType: "application/json",
            beforeSend: function () {
                //$.blockUI({ message: 'derp, derp, derp!', css: { backgroundColor: '#6F1623', color: '#FFF' } });
            },
            complete: function () {
                //$.unblockUI();
            },
            success: function (data) {
                // get the new


                $('#' + targetElement).html(data); // insert the new data into the page

                ToggleGradient(elementId, true);
                SetBackgroundColour(elementId, commentId);
                ToggleReplyText(elementId, false);
            },
            error: function (e) {
                //alert("Something broke!");
            }
        });
    }
}

// TODO
function SetBackgroundColour(elementId, commentId) {
    var ancestorList = $("#" + elementId).parentsUntil($("div[id^='color-']"));
    var parentCommentId = GetTrailingId(ancestorList[5].id);
    // this index 5 is a little too hard coded for my liking...
    var newBackgroundColor = GetNextColour('#comments-' + parentCommentId);
    $("#comments-" + commentId).css("background-color", newBackgroundColor);
    //var value = $('#' + elementId).val();
}

// TODO
function ToggleReplyText(elementId, showing) {
    if (showing == true) {
        $('#' + elementId).val($('#' + elementId).val().replace("Showing", "Show"));
    } else {
        $('#' + elementId).val($('#' + elementId).val().replace("Show", "Showing"));
    }
}

// TODO
function ToggleGradient(elementId, add) {
    if (add == true) {
        var containingDivList = $("#" + elementId).parentsUntil($("div[class^='individual-comment']"));
        $("#horizontal-" + GetTrailingId(containingDivList[2].id)).addClass("gradient");
    } else {
        var containingDivList = $("#" + elementId).parentsUntil($("div[class^='container']"));
        $("#horizontal-" + GetTrailingId(containingDivList[2].id)).removeClass("gradient");
    }
}

// TODO CORE
function GetNextColour(elementId) {
    var colour = $(elementId).css('background-color');
    colour = (colour == 'transparent') ? rgb2hex("rgb(255, 255, 255)") : rgb2hex(colour);
    var nextColour = "";
    switch (colour.toUpperCase()) {
        case "#FFFFFF":
            nextColour = "#F5F5F5";
            break;
        case "#F5F5F5":
            nextColour = "#EBEBEB";
            break;
        case "#EBEBEB":
            nextColour = "#E0E0E0";
            break;
        case "#E0E0E0":
            nextColour = "#D6D6D6";
            break;
        case "#D6D6D6":
            nextColour = "#CCCCCC";
            break;
        case "#CCCCCC":
            nextColour = "#C2C2C2";
            break;
        case "#C2C2C2":
            nextColour = "#B8B8B8";
            break;
        case "#B8B8B8":
            nextColour = "#ADADAD";
            break;
        case "#ADADAD":
            nextColour = "#A3A3A3";
            break;
        case "#A3A3A3":
            nextColour = "#999999";
            break;
        case "#999999":
            nextColour = "#8F8F8F";
            break;
        default:
            nextColour = "#858585";
            break;
            // http://www.colorhexa.com/ffffff
            // use "tint color variations"
    }
    return nextColour;
}

//Function to convert hex format to a rgb color
function rgb2hex(rgb) {
    rgb = rgb.match(/^rgb\((\d+),\s*(\d+),\s*(\d+)\)$/);
    var hexDigits = new Array
    ("0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "a", "b", "c", "d", "e", "f");
    return "#" + hex(rgb[1], hexDigits) + hex(rgb[2], hexDigits) + hex(rgb[3], hexDigits);
}

function hex(x, hexDigits) {
    return isNaN(x) ? "00" : hexDigits[(x - x % 16) / 16] + hexDigits[x % 16];
}