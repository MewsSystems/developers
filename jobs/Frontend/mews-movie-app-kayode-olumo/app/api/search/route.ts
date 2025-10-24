import { NextRequest, NextResponse } from "next/server";
import { tmdbGet } from "@/services/tmdbClient";

export async function GET(request: NextRequest) {
  const query = (request.nextUrl.searchParams.get("q") ?? "").trim();
  const pageNumber = request.nextUrl.searchParams.get("page") ?? "1";

  if (!query) {
    return NextResponse.json({ page: 1, results: [], total_pages: 0, total_results: 0 });
  }

  try {
    const data = await tmdbGet(`/search/movie?query=${encodeURIComponent(query)}&page=${pageNumber}&include_adult=false&language=en-GB`);
    return NextResponse.json(data);
  } catch {
    return NextResponse.json({ error: "TMDB error" }, { status: 502 });
  }
}