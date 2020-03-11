import axios from 'axios';

const instance = axios.create({ baseURL: 'https://api.themoviedb.org/3' });

const getAll = (searchTerm = '', page = 1) => {
  const term = searchTerm.replace(' ', '+');
  if (searchTerm) {
    return instance.get(`search/movie?api_key=${process.env.REACT_APP_API_KEY}&query=${term}&page=${page}`);
  } else {
    return instance.get(`movie/popular?api_key=${process.env.REACT_APP_API_KEY}&page=${page}`);
  }
};

const getDetail = id => {
  return instance.get(`movie/${id}?api_key=${process.env.REACT_APP_API_KEY}`);
};

export { getAll, getDetail };
