import styled from 'styled-components'
import { selectQuery, setQuery } from '../redux/querySlice'
import { setCurrentPage } from '../redux/currentPageSlice'
import { useAppDispatch, useAppSelector } from '../hooks/redux'
import { useRef } from "react"

const Input = styled.input`
    padding: 1rem;
    font-size: 1.25rem;
    margin: 1rem 0 1rem 0;
`

const SearchField = (props: any) => {

    const inputEl = useRef<HTMLInputElement>(null)
    const dispatch = useAppDispatch()
    const query = useAppSelector(selectQuery)

    const handleChange = () => {
        dispatch(setQuery(inputEl?.current?.value ?? ""))
        dispatch(setCurrentPage(1))
    }

    return (
        <div>
            <Input ref={inputEl} value={query} onChange={handleChange} />
        </div>
    )
}

export default SearchField
