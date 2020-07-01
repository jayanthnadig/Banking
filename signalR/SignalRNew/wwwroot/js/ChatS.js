"use strict";

//Disable send button until connection is established
document.getElementById("sendButton").disabled = true;

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

console.log('Server ');

connection.start()
    .then(function () {
        console.log('start 1');
        document.getElementById("sendButton").disabled = false;
        console.log('start 2');
    })
    .catch(function (err) {
        return console.error(err.toString());
    });

// send to server
document.getElementById("sendButton").addEventListener("click", function (event) {
    console.log('sendButton 1');

    var message = [
        {
            id: 1,
            message: "one",
            read: false
        },
        {
            id: 2,
            message: "two",
            read: false
        }
    ];
    connection
        .invoke("SendMessage", "server user", JSON.stringify(message))
        .catch(function (err) {
            return console.error(err.toString());
        });
    event.preventDefault();
    console.log('sendButton end');
});


// recieve from client
connection.on("ReceiveMessage", function (user, message) {
    console.log('ReceiveMessage 1');
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var encodedMsg = user + " says " + msg;
    var li = document.createElement("li");
    li.textContent = encodedMsg;
    document.getElementById("messagesList").appendChild(li);
    console.log('ReceiveMessage end');
});
