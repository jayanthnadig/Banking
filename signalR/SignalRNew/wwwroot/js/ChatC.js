"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

console.log('Client ');

connection.start()
    .then(function () {
        console.log('start 1');
        console.log('start 2');
    })
    .catch(function (err) {
        return console.error(err.toString());
    });

let g_msgs;
// recieve from server
connection.on("ReceiveMessage", function (user, msgs) {
    console.log('ReceiveMessage 1');
    //     var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    g_msgs = JSON.parse(msgs);

    document.getElementById("messagesList").textContent = msgs;
    console.log('ReceiveMessage end');
});

// send to server
document.getElementById("sendButton").addEventListener("click", function (event) {
    console.log('sendButton 1');

    // for (let msg of g_msgs) {        msg.read = true;    }
    g_msgs[0].read = true;

    connection
        .invoke("SendMessage", "client user", JSON.stringify(g_msgs))
        .catch(function (err) {
            return console.error(err.toString());
        });
    event.preventDefault();
    console.log('sendButton end');
});
