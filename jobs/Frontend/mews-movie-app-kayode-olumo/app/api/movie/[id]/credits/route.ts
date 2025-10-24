import { NextRequest, NextResponse } from "next/server";
import { tmdbGet } from "@/services/tmdbClient";

export async function GET(_: NextRequest, { params }: { params: Promise<{ id: string }> }) {
  try {
    const { id } = await params;
    const data = await tmdbGet(`/movie/${id}/credits?language=en-US`);
    return NextResponse.json(data);
  } catch (error) {
    console.error('Credits API Error:', error);
    return NextResponse.json({ error: "TMDB error" }, { status: 502 });
  }
}