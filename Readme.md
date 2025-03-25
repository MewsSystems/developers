# 🎬 Movie Searcher

Movie Searcher is a web application that allows users to search for movies using data from [TheMovieDB.org](https://www.themoviedb.org/). Users can browse search results and click on a movie card to be redirected to a detailed view of the selected movie.

## ✨ Features

- 🔍 Search for movies via TheMovieDB API
- 🎞️ Interactive movie cards with details
- 🧭 Seamless navigation between search and movie detail pages
- 🎨 Modern and responsive UI with smooth animations

## 🛠️ Tech Stack

- **T3 App** (Next.js + TypeScript + TailwindCSS + TRPC)
- **Next.js** – React framework for server-side rendering and routing
- **React** – Frontend library for building UI components
- **TypeScript** – Strongly typed JavaScript for better developer experience
- **TailwindCSS** – Utility-first CSS framework
- **TRPC** – End-to-end typesafe APIs
- **Shadcn UI Components** – Pre-built and customizable UI components
- **Framer Motion** – Smooth animations with motion divs

## 🚀 Getting Started

### Prerequisites

- Node.js >= 16.x
- npm, yarn, pnpm

### Installation

1. Clone the repository:

   ```bash
   git clone https://github.com/your-username/movie-searcher.git
   cd movie-searcher

   ```

2. Install dependencies:

   ```bash
      pnpm install
      or
      npm install
      or
      yarn install
   ```

3. Set up environment variables:
   Create a .env.local file in the root of your project and add:

   ```bash
   TMDB_URL="https://api.themoviedb.org/3"
   TMDB_API_KEY= Your_tmdb_api_key
   NEXT_PUBLIC_TMDB_IMAGE_URL="https://image.tmdb.org/t/p/w600_and_h900_face/"

   ```

4. Run project:
   ```bash
      pnpm run dev
      or
      npm run dev
      or
      yarn dev
   ```

The app should now be running at http://localhost:3000 🚀
