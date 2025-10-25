import { NextResponse } from "next/server";

export async function GET(
  _req: Request,
  { params }: { params: { id: string } }
) {
  const res = await fetch(`https://api.themoviedb.org/3/movie/${params.id}?language=en-US`, {
    headers: {
      Authorization: `Bearer ${process.env.TMDB_ACCESS_TOKEN!}`,
      accept: "application/json",
    },
    cache: "no-store",
  });

  if (!res.ok) return NextResponse.json({ error: "Upstream error" }, { status: 502 });
  return NextResponse.json(await res.json());
}