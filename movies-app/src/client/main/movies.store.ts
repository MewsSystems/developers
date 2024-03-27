import {Subject, Subscription} from "rxjs";
import {observable, action, runInAction, computed, makeObservable} from "mobx";
import {inject, injectable} from "inversify";

import {MoviesApi} from "../../data/api/movies-api.store";

const enum MovieSearchError {
    BadInput,
    ServerError,
}
const MIN_CHARACTERS_FOR_SEARCH = 2;

@injectable()
export class MoviesStore {
    @observable
    private _searchString: string = '';
    private readonly _searchString$ = new Subject<string>();
    private readonly _disposeBag: Set<Subscription> = new Set();
    constructor(
        @inject(MoviesApi)
        private readonly _moviesApi: MoviesApi,
    ) {
        this._disposeBag.add(
            this._searchString$.subscribe(
                searchString => {
                    // console.warn('### _moviesApi', this._moviesApi);

                    console.warn('### searchString', searchString);
                    if (isQueryValid(searchString)) {

                        console.warn('### searchString set', searchString);
                        runInAction(() => this._searchString = searchString);
                        console.warn('### searchString set after', this._searchString);
                    }
                }
            )
        );
        makeObservable(this);
    }

    dispose(): void {
        this._disposeBag.forEach(sub => sub.unsubscribe());
    }

    get searchString(): string {
        return this._searchString;
    }

    set searchString(val: string) {
        this._searchString$.next(val);
    }
}

function isQueryValid(query: string): boolean {
    return query.trim().length > MIN_CHARACTERS_FOR_SEARCH;
}