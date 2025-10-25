import { type NextRequest, NextResponse } from "next/server"
import { tmdbGet } from "@/services/tmdb"

export async function GET(request: NextRequest) {
  const searchParams = request.nextUrl.searchParams
  const query = searchParams.get("query") || ""
  const page = searchParams.get("page") || "1"

  try {
    const params = new URLSearchParams({
      page: page,
    })

    if (query.trim()) {
      params.append("query", query)
    }

    const endpoint = query.trim()
      ? `/search/movie?${params}`
      : `/movie/popular?${params}`

    const data = await tmdbGet(endpoint)
    return NextResponse.json(data)
  } catch (error) {
    console.error("Error in search route:", error)
    return NextResponse.json({ error: "Failed to fetch movies" }, { status: 500 })
  }
}