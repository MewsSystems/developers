import { clsx } from 'clsx'
import { ChangeEvent, memo, useState } from 'react'

interface SearchBarProps {
    // onSearch?: (query: string) => void
    handleSearch?: (query: string) => void
    placeholder?: string
    className?: string
}

const SearchBar = memo(
    ({
        handleSearch,
        placeholder = 'Search for a movie',
        className,
    }: SearchBarProps) => {
        const [value, setValue] = useState('')

        const onSearch = (event: ChangeEvent<HTMLInputElement>) => {
            setValue(event.target.value)
            handleSearch?.(event.target.value)
        }

        return (
            <input
                type="text"
                value={value}
                className={clsx(
                    className,
                    'w-full max-w-md rounded-md border border-gray-300 p-2 text-black focus:outline-none focus:ring-2 focus:ring-blue-500',
                )}
                onChange={onSearch}
                placeholder={placeholder}
            />
        )
    },
)

SearchBar.displayName = 'SearchBar'
export default SearchBar
