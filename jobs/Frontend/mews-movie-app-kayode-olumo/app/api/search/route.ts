import { type NextRequest, NextResponse } from "next/server"
import { fetchSearchResults } from "@/lib/api/tmdb"

export async function GET(request: NextRequest) {
  const searchParams = request.nextUrl.searchParams
  const query = searchParams.get("query") || ""
  const page = searchParams.get("page") || "1"

  try {
    const data = await fetchSearchResults(query, parseInt(page))
    return NextResponse.json(data)
  } catch (error) {
    console.error("Error in search route:", error)
    return NextResponse.json({ error: "Failed to fetch movies" }, { status: 500 })
  }
}