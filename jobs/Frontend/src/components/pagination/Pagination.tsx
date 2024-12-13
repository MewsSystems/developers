import { styled } from 'styled-components'
import Button from '../ui/button/Button'

type PaginationProps = {
  currentPage: number
  setCurrentPage: React.Dispatch<React.SetStateAction<number>>
  totalPages: number
}

function Pagination({
  currentPage,
  setCurrentPage,
  totalPages,
}: PaginationProps) {
  return (
    <PaginationContainer>
      <PaginationButtonContainer>
        <Button onClick={() => setCurrentPage(1)} disabled={currentPage === 1}>
          First
        </Button>
        <Button
          onClick={() => setCurrentPage((prev) => prev - 1)}
          disabled={currentPage === 1}
        >
          Previous
        </Button>
      </PaginationButtonContainer>
      <PaginationCurrentPageContainer>
        {currentPage}
      </PaginationCurrentPageContainer>
      <PaginationButtonContainer>
        <Button
          onClick={() => setCurrentPage((prev) => prev + 1)}
          disabled={currentPage === totalPages}
        >
          Next
        </Button>
        <Button
          onClick={() => setCurrentPage(totalPages)}
          disabled={currentPage === totalPages}
        >
          Last
        </Button>
      </PaginationButtonContainer>
    </PaginationContainer>
  )
}

const PaginationContainer = styled.div`
  display: flex;
  justify-content: space-between;
`

const PaginationButtonContainer = styled.div`
  display: flex;
  gap: 0.5rem;
  align-items: center;
  justify-content: space-between;
`
const PaginationCurrentPageContainer = styled.div`
  display: flex;
  justify-content: center;
  align-self: center;
  width: 44px;
  flex-basis: auto;
  padding: 0 0.6rem;
  border-radius: 0.8rem;
  background-color: var(--secondary-brand-color-100);
`

export default Pagination
