import "./searchBar.style.css"

export const SearchBar = () => {

    return (
        <div className="search-bar">
            <label htmlFor="search">Search movie:</label>
            <input type="text" id="search" placeholder="Type to search..." />
        </div>
    )
}