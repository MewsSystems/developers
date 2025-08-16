import { useState } from 'react'
interface SearchProps {
    query: string
    onSearch: (query: string) => void
}

export default function Search({ query, onSearch }: SearchProps) {
    const [value, setValue] = useState(query)

    const onChange = (query: string) => {
        setValue(query)
        onSearch(query)
    }

    return (
        <>
            <div>
                <input
                    type="text"
                    placeholder="Type to search a movie"
                    value={value}
                    onChange={(e) => onChange(e.target.value)}
                />
            </div>
        </>
    )
}
