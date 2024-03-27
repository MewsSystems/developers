import {debounce, debounceTime, filter, map, mapTo, merge, Subject, Subscription, switchMap} from "rxjs";
import {observable, action, runInAction, computed, makeObservable} from "mobx";
import {inject, injectable} from "inversify";

import {MoviesApi} from "../../data/api/movies-api.store";

const enum MovieSearchErrorType {
    BadInput,
    ServerError,
}
type MovieSearchError = Readonly<{
    type: MovieSearchErrorType;
    message: string; // todo: i18n
}>;
const MIN_CHARACTERS_FOR_SEARCH = 3;

@injectable()
export class MoviesStore {
    @observable
    private _searchString: string = '';
    @observable
    private _error: MovieSearchError | undefined = undefined;
    private readonly _searchString$ = new Subject<string>();
    private readonly _disposeBag: Set<Subscription> = new Set();
    constructor(
        @inject(MoviesApi)
        private readonly _moviesApi: MoviesApi,
    ) {
        makeObservable(this);

        const serverError$ = new Subject<MovieSearchError>();
        const searchInput$ = this._searchString$.pipe(
            debounceTime(500),
        );

        this._disposeBag.add(
            searchInput$.pipe(
                filter(isQueryValid),
                switchMap(searchString => this._moviesApi.search({query: searchString})),
            ).subscribe({
                next: (data) => {
                    console.warn('### data', data);
                },
                error: err => {
                    serverError$.next({
                        type: MovieSearchErrorType.ServerError,
                        message: 'Oops, something went wrong. Please try again later.',
                    });
                    // todo: send error to sentry, kibana, etc
                    console.error('Failed to get data from server: ', err);
                },
            }),
        );

        this._disposeBag.add(
            merge(
                searchInput$.pipe(
                    filter(query => query.length > 0),
                    filter(query => !isQueryValid(query)),
                    debounceTime(500),
                    map(() => ({
                        type: MovieSearchErrorType.BadInput,
                        message: `Please enter at least ${MIN_CHARACTERS_FOR_SEARCH} characters`,
                    })),
                ),
                serverError$,
            ).subscribe(error => {
                runInAction(() => {
                    this._error = error;
                });
            }),
        );
    }

    dispose(): void {
        this._disposeBag.forEach(sub => sub.unsubscribe());
    }

    get searchString(): string {
        return this._searchString;
    }

    get error(): MovieSearchError | undefined {
        return this._error;
    }

    searchForString(val: string): void {
        this._searchString$.next(val);
    }
}

function isQueryValid(query: string): boolean {
    return query.trim().length >= MIN_CHARACTERS_FOR_SEARCH;
}