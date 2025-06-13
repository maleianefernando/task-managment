const api = axios.create({
  baseURL: "http://localhost:5055/api",
});

const endpoints = {
  login: "/login",
};

async function fetchData(url) {
  try {
    const response = await api.get(url);
    return { ...response.data, status: response.status };
  } catch (error) {
    return error.response;
  }
}

async function postData(url, data) {
  try {
    const response = await api.post(url, data, {
      headers: {
        "Content-Type": "application/json",
      },
    });
    return { ...response.data, status: response.status };
  } catch (error) {
    return error.response;
  }
}
