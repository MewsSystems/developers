import { PaginationButton } from './PaginationButton';

export const Pagination = () => {
  return (
    <div>
      <PaginationButton direction="Previous" />
      <a href="">1, 2, 3...</a>
      <PaginationButton direction="Next" />
    </div>
  );
};
