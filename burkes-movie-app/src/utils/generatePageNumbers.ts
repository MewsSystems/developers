export const generatePageNumbers = (numberOfPages: number) =>
  [...Array(numberOfPages).keys()].map((pageNumber) => pageNumber + 1);
