import styled from "styled-components"

const PaginationContainer = styled.div`
  display: flex;
`

const PageDetail = styled.div`
  padding: 0.5em 0.7em;
  font-size: 0.8em;
`

const Button = styled.button`
  padding: 0.5em 0.7em;
  color: ${(props) => props.theme.main};
  border: 1px solid ${(props) => props.theme.light_gray};
  border-radius: 0.375rem;
  background-color: transparent;
  &:hover {
    background-color: ${(props) => props.theme.lightest_gray};
    cursor: pointer;
  }
  &:disabled {
    background-color: transparent;
    color: ${(props) => props.theme.light_gray};
    cursor: default;
  }
  &:focus {
    outline: 0;
    border-color: transparent;
    box-shadow: inset 0 0 0 2px ${(props) => props.theme.primary};
  }
`

function Pagination({
  page,
  setPage,
  totalPages,
}: {
  page: number
  setPage: (_: number) => void
  totalPages: number
}) {
  return (
    <PaginationContainer>
      <Button
        type="button"
        disabled={page === 1}
        onClick={() => setPage(page - 1)}
      >
        {"<"}
      </Button>
      <PageDetail>
        Page {page} of {totalPages}
      </PageDetail>
      <Button
        type="button"
        disabled={page === totalPages}
        onClick={() => setPage(page + 1)}
      >
        {">"}
      </Button>
    </PaginationContainer>
  )
}

export default Pagination
