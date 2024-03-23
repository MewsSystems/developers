export const generatePageNumbers = (numberOfPages: number) =>
  numberOfPages > 0
    ? [...Array(numberOfPages).keys()].map((pageNumber) => pageNumber + 1)
    : [];
