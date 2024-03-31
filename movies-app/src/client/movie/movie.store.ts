import { injectable } from 'inversify';
import { map, of, Subject, switchMap, take } from 'rxjs';
import { makeObservable, observable, runInAction } from 'mobx';
import { Disposable } from '~data/disposable';
import { MoviesApi } from '~data/api/movies-api.store';
import type { Movie } from '~data/types';
import { movieLikeTypeguard } from '~data/types';
import { locationWithMovieStateTypeguard } from './types';
import { logger } from '~data/logger/logger.store';

@injectable()
export class MovieStore extends Disposable {
    @observable.ref
    private _movieInfo: Movie | undefined = undefined;
    @observable
    private _couldNotLoadMovie = false;
    private _locationState$ = new Subject();

    constructor(
        private readonly _moviesApi: MoviesApi,
    ) {
        super();
        makeObservable(this);

        const stateFromLocationState$ = this._locationState$.pipe(
            take(1),
            map(locationState => {
                if (!locationWithMovieStateTypeguard(locationState)) {
                    return undefined;
                }
                try {
                    const parseData = JSON.parse(locationState.movie);
                    if (movieLikeTypeguard(parseData)) {
                        return {
                            ...parseData,
                            releaseDate: parseData.releaseDate ? new Date(parseData.releaseDate) : undefined,
                        };
                    }
                } catch (e) {
                    logger.error('Error parsing movie from location state: ', e);
                }
                return undefined;
            }),
        )

        this._disposeBag.add(
            stateFromLocationState$.pipe(
                switchMap(stateFromLocation => {
                    if (stateFromLocation) {
                        return of(stateFromLocation);
                    }
                    // If there is no location state, get movie from API
                    const movieStringId = window.location.href.match(/\/movie\/(\d+)/)?.[1];
                    const movieId = movieStringId ? parseInt(movieStringId) : undefined;
                    if (movieId === undefined || isNaN(movieId)) {
                        throw new Error(`Movie id in url is invalid: ${movieStringId}`);
                    }
                    return this._moviesApi.getMovieInfo(movieId);
                }),
            ).subscribe({
                next: movie => {
                    runInAction(() => {
                        this._movieInfo = movie;
                    });
                },
                error: e => {
                    logger.error('Error getting movie info: ', e);
                    runInAction(() => {
                        this._couldNotLoadMovie = true;
                    });
                }
            })
        );
    }

    setLocationState(locationState: unknown): void {
        this._locationState$.next(locationState);
    }

    get movieInfo(): Movie | undefined {
        return this._movieInfo;
    }

    get couldNotLoadMovie(): boolean {
        return this._couldNotLoadMovie;
    }
}