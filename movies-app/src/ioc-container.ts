import { Container } from 'inversify';
import { MoviesApi } from '~data/api/movies-api.store';
import { MoviesStore } from './client/main/movies.store';
import { MovieStore } from './client/movie/movie.store';

export const container = new Container();
container.bind(MoviesApi).toSelf().inSingletonScope();
container.bind(MoviesStore).toSelf().inSingletonScope();
container.bind(MovieStore).toSelf().inTransientScope();