import "./card.style.css"

export const Card = ({posterPath, title}) => {

    return (
        <div className="card">
            <img src={`https://image.tmdb.org/t/p/w500/${posterPath}.jpg`} alt={title} />
        </div>
    )
}