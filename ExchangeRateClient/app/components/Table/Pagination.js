import React from 'react';
import PropTypes from 'prop-types';
import { Pagination as BSPagination, PaginationItem, PaginationLink } from 'reactstrap';
import { __, add, apply, compose, cond, divide, gte, inc, lte, map, range, subtract, T, useWith } from 'ramda';
import { duplicate } from 'ramda-extension';

const getPageItemRange = (currentPage, numberOfPages) => cond([
    [
        compose(lte(currentPage), Math.ceil, divide(__, 2)),
        range(0),
    ],
    [
        compose(gte(__, numberOfPages), add(currentPage), Math.ceil, divide(__, 2)),
        compose(range(__, numberOfPages), subtract(numberOfPages)),
    ],
    [
        T,
        compose(apply(useWith(range, [subtract(currentPage), add(currentPage)])), duplicate, Math.ceil, divide(__, 2)),
    ],
]);

// eslint-disable-next-line react/display-name
const renderPaginationItem = (currentPage, handlePageChange) => (number) =>
    <PaginationItem key={`p${number}`} active={inc(number) === currentPage}>
        <PaginationLink onClick={handlePageChange(inc(number))}>
            {inc(number)}
        </PaginationLink>
    </PaginationItem>;

const renderPaginationItems = (currentPage, numberOfPages, handlePageChange) => compose(
    map(renderPaginationItem(currentPage, handlePageChange)),
    getPageItemRange(currentPage, numberOfPages),
);

const Pagination = ({ currentPage, handlePageChange, maxNumberOfPageItems, numberOfPages }) =>
    <BSPagination>
        { currentPage !== 1 &&
            <PaginationItem onClick={handlePageChange(1)}>
                <PaginationLink previous />
            </PaginationItem>
        }
        {renderPaginationItems(currentPage, numberOfPages, handlePageChange)(maxNumberOfPageItems)}
        { currentPage !== numberOfPages &&
            <PaginationItem onClick={handlePageChange(numberOfPages)}>
                <PaginationLink next />
            </PaginationItem>
        }
    </BSPagination>;

Pagination.propTypes = {
    currentPage: PropTypes.number,
    handlePageChange: PropTypes.func,
    maxNumberOfPageItems: PropTypes.number,
    numberOfPages: PropTypes.number,
};

export default Pagination;
