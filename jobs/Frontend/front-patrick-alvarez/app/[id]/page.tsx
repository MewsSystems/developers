import { Loading } from '@/components/Loading'
import MovieDetail from '@/components/MovieDetail'
import { MovieDetailProvider } from '@/provider/MovieDetailProvider'
import { Suspense } from 'react'

export default async function MovieDetailPage({
    params,
}: {
    params: Promise<{ id: string }>
}) {
    const id = (await params).id

    return (
        <Suspense fallback={<Loading />}>
            <MovieDetailProvider id={id}>
                <MovieDetail />
            </MovieDetailProvider>
        </Suspense>
    )
}
