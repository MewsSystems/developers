import React, { Fragment } from 'react';
import {observer} from "mobx-react";
import {useInjection} from "inversify-react";
import classNames from 'classnames';
import {MoviesStore} from "../movies.store";
import './pagination.scss';

export const Pagination = observer(() => {
    const moviesStore = useInjection(MoviesStore);

    if (moviesStore.totalPages === 0) {
        return null;
    }

    return (
        <div className="pagination">
            <div className={classNames(
                'pagination__item',
                {
                    'pagination__item_disabled': moviesStore.currentPages.includes(1),
                }
            )}
            >
                Previous
            </div>
            <div className="pagination__pages">
                {
                    moviesStore.pagesToPickFrom.map((page, i) => {
                        const pageClass = classNames(
                            'pagination__item',
                            {
                                'pagination__item_current': moviesStore.currentPages.includes(page),
                            }
                        );
                        if (moviesStore.pagesToPickFrom[i - 1] && moviesStore.pagesToPickFrom[i - 1] !== page - 1) {
                            return (
                                <Fragment key={page.toString()}>
                                    <div className="pagination__dots">...</div>
                                    <div className={pageClass}>{page}</div>
                                </Fragment>
                            )
                        }
                        return (
                            <div key={page.toString()} className={pageClass}>{page}</div>
                        );
                    })
                }
            </div>
            <div className={classNames(
                'pagination__item',
                {
                    'pagination__item_disabled': moviesStore.currentPages.includes(moviesStore.totalPages),
                }
            )}>
                Next
            </div>
        </div>
    );

});