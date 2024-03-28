import React from 'react';
import {observer} from "mobx-react";
import {resolve, useInjection} from "inversify-react";
import classNames from 'classnames';
import {MoviesStore} from "./movies.store";
import './search.scss';


export const Pagination = observer(() => {
    const moviesStore = useInjection(MoviesStore);

    return (
        <div className="pagination">
            {
                moviesStore.pagesToPickFrom.map(page => {
                    const pageClass = classNames(
                        'pagination__page',
                        {
                            'pagination__page_active': moviesStore.currentPages.includes(page),
                        }
                    );
                    return (
                        <div key={page.toString()} className={pageClass}>{page}</div>
                    );
                })
            }
        </div>
    );

});