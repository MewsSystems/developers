const fetchBase = (url: string, options: RequestInit = { method: "GET" }) => {
  const { VITE_API_URL, VITE_API_KEY } = import.meta.env;

  return fetch(`${VITE_API_URL}${url}`, {
    headers: {
      accept: "application/json",
      Authorization: `Bearer ${VITE_API_KEY}`,
    },
    ...options,
  });
};

export default fetchBase;
