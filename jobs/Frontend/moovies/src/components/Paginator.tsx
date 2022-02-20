import styled from 'styled-components'
import { useAppSelector, useAppDispatch } from "../hooks/redux"
import { selectCurrentPage, incrementCurrentPage, decrementCurrentPage } from "../redux/currentPageSlice"

const Button = styled.button`
    padding: 1rem;
`

const Span = styled.span`
    margin: 0 1rem;
`

const Div = styled.div`
    margin: 1rem 0;
`


const Paginator = (props: any) => {

    const { maxPages } = props

    const currPage = useAppSelector(selectCurrentPage)
    const dispatch = useAppDispatch()

    const increasePage = () => {
        if (currPage + 1 > maxPages) {
            return
        }
        dispatch(incrementCurrentPage(1))
    }

    const decreasePage = () => {
        if (currPage - 1 < 1) {
            return
        }
        dispatch(decrementCurrentPage(1))
    }

    return (
        <Div>
            <Button onClick={decreasePage}>prev page</Button>
            <Span>current: {currPage}</Span>
            <Button onClick={increasePage}>next page</Button>
        </Div>
    )
}

export default Paginator
