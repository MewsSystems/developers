const token = import.meta.env.VITE_API_TOKEN;

export const options = {
  method: "GET",
  headers: {
    accept: "application/json",
    Authorization: token,
  },
};
