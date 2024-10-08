import Link from "next/link"

import { Button } from "@/app/components/ui"
import { MovieContainer } from "@/app/features/movie"

export default function MoviePage({ params }: { params: { id: string } }) {
  const movieId = params.id
  return (
    <>
      <Button>
        <Link href="/">Go back to home page</Link>
      </Button>
      <MovieContainer movieId={movieId} />
    </>
  )
}
