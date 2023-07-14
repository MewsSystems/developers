import { Dispatch, SetStateAction } from "react";

export interface PaginationControlsProps {
  page: number;
  totalPages: number;
  setPage: Dispatch<SetStateAction<number>>;
}
