import React, { Fragment } from 'react';
import { observer } from 'mobx-react';
import { useInjection } from 'inversify-react';
import { MoviesStore } from '../movies.store';
import { PageItem, Dots } from './pagination.styled';

type PageProps = Readonly<{
    page: number;
    selectPage: (page: number) => void;
    previousItemInPagination: number | undefined;
}>;
export const Page = observer(({
    page,
    selectPage,
    previousItemInPagination,
}: PageProps) => {
    const moviesStore = useInjection(MoviesStore);

    // for array of current pages render only first element in a range way, like 5 - 8
    if (moviesStore.currentPages.includes(page) && moviesStore.firstOfCurrentPages !== page) {
        return null
    }
    if (moviesStore.currentPages.includes(page) && moviesStore.firstOfCurrentPages === page) {
        return (
            <PageItem
                disabled={false}
                current={true}
                key={page.toString()}
                onClick={() => selectPage(page)}
            >
                {
                    moviesStore.firstOfCurrentPages === moviesStore.lastOfCurrentPages
                        ? moviesStore.firstOfCurrentPages
                        : `${moviesStore.firstOfCurrentPages} - ${moviesStore.lastOfCurrentPages}`
                }
            </PageItem>
        )

    }

    const isCurrentPage = moviesStore.currentPages.includes(page);
    // if there is a gap between current item and previous item, render additional dots
    if (previousItemInPagination && previousItemInPagination !== page - 1) {
        return (
            <Fragment key={page.toString()}>
                <Dots>...</Dots>
                <PageItem
                    disabled={false}
                    current={isCurrentPage}
                    onClick={() => selectPage(page)}
                >
                    {page}
                </PageItem>
            </Fragment>
        )
    }
    return (
        <PageItem
            disabled={false}
            current={isCurrentPage}
            key={page.toString()}
            onClick={() => selectPage(page)}
        >
            {page}
        </PageItem>
    );

});