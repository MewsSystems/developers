import { NextResponse } from "next/server";
import { tmdbGet } from "@/services/tmdb";

export async function GET(
  _req: Request,
  { params }: { params: { id: string } }
) {
  try {
    const data = await tmdbGet(`/movie/${params.id}?language=en-US`);
    return NextResponse.json(data);
  } catch (error) {
    console.error("Error fetching movie details:", error);
    return NextResponse.json({ error: "Failed to fetch movie details" }, { status: 500 });
  }
}