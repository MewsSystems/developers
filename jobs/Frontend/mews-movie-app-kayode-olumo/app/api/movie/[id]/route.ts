import { NextRequest, NextResponse } from "next/server";
import { tmdbGet } from "@/services/tmdbClient";

export async function GET(_: NextRequest, { params }: { params: Promise<{ id: string }> }) {
  try {
    const { id } = await params;
    const data = await tmdbGet(`/movie/${id}?language=en-US`);
    return NextResponse.json(data);
  } catch (error) {
    console.error('Movie API Error:', error);
    return NextResponse.json({ error: "TMDB error" }, { status: 502 });
  }
}