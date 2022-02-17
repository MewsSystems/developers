import { Link } from "react-router-dom"

const SearchResults = (props: any) => {

    let { results } = props

    console.log(results)

    const names = results?.results?.map((result: any) => (
        <Link
            to={`/detail/${result.id}`}
            key={result.id}
        >
            <p>{result.title}</p>
        </Link>
    ))


    if (results === undefined) {
        return (
            <div>
                no results to display
            </div>
        )
    }
    return (
        <div>
            {names}
        </div>
    )
}

export default SearchResults
