import type { ReactNode } from "react"
import type { LinksFunction } from "@remix-run/node"
import {
  Links,
  Meta,
  Outlet,
  Scripts,
  ScrollRestoration,
} from "@remix-run/react"
import { cssBundleHref } from "@remix-run/css-bundle"
import "~/styles/global.module.css"

export const links: LinksFunction = () => [
  ...(cssBundleHref ? [{ rel: "stylesheet", href: cssBundleHref }] : []),
  { rel: "icon", type: "image/png", href: "/favicon.png" },
  { rel: "alternate icon", href: "/favicon.ico" },
]

export function Layout({ children }: { children: ReactNode }) {
  return (
    <html lang="en">
      <head>
        <meta charSet="utf-8" />
        <meta name="viewport" content="width=device-width, initial-scale=1" />
        <Meta />
        <Links />
      </head>
      <body>
        <header>
          <h1>🎞️ Movie Search App</h1>
        </header>
        {children}
        <footer>
          <p>
            This app is using data from{" "}
            <a href="https://www.themoviedb.org/">TMDB</a>
          </p>
        </footer>
        <ScrollRestoration />
        <Scripts />
      </body>
    </html>
  )
}

export default function App() {
  return <Outlet />
}
