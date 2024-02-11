import { useContext } from "react";
import { AppContext } from "../../contexts/AppContext";
import PaginationPage from "./PaginationPage";
import PaginationEllipsis from "./PaginationEllipsis";
import PaginationChevron from "./PaginationChevron";

export default function Pagination() {
  const {
    page: appPage,
    maximumPage,
    setAppSearchParams,
  } = useContext(AppContext);

  const changePageHandler = (page: number) => {
    // TODO wont work for more params - only movie and page HARDCODED - use URLSearchParams.entries() to iterate over all params
    setAppSearchParams(null, page);
  };
  // TODO react to shorter lists - with less than 7 pages

  if (maximumPage === null) return null;

  const generatePages = (page: number, maximumPage: number) => {
    if (page === 1 || page === 2) {
      return [1, 2, 3, "..", "..", maximumPage - 1, maximumPage];
    }
    if (page === maximumPage || page === maximumPage - 1) {
      return [1, 2, "..", "..", maximumPage - 2, maximumPage - 1, maximumPage];
    }
    if (page === 3) {
      return [1, 2, 3, 4, "..", maximumPage - 1, maximumPage];
    }
    if (page === maximumPage - 2) {
      return [
        1,
        2,
        "..",
        maximumPage - 3,
        maximumPage - 2,
        maximumPage - 1,
        maximumPage,
      ];
    }
    if (page >= 4 || page <= maximumPage - 3) {
      return [1, 2, "..", appPage, "..", maximumPage - 1, maximumPage];
    }
  };
  const itemPages = generatePages(appPage, maximumPage);
  return (
    <div className="flex items-center justify-between">
      {/* For small screens - only previous and next page button */}
      <div className="flex flex-1 justify-between sm:hidden">
        <PaginationChevron
          direction="previous"
          appPage={appPage}
          maximumPage={maximumPage}
          changePage={changePageHandler}
        />
        <PaginationChevron
          direction="next"
          appPage={appPage}
          maximumPage={maximumPage}
          changePage={changePageHandler}
        />
      </div>
      {/*  For bigger screens - shows the whole pagination with page numbers */}
      <div className="hidden sm:flex sm:flex-1 sm:items-center sm:justify-center">
        <div>
          <nav
            className="isolate inline-flex -space-x-px rounded-md shadow-sm"
            aria-label="Pagination"
          >
            <PaginationChevron
              direction="previous"
              appPage={appPage}
              maximumPage={maximumPage}
              changePage={changePageHandler}
            />

            {/* Generate page numbers between previous and next button */}
            {itemPages.map((itemPage, index) => {
              const itemKey = itemPage + "-" + index;

              if (itemPage === "..")
                return <PaginationEllipsis key={itemKey} />;
              return (
                <PaginationPage
                  key={itemKey}
                  appPageValue={appPage}
                  itemPageValue={itemPage}
                  changePage={changePageHandler}
                />
              );
            })}

            <PaginationChevron
              direction="next"
              appPage={appPage}
              maximumPage={maximumPage}
              changePage={changePageHandler}
            />
          </nav>
        </div>
      </div>
    </div>
  );
}
