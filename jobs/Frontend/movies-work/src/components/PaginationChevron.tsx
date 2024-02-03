export default function PaginationChevron({
  direction,
  appPage,
  maximumPage,
  changePage,
}) {
  const onClickChevron = (e) => {
    e.preventDefault();
    const newPage = direction === "previous" ? appPage - 1 : appPage + 1;

    if (
      (direction === "previous" && appPage === 1) ||
      (direction === "next" && appPage === maximumPage)
    )
      return;
    changePage(newPage);
  };

  const previous = (
    <a
      href="#"
      onClick={onClickChevron}
      className="relative inline-flex items-center rounded-l-md px-2 py-2 text-gray-400 ring-1 ring-inset ring-gray-300 hover:bg-gray-50 focus:z-20 focus:outline-offset-0"
    >
      <span className="sr-only">Previous</span>
      <p className="h-5 w-5" aria-hidden="true">
        &#60;
      </p>
    </a>
  );

  const next = (
    <a
      href="#"
      onClick={onClickChevron}
      className="relative inline-flex items-center rounded-r-md px-2 py-2 text-gray-400 ring-1 ring-inset ring-gray-300 hover:bg-gray-50 focus:z-20 focus:outline-offset-0"
    >
      <span className="sr-only">Next</span>
      <p className="h-5 w-5" aria-hidden="true">
        &#62;
      </p>
    </a>
  );

  const currentChevron = direction === "previous" ? previous : next;

  return currentChevron;
}
