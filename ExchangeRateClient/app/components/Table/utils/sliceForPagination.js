import { slice } from 'ramda';

export default (currentPage, maxNumberOfRows) => slice(
    maxNumberOfRows * (currentPage - 1),
    maxNumberOfRows * currentPage,
);
