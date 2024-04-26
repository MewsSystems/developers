import type { MetaFunction } from "@remix-run/node"
import { Link } from "@remix-run/react"

export const meta: MetaFunction = () => {
  return [
    { title: "Movie Search App" },
    { name: "description", content: "Simple movie search application" },
  ]
}

export default function Index() {
  return (
    <main>
      <h1>Find the movie...</h1>
      <Link to={"/movie-details/1"}>Movie 1</Link>
    </main>
  )
}
