# 🎬 Movie Search App

A simple React + TypeScript application for searching movies using the [TMDB API](https://www.themoviedb.org/).

## 🖼️ Preview

Here’s a quick look at the app in action:

![How it looks](src/assets/how%20it%20looks.png)

## 🚀 Features

- Search for movies with debounce (search starts after typing stops)
- Paginated results with **Load More** button
- Movie detail view with overview, release date, and rating
- Persist movie search results in session storage
- Built using **React, TypeScript, styled-components, react-router-dom**
- Continuous Integration automatically runs tests and checks code formatting on every push or pull request.

## 🛠️ Tech Stack

- [React](https://react.dev/)
- [TypeScript](https://www.typescriptlang.org/)
- [Vite](https://vitejs.dev/) (build tool)
- [styled-components](https://styled-components.com/)
- [React Router](https://reactrouter.com/)

## 📦 Installation

1. Clone your fork of the repository:

```bash
git clone git@github.com:youneshenniwrites/Mews-Developers.git

cd Mews-Developers/frontend/younes-assignment
```

2. Install dependencies:

```bash
npm install
```

3. Create a `.env` file in the project root and add your TMDB API key:

```env
VITE_TMDB_API_KEY=your-api-key-here
VITE_TMDB_API_URL=your-api-url-here
VITE_TMDB_API_READ_ACCESS_TOKEN=your-read-access-token-here
```

4. Run the development server:

```bash
npm run dev
```

5. Run tests locally:

```
npm run test
```

Open [http://localhost:5173](http://localhost:5173) in your browser.

## 📖 Usage

- Start typing a movie name in the search input → results will appear.
- Click on a movie to view its details.
- Use **Load More** to fetch additional results.

## ✅ Notes

- API requests use TheMovieDB’s free API — you need your own key.
- [Get your API KEY](https://developer.themoviedb.org/docs/getting-started)
