export const getPaginationModel = (currentPage: number, numberOfPages: number): (number | '...')[] => {
    const currentPageMatchesAnyOf = (...pageNumbers: number[]) => pageNumbers.some(page => currentPage === page);
    const getNumberArray = (from: number, count: number) =>
        Array.from(Array(count).keys(), (_, i) => from + i);

    if (numberOfPages < 6) {
        return getNumberArray(1, numberOfPages);
    }

    if (currentPageMatchesAnyOf(1, 2)) {
        return [...getNumberArray(1, 3), '...', numberOfPages - 1, numberOfPages];
    }

    if (currentPageMatchesAnyOf(numberOfPages, numberOfPages - 1)) {
        const lowerBound = numberOfPages - 2;
        return [1, 2, '...', ...getNumberArray(lowerBound, 3)];
    }

    if (currentPageMatchesAnyOf(3)) {
        return [...getNumberArray(1, 4), '...', numberOfPages];
    }

    if (currentPageMatchesAnyOf(numberOfPages - 2)) {
        const lowerBound = numberOfPages - 3;
        return [1, '...', ...getNumberArray(lowerBound, 4)];
    }

    return [1, '...', currentPage - 1, currentPage, currentPage + 1, '...', numberOfPages];
};