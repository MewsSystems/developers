import { useRef } from "react"

const SearchField = (props: any) => {

    const { query, setQuery } = props
    const inputEl = useRef<HTMLInputElement>(null)

    const handleChange = () => {
        setQuery(inputEl?.current?.value ?? "")
    }

    return (
        <div>
            <input ref={inputEl} value={query} onChange={handleChange} />
        </div>
    )
}

export default SearchField
