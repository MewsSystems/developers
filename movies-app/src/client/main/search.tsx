import React from 'react';
import {observer} from "mobx-react";
import {resolve, useInjection} from "inversify-react";
import {MoviesStore} from "./movies.store";


export const Search = observer(() => {
    const moviesStore = useInjection(MoviesStore);

    return (
        <div className="search">
            <input type="text" placeholder="Search for a movie" onChange={ev => moviesStore.searchString = ev.target.value}/>
            <div>hello { moviesStore.searchString }</div>
        </div>
    );

});