import { debounceTime, filter, map, merge, share, Subject, switchMap } from "rxjs";
import { observable, runInAction, computed, makeObservable } from "mobx";
import { injectable } from "inversify";

import { MoviesApi } from "~data/api/movies-api.store";
import { Disposable } from "~data/disposable";
import { Movie, MoviesPage } from "~data/types";

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
export class MoviesStore extends Disposable {
    @observable
    private _error: MovieSearchError | undefined = undefined;
    private readonly _moviesResponses = observable.array<MoviesPage>([]);
    private readonly _searchString$ = new Subject<string>();

    constructor(
        private readonly _moviesApi: MoviesApi,
    ) {
        super();
        makeObservable(this);

        const serverError$ = new Subject<MovieSearchError>();
        const searchInput$ = this._searchString$.pipe(
            debounceTime(500),
            share(),
        );

        const moviesData$ = searchInput$.pipe(
            filter(isQueryValid),
            switchMap(searchString => this._moviesApi.search({query: searchString})),
        );

        this._disposeBag.add(
            moviesData$.subscribe(moviesData => {
                runInAction(() => {
                    this._moviesResponses.clear();
                    this._moviesResponses.push(moviesData);
                });
            })
        )

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
                // reset instantly on input change
                this._searchString$.pipe(map(() => undefined)),
            ).subscribe(error => {
                runInAction(() => {
                    this._error = error;
                });
            }),
        );
    }

    @computed
    get totalPages(): number {
        // every response has the same total_pages
        return this._moviesResponses[0]?.totalPages ?? 0;
    }

    @computed.struct
    get currentPages(): readonly number[] {
        return this._moviesResponses.map(response => response.page);
    }

    @computed.struct
    get pagesToPickFrom(): readonly number[] {
        // showing 2 first pages, 2 last pages and 4 pages around current page
        const currentLowestPage = this.currentPages[0];
        const currentHighestPage = this.currentPages[this.currentPages.length - 1];
        const pages = new Set([
            1,
            2,
            currentLowestPage - 2,
            currentLowestPage - 1,
            ...this.currentPages,
            currentHighestPage + 1,
            currentHighestPage + 2,
            this.totalPages - 1,
            this.totalPages,
        ]);
        return Array.from(pages).filter(page => page > 0 && page <= this.totalPages);
    }

    @computed.struct
    get movies(): readonly Movie[] {
        return this._moviesResponses.flatMap(response => response.movies);
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