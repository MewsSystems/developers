// Asynchronous super-simple fetch wrapper
async function request (method, url, data, headers = {}) {
  return fetch(url, {
    method: method.toUpperCase(),
    body: JSON.stringify(data),
    headers: {
      ...headers,
    },
  })
}

// Create basic REST methods
const TMethods = ['get', 'post', 'put', 'delete']
TMethods.forEach((method) => {
  request[method] = request.bind(null, method) // Call a method by request.get()
})

export default request
