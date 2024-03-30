import React, { Fragment } from 'react';
import {observer} from "mobx-react";
import {useInjection} from "inversify-react";
import classNames from 'classnames';
import {MoviesStore} from "../movies.store";
import './page.scss';

type PageProps = Readonly<{
    page: number;
    selectPage: (page: number) => void;
    previousItemInPagination: number | undefined;
    pageClassname: string;
    pageCurrentClassname: string;
}>;
export const Page = observer(({
    page,
    selectPage,
    previousItemInPagination,
    pageClassname,
    pageCurrentClassname,
}: PageProps) => {
    const moviesStore = useInjection(MoviesStore);

    // for array of current pages render only first element in a range way, like 5 - 8
    if (moviesStore.currentPages.includes(page) && moviesStore.firstOfCurrentPages !== page) {
        return null
    }
    const pageClass = classNames(
        pageClassname,
        {
            [pageCurrentClassname]: moviesStore.currentPages.includes(page),
        }
    );
    if (moviesStore.currentPages.includes(page) && moviesStore.firstOfCurrentPages === page) {
        return (
            <div key={page.toString()} className={pageClass} onClick={() => selectPage(page)}>
                {
                    moviesStore.firstOfCurrentPages === moviesStore.lastOfCurrentPages
                        ? moviesStore.firstOfCurrentPages
                        : `${moviesStore.firstOfCurrentPages} - ${moviesStore.lastOfCurrentPages}`
                }
            </div>
        )

    }

    // if there is a gap between current item and previous item, render additional dots
    if (previousItemInPagination && previousItemInPagination !== page - 1) {
        return (
            <Fragment key={page.toString()}>
                <div className="dots">...</div>
                <div className={pageClass} onClick={() => selectPage(page)}>{page}</div>
            </Fragment>
        )
    }
    return (
        <div key={page.toString()} className={pageClass} onClick={() => selectPage(page)}>{page}</div>
    );

});