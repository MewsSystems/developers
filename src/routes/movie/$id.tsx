import { createFileRoute } from '@tanstack/react-router'

export const Route = createFileRoute('/movie/$id')({
  component: MovieDetail,
})

function MovieDetail() {
  const { id } = Route.useParams()
  return <div>Post ID: {id}</div>
}
