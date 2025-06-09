import "./cardContainer.style.css"
import { Card } from "./Card/Card"
import { MovieCardContainerProps } from "../types/movie"

export const CardContainer: React.FC<MovieCardContainerProps> = ({page, items}) => {

    return (
        <div className="card-container">
            {items
                .map((item, index) => (
                    <Card key={`${item.id}-${page}-${index}`} posterPath={item.posterPath} title={item.title} id={item.id}/>
                ))
            }
        </div>
    )
}