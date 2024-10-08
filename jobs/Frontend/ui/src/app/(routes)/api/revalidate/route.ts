import { revalidatePath } from "next/cache"

export async function GET(request: Request) {
  const { searchParams } = new URL(request.url)

  const path = searchParams.get("path") || "/"

  revalidatePath(path)

  return new Response("Revalidated", { status: 200 })
}
