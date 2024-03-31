import { useState } from 'react'

interface SearchProps {
    onSearch: (query: string) => void
}

export default function Search({ onSearch }: SearchProps) {
    const [searchQuery, setSearchQuery] = useState('')
    const onChange = (value: string) => {
        setSearchQuery(value)
        onSearch(value)
    }

    return (
        <>
            <div>
                <input
                    type="text"
                    placeholder="Type to search a movie"
                    value={searchQuery}
                    onChange={(e) => onChange(e.target.value)}
                />
            </div>
        </>
    )
}
