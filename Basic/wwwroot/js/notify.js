"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/notify").build();
connection.start();
connection.on("ReceiveMsg", function (msg){
    var li = document.createElement("li");
    li.textContent = msg;
    document.getElementById("msglist").appendChild(li)
})
