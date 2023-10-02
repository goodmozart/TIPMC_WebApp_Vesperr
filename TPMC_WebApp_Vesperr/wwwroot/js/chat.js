"use strict";

/*$(document).ready(function () {*/

console.log("Base URL:", baseurl);

var connection = new signalR.HubConnectionBuilder().withUrl("chatHub").build(),
        connectionId;

    //Disable send button until connection is established
    $("#sendButton").disabled = true;

    connection.start().then(function () {
        $("#sendButton").disabled = false;
        // Retrieve the username from the anchor element
        var username = $('#getstarted').text().trim();
        console.log("Username:", username);
        connection.invoke('GetConnectionId', username)
            .then(function (Id) {
                connectionId = Id;
            });
    }).catch(function (err) {
        return console.error(err.toString());
    });

    connection.on("RecieveMessage", function (message, time, image, sender) {
        var msg = '<div class="bubble other">'
            + '<span class="message-text">' + message + '</span>'
            + '<span class="message-time">' + time + '</span></div>';

        $('.chat-wrapper.shown .chat').append(msg);
        scrollToBottom();
    });

    var sendMessage = function (recipient) {
        event.preventDefault();

        var text = $("#message-text").val();
        console.log("Text from textarea:", text); // Debug output

        $.ajax({
            type: 'POST',
            url: baseurl + 'Chat/SendMessage',
            data: { to: recipient, text: text },
            cache: false,
            success: function () {
                $(".write textarea").val('');
                $(".chat-wrapper.shown .chat").append('<div class="bubble me"><span class="message-text">' + text + ' </span>'
                 + '<span class="message-time">' + getTimeNow() + '</span></div>');
                scrollToBottom();
            },
            error: function () {
                //alert("Failed to send message!");
                Swal.fire('Failed to send message', '', 'error')
            }
        });
    }


    function getTimeNow() {
        return new Date().toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });
    }

    function scrollToBottom() {
        $(".chat-wrapper.shown .chat").animate({ scrollTop: $('.chat-wrapper.shown .chat').prop("scrollHeight") }, 500);
    }


    //connection.on("RecieveMessage", function (message, time, image, sender) {
    //    var msg = '<div class="bubble other">'
    //        + '<span class="message-text">' + message + '</span>'
    //        + '<span class="message-time">' + time + '</span></div>';

    //    var senderInfo = '<div class="person">'
    //        + '<img class="direct-chat-img src="' + image + '" alt="" />'
    //        + '<span class="name">' + sender + '</span></div>';

    //    var chatMessage = '<div class="chat-message">'
    //        + senderInfo
    //        + msg + '</div>';

    //    $('.chat-wrapper.shown .chat').append(chatMessage);
    //    scrollToBottom();
    //});

    //var sendMessage = function (recipient, image) {
    //    event.preventDefault();

    //    var text = $("#message-text").val();
    //    console.log("Text from textarea:", text); // Debug output

    //    $.ajax({
    //        type: 'POST',
    //        url: baseurl + 'Chat/SendMessage',
    //        data: { to: recipient, text: text },
    //        cache: false,
    //        success: function () {
    //            $(".write textarea").val('');

    //            var senderImage = "~/images/user.png"; // Replace with actual image path

    //            var senderInfo = '<div class="person">'
    //                + '<img class="direct-chat-img src="' + senderImage + '" alt="" /></div>';

    //            var msg = '<div class="bubble me">'
    //                + '<span class="message-text">' + text + ' </span>'
    //                + '<span class="message-time">' + getTimeNow() + '</span></div>';

    //            var chatMessage = '<div class="chat-message">'
    //                + senderInfo
    //                + msg + '</div>';

    //            $(".chat-wrapper.shown .chat").append(chatMessage);
    //            scrollToBottom();
    //        },
    //        error: function () {
    //            alert("Failed to send message!");
    //        }
    //    });
    //}


/*});*/



