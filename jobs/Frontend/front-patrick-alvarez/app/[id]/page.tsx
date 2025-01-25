import MovieDetail from '@/components/MovieDetail'
import { MovieDetailProvider } from '@/provider/MovieDetailProvider'

interface Props {
    params: {
        id: string
    }
}

export default function MovieDetailPage({ params }: Props) {
    return (
        <MovieDetailProvider id={params.id}>
            <MovieDetail />
        </MovieDetailProvider>
    )
}
