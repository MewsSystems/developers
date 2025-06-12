import React from "react"
import { SearchBarProps } from "./types"
import { SearchBarWrapper, Label, Input } from "./style"

export const SearchBar: React.FC<SearchBarProps> = ({searchTerm, setSearchTerm}) => {

    return (
        <SearchBarWrapper>
            <Label htmlFor="search">Search movie:</Label>
            <Input 
                type="text"
                id="search"
                placeholder="Type to search..."
                value={searchTerm}
                onChange={(event: React.ChangeEvent<HTMLInputElement>) => {
                    setSearchTerm(event.target.value)
                }}
            />
        </SearchBarWrapper>
    )
}