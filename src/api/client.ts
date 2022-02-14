export async function client(endpoint: string, {body, ...customConfig}: any = {}) {
  const headers = {'Content-Type': 'application/json'};

  const config = {
    method: body ? 'POST' : 'GET',
    ...customConfig,
    headers: {
      ...headers,
      ...customConfig.headers,
    },
  };

  if (body) {
    config.body = JSON.stringify(body);
  }

  let data;
  try {
    const response = await window.fetch(`${process.env.REACT_APP_API_URL}/${endpoint}`, config);
    data = await response.json();
    if (response.ok) {
      // Return a result object similar to Axios
      return {
        status: response.status,
        data,
        headers: response.headers,
        url: response.url,
      };
    }
    throw new Error(response.statusText);
  } catch (err) {
    return Promise.reject(err.message ? err.message : data);
  }
}

client.get = function (endpoint: string, params: any, customConfig = {}) {
  let url = endpoint;
  if (params) {
    url += '?' + new URLSearchParams(params).toString();
  }
  return client(url, {...customConfig, method: 'GET'});
};

client.post = function (endpoint: string, body: any, customConfig = {}) {
  return client(endpoint, {...customConfig, body});
};
