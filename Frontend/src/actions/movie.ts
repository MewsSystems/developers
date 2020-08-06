import axios from 'axios';
import constants from '../constants';
import config from '../config';

export const fetchMovie = (id: string) => ({
  type: constants.FETCH_MOVIE,
  payload: axios(`${config.BASE_URL}/movie/${id}?api_key=${config.API_KEY}`).then((response) => response.data),
});

export const resetMovie = () => ({
  type: constants.RESET_MOVIE,
});
