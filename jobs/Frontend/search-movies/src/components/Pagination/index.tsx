import { useEffect, useState } from "react";
import { useDispatch, useSelector } from "react-redux";
import ResponsivePagination from "react-responsive-pagination";
import { useNavigate } from "react-router";
import styled from "styled-components";
import { AppDispatch, RootState } from "../../store";
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

export default function Pagination(props: PaginationProps) {
  const { currentPage } = props;
  const browseMovies = useSelector((state: RootState) => state.browseMovies);
  const [totalPages, setTotalPages] = useState<number>();

  const navigate = useNavigate();
  const dispatch = useDispatch<AppDispatch>();

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
    <PaginationContainer>
      <ResponsivePagination
        total={totalPages ?? 0}
        current={Number(currentPage) ?? 1}
        onPageChange={(page) => handlePageChange(page)}
      />
    </PaginationContainer>
  );
}
