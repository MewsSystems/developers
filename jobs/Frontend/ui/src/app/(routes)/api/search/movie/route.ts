import MovieDbApiService from "@/app/services/MovieDbApi.service"

export async function GET(request: Request): Promise<Response> {
  const { searchParams } = new URL(request.url)
  const query = searchParams.get("query")
  const page = searchParams.get("page")

  if (!query || !page) {
    return new Response("Invalid parameters, please provide a query and page", {
      status: 400,
    })
  }

  const response = await MovieDbApiService.searchMovies(query, Number(page))

  return Response.json(response)
}
