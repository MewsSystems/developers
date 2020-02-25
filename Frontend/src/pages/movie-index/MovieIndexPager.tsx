import React from 'react'
import { useDispatch, useSelector } from 'react-redux'
import { ApplicationState } from '../../store'
import { pageChange } from '../../store/movie-index/actions'
import styled from '../../utils/styled'

const MovieIndexPager = () => {
  const { page, data } = useSelector((state: ApplicationState) => state.movieIndex)
  const current = page || 1
  const pagesTotal = data ? data.total_pages : 1
  const hasNext = current < pagesTotal

  const dispatch = useDispatch()
  const handlePageNext = () => {
    dispatch(pageChange(current + 1))
  }
  const handlePagePrev = () => {
    dispatch(pageChange(current - 1))
  }

  if (pagesTotal > 1) {
    return (
      <PaginationWrapper>
        {current > 1 && <PageNumber onClick={handlePagePrev}>← Previous</PageNumber>}
        <PageInfo>
          {current} / {pagesTotal}
        </PageInfo>
        {hasNext && <PageNumber onClick={handlePageNext}>Next →</PageNumber>}
      </PaginationWrapper>
    )
  }
  return null
}

export default MovieIndexPager

const PaginationWrapper = styled.div`
  padding-top: 2em;
  text-align: center;
`
const PageInfo = styled.span`
  display: inline-block;
  margin: 0 2em;
`

const PageNumber = styled.a`
  display: inline-block;
  cursor: pointer;
`
