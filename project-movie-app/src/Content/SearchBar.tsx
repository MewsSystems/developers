import "./searchBar.style.css"
import React from "react"
import { SearchBarProps } from "../types/searchBar"

export const SearchBar: React.FC<SearchBarProps> = ({searchTerm, setSearchTerm}) => {

    return (
        <div className="search-bar">
            <label htmlFor="search">Search movie:</label>
            <input 
                type="text"
                id="search"
                placeholder="Type to search..."
                value={searchTerm}
                onChange={(event: React.ChangeEvent<HTMLInputElement>) => {
                    setSearchTerm(event.target.value)
                }}
            />
        </div>
    )
}