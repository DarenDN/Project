export const httpClient = {
  IdentityManagementPath: "http://localhost:5002/api",
  ProjectManagementServicePath: "http://localhost:5012/api",
  MeetingServicePath: "http://localhost:5102/api",

  get(root, endpoint) {
    const headers = {
      "Content-Type": "application/json",
    };

    if (useToken) {
      const token = localStorage.getItem("authToken");

      if (token === null) {
        //TODO: fix
      }

      headers["Authorization"] = `bearer ${token}`;
    }
    return fetch(`${root}/${endpoint}`, {
      method: "GET",
      headers,
    });
  },

  post(root, endpoint, data, useToken = true) {
    const headers = {
      "Content-Type": "application/json",
    };

    if (useToken) {
      const token = localStorage.getItem("authToken");

      if (token === null) {
        //TODO: fix
      }

      headers["Authorization"] = `bearer ${token}`;
    }

    return fetch(`${root}/${endpoint}`, {
      method: "POST",
      headers,
      body: JSON.stringify(data),
    });
  },
};
