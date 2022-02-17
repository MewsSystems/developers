import { useState, useRef, useEffect } from "react"
import useSearch from "../hooks/useSearch"

const SearchField = (props: any) => {

    const { setResults } = props
    const [query, setQuery] = useState("")
    const inputEl = useRef<HTMLInputElement>(null)
    let results = useSearch(query, 1)

    const handleChange = () => {
        setQuery(inputEl?.current?.value ?? "")
    }

    useEffect(() => {
        if (!results) {
            return
        }
        setResults(results)
    }, [results])

    return (
        <div>
            <input ref={inputEl} value={query} onChange={handleChange} />
        </div>
    )
}

export default SearchField
