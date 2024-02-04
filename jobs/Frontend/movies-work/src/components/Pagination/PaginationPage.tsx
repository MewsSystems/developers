export default function PaginationPage({
  appPageValue,
  itemPageValue,
  changePage,
}) {
  const activeClass =
    "w-[3rem] justify-center relative z-10 inline-flex items-center bg-white px-4 py-2 text-sm font-semibold text-black focus:z-20 focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-indigo-600";
  const inactiveClass =
    "w-[3rem] justify-center relative inline-flex items-center px-4 py-2 text-sm font-semibold text-white ring-1 ring-inset ring-gray-300 hover:bg-gray-50 hover:text-black focus:z-20 focus:outline-offset-0";

  const currentClass =
    appPageValue == itemPageValue ? activeClass : inactiveClass;

  return (
    <a
      onClick={(e) => {
        e.preventDefault();
        changePage(itemPageValue);
      }}
      href="#"
      aria-current="page"
      className={currentClass}
    >
      {itemPageValue}
    </a>
  );
}
