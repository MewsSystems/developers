import MovieDbAPI from '../libs/MovieDbAPI';
import { API_MOVIE_DB_KEY } from '../env';

export default new MovieDbAPI(API_MOVIE_DB_KEY);
