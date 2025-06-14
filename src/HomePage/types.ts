import { MovieCardContainerProps } from "../types/movie"
import { SearchBarProps } from "./HomepageContent/SearchBar/types"

export type HomePageProps = SearchBarProps &
    MovieCardContainerProps  & {
        handleLoadMore: () => void
    }