"use client";

import { usePathname, useRouter, useSearchParams } from "next/navigation";
import styled from "styled-components";
import { tabletMediaQuery } from "@/breakpoints";

const Container = styled.div`
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 20px;
  margin-top: 10px;
  margin-bottom: 30px;

  ${tabletMediaQuery} {
    margin-bottom: 10px;
  }
`;

const Button = styled.button`
  font-size: 14px;
  width: 100px;
  height: 45px;
  background: ${(props) => props.theme.primary.button};
  border: 1px solid ${(props) => props.theme.primary.border};
  border-radius: 5px;
  color: ${(props) => props.theme.secondary.text};

  &.disabled {
    background: ${(props) => props.theme.primary.background};
    color: ${(props) => props.theme.primary.text};
    opacity: 0.5;
  }

  &:hover {
    cursor: pointer;
    background: ${(props) => props.theme.primary.buttonHover};

    &.disabled {
      background: ${(props) => props.theme.primary.background};
      cursor: not-allowed;
    }
  }
`;

interface PaginationProps {
  totalPages: number;
}

const Pagination = ({ totalPages }: PaginationProps) => {
  const searchParams = useSearchParams();
  const pathname = usePathname();
  const { replace } = useRouter();

  const query = searchParams.get("query") ?? "";
  const currentPage = Number(searchParams.get("page")) || 1;

  const handleClick = (term: string, page: number) => {
    const params = new URLSearchParams(searchParams);

    params.set("page", page.toString());

    if (term) {
      params.set("query", term);
    } else {
      params.delete("query");
    }

    replace(`${pathname}?${params.toString()}`);
  };

  return (
    <Container data-testid="pagination">
      <Button
        onClick={() => handleClick(query, currentPage - 1)}
        disabled={currentPage === 1}
        className={currentPage === 1 ? "disabled" : ""}
      >
        Previous
      </Button>
      <span data-testid="pageNumbers">
        Page {currentPage} of {totalPages}
      </span>
      <Button
        onClick={() => handleClick(query, currentPage + 1)}
        disabled={currentPage === totalPages}
        className={currentPage === totalPages ? "disabled" : ""}
      >
        Next
      </Button>
    </Container>
  );
};

export default Pagination;
