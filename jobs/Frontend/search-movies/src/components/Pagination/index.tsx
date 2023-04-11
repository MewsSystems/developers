import { useEffect, useState } from "react";
import ResponsivePagination from "react-responsive-pagination";
import { useNavigate } from "react-router";
import styled from "styled-components";
import { useAppDispatch, useAppSelector } from "../../store/hooks";
import { updateCurrentPage } from "../../store/reducers/BrowseMoviesReducer";
import { colors } from "../../utils/theme";

export const PaginationContainer = styled.div`
  .pagination {
    justify-content: center;
    display: flex;
    padding-left: 0;
    list-style: none;
  }

  .page-link {
    display: block;
    border: 1px solid ${colors.secondary};
    border-radius: 5px;
    padding: 5px 10px;
    margin: 0 2px;
    text-decoration: none;
    color: ${colors.primaryText};
  }

  a.page-link:hover {
    background-color: ${colors.secondary};
  }

  .page-item.active .page-link {
    color: ${colors.white};
    background-color: ${colors.secondary};
  }
`;

interface PaginationProps {
  currentPage: string;
}


/**
 * Provides movie browsing support for the browse endpoint in a numbered pagination
 * @param props {currentPage} Current page for the browse endpoint
 * @returns renders route responsive pagination component
 */
const Pagination = (props: PaginationProps) => {
  const { currentPage } = props;
  const browseMovies = useAppSelector((state) => state.browseMovies);
  const dispatch = useAppDispatch();
  const [totalPages, setTotalPages] = useState<number>();

  const navigate = useNavigate();

  function handlePageChange(newPage: any) {
    dispatch(updateCurrentPage(newPage));
    navigate(`/${newPage}`);
  }
  useEffect(() => {
    (function () {
      if (totalPages !== browseMovies.totalPages) {
        setTotalPages(browseMovies.totalPages);
      }
    })();
  }, [browseMovies.totalPages]);

  return (
    <PaginationContainer data-testid="pagination">
      <ResponsivePagination
        total={totalPages ?? 0}
        current={Number(currentPage) ?? 1}
        onPageChange={(page) => handlePageChange(page)}
      />
    </PaginationContainer>
  );
};

export default Pagination;
