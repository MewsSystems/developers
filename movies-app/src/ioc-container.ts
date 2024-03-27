import { Container } from 'inversify';
import { Provider } from 'inversify-react';
import {MoviesApi} from "./data/api/movies-api.store";
import {MoviesStore} from "./client/main/movies.store";

export const container = new Container();
container.bind(MoviesApi).toSelf().inSingletonScope();
container.bind(MoviesStore).toSelf().inSingletonScope();