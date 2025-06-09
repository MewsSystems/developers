import { Card } from "../Card/Card"
import { CardGrid } from "./style"
import { MovieCardContainerProps } from "./types"

export const CardContainer: React.FC<MovieCardContainerProps> = ({page, items}) => {

    return (
        <CardGrid>
            {items
                .map((item, index) => (
                    <Card key={`${item.id}-${page}-${index}`} posterPath={item.posterPath} title={item.title} id={item.id}/>
                ))
            }
        </CardGrid>
    )
}