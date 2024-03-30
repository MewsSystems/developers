import { injectable } from "inversify";
import { from, map, of, Subject, switchMap, take } from "rxjs";
import { makeObservable, observable, runInAction } from "mobx";
import { Disposable } from "~data/disposable";
import { MoviesApi } from "~data/api/movies-api.store";
import type { Movie } from "~data/types";
import { movieLikeTypeguard } from "~data/types";
import { locationWithMovieStateTypeguard } from "./types";

@injectable()
export class MovieStore extends Disposable {
    @observable.ref
    private _movieInfo: Movie | undefined = undefined;
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
                    console.error('Error parsing movie', e);
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
                    if ( movieId === undefined || isNaN(movieId)) {
                        throw new Error('Movie id is invalid');
                    }
                    return this._moviesApi.getMovieInfo(movieId);
                }),
            ).subscribe(movieInfo => {
                runInAction(() => this._movieInfo = movieInfo);
            })
        );
    }

    setLocationState(locationState: unknown): void {
        this._locationState$.next(locationState);
    }

    get movieInfo(): Movie | undefined {
        return this._movieInfo;
    }
}