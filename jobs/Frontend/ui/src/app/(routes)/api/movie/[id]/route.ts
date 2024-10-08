import MovieDbApiService from "@/app/services/MovieDbApi.service"

export async function GET(
  request: Request,
  { params }: { params: { id: string } },
) {
  const movieId = params.id
  const response = await MovieDbApiService.getMovie(movieId)

  return Response.json(response)
}
