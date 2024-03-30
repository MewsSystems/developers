import React, { Fragment } from 'react';
import { observer } from "mobx-react";
import { useInjection } from "inversify-react";
import classNames from 'classnames';
import { MoviesStore } from "../movies.store";
import { Page } from "./page";
import './pagination.scss';

export const Pagination = observer(() => {
    const moviesStore = useInjection(MoviesStore);

    if (moviesStore.currentPages.length === 0) {
        return null;
    }

    const selectPage = (page: number) => {
        if (page > 0 && page <= moviesStore.totalPages) {
            moviesStore.setCurrentPage(page);
        }
    }


    return (
        <div className="pagination">
            <div
                className={classNames(
                    'pagination__item',
                    {
                        'pagination__item_disabled': moviesStore.currentPages.includes(1),
                    }
                )}
                onClick={() => selectPage((moviesStore?.firstOfCurrentPages ?? 1) - 1)}
            >
                Previous
            </div>
            <div className="pagination__pages">
                {
                    moviesStore.pagesToPickFrom.map((page, i) => (
                        <Fragment key={page.toString()}>
                            <Page
                                page={page}
                                selectPage={selectPage}
                                previousItemInPagination={moviesStore.pagesToPickFrom[i - 1]}
                                pageClassname={"pagination__item"}
                                pageCurrentClassname={"pagination__item_current"}
                            />
                        </Fragment>
                    ))
                }
            </div>
            <div className={classNames(
                'pagination__item',
                {
                    'pagination__item_disabled': moviesStore.currentPages.includes(moviesStore.totalPages),
                }
            )}
                 onClick={() => selectPage((moviesStore.lastOfCurrentPages ?? 1) + 1)}
            >
                Next
            </div>
        </div>
    );

});