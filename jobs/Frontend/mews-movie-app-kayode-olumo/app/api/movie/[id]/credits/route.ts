import { NextRequest, NextResponse } from "next/server";
import { tmdbGet } from "@/services/tmdbClient";

export async function GET(_: NextRequest, { params }: { params: { id: string } }) {
  try {
    const data = await tmdbGet(`/movie/${params.id}/credits?language=en-GB`);
    return NextResponse.json(data);
  } catch {
    return NextResponse.json({ error: "TMDB error" }, { status: 502 });
  }
}