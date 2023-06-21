import * as signalR from "@microsoft/signalr";

const connection = new signalR.HubConnectionBuilder()
  .withUrl("http://localhost:5102/api/meetinghub")
  .build();

export const hub = {
  start() {
    connection.start().catch((err) => console.error(err.toString()));
  },
  on(methodName, newMethod) {
    connection.on(methodName, (data) => {
      newMethod(data);
    });
  },
  invoke(methodName, data) {
    connection
      .invoke(methodName, data)
      .catch((err) => console.error(err.toString()));
  },
};
