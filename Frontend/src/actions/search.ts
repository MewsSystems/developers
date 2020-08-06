import axios from 'axios';
import constants from '../constants';
import config from '../config';

const makeRequestCreator = () => {
  let call;
  return (url: string) => {
    if (call) {
      call.cancel();
    }
    call = axios.CancelToken.source();
    return axios.get(url, { cancelToken: call.token });
  };
};

const get = makeRequestCreator();

export const fetchTopRatedMovies = (page = 1) => ({
  type: constants.FETCH_MOVIES,
  payload: get(`${config.BASE_URL}/movie/top_rated?api_key=${config.API_KEY}&region=CZ&page=${page}`).then(
    (response) => response.data
  ),
});

export const searchMovies = (search: string, page = 1) => ({
  type: constants.FETCH_MOVIES,
  payload: get(
    `${config.BASE_URL}/search/movie?api_key=${config.API_KEY}&query=${encodeURIComponent(search)}&page=${page}`
  ).then((response) => response.data),
});

export const handleSearchChange = (value: string) => ({
  type: constants.HANDLE_SEARCH_CHANGE,
  payload: value,
});
