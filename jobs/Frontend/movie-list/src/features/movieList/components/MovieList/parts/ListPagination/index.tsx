import { FC } from "react";
import { ReactPaginateProps } from "react-paginate";
import { StyledPagination } from "./styled";

type Props = {} & ReactPaginateProps;

// TODO: build custom pagination, react-paginate is not very suitable
export const ListPagination: FC<
  Omit<Props, "previousLabel" | "nextLabel" | "pageRangeDisplayed">
> = (props) => {
  return (
    <StyledPagination
      {...props}
      previousLabel="<"
      nextLabel=">"
      pageRangeDisplayed={1}
    />
  );
};
