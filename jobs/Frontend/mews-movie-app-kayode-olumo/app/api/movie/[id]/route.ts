import { type NextRequest, NextResponse } from "next/server"
import { fetchMovieDetails } from "@/lib/api/tmdb"

export async function GET(request: NextRequest, { params }: { params: Promise<{ id: string }> }) {
  const { id } = await params

  // Validate movie ID
  if (!id || isNaN(Number(id))) {
    return NextResponse.json({ error: "Invalid movie ID" }, { status: 400 })
  }

  try {
    const data = await fetchMovieDetails(id)
    return NextResponse.json(data)
  } catch (error) {
    console.error("Error fetching movie details:", error)
    if (error instanceof Error && error.message === "Movie not found") {
      return NextResponse.json({ error: "Movie not found" }, { status: 404 })
    }
    return NextResponse.json({ error: "Failed to fetch movie details" }, { status: 500 })
  }
}