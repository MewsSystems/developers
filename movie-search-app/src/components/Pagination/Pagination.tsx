import { Link } from "react-router-dom";
import {
  PaginationContainer,
  PageButton,
  PageIndicator,
} from "./PaginationStyles";
import LABEL from "../../constants/Labels";

export const Pagination = ({
  currentPage,
  onPageChange,
  totalPages,
}: {
  currentPage: number;
  onPageChange: (page: number) => void;
  totalPages: number;
}) => {
  return (
    <PaginationContainer>
      <Link to={`/movies/${currentPage - 1}`}>
        <PageButton
          disabled={currentPage === 1}
          onClick={() => {
            window.scrollTo({ top: 0, behavior: "smooth" });
            onPageChange(currentPage - 1);
          }}
        >
          {LABEL.PAGE_BUTTON_PREVIOUS}
        </PageButton>
      </Link>
      <PageIndicator>
        Page {currentPage} of {totalPages}
      </PageIndicator>
      <Link to={`/movies/${currentPage + 1}`}>
        <PageButton
          disabled={currentPage === totalPages}
          onClick={() => {
            window.scrollTo({ top: 0, behavior: "smooth" });
            onPageChange(currentPage + 1);
          }}
        >
          {LABEL.PAGE_BUTTON_NEXT}
        </PageButton>
      </Link>
    </PaginationContainer>
  );
};
