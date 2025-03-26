import "./searchBar.style.css"

export const SearchBar = ({searchTerm, setSearchTerm}) => {

    return (
        <div className="search-bar">
            <label htmlFor="search">Search movie:</label>
            <input 
                type="text"
                id="search"
                placeholder="Type to search..."
                value={searchTerm}
                onChange={(event) => {setSearchTerm(event.target.value)}}
            />
        </div>
    )
}