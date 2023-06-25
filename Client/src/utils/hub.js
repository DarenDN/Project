import * as signalR from "@microsoft/signalr";

const events = [];
const connection = new signalR.HubConnectionBuilder()
  .withUrl("http://localhost:5102/api/meetinghub")
  .build();

export const hub = {
  start() {
    if (connection.state !== signalR.HubConnectionState.Connected)
      connection.start();
  },
  on(methodName, newMethod) {
    if (!events.includes(methodName)) {
      events.push(methodName);
      connection.on(methodName, (data) => {
        newMethod(data);
      });
    }
  },
  stop() {
    connection.stop();
  },
  invoke(methodName, data) {
    connection
      .invoke(methodName, data)
      .catch((err) => console.error(err.toString()));
  },
};
