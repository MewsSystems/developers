# Movie Search App

A minimalistic, responsive movie search application built with React, TypeScript, and Vite. This app allows users to browse popular movies, search for specific movie/movies, and view detailed movie information using The Movie Database (TMDb) API.

## Features

- **Popular Movies**: Browse trending movies with infinite scroll pagination
- **Search Functionality**: Search for movies with debounced input for optimal performance
- **Responsive Design**: Mobile-first design that works on all devices
- **Modern UI**: Clean, intuitive interface with hover effects and smooth animations
- **Movie Details**: Detailed movie information pages with comprehensive data
- **Performance**: Optimized with React Query for efficient data fetching and caching
- **Testing**: Comprehensive test suite with Vitest and React Testing Library

## Tech Stack

- **Frontend**: React 19, TypeScript, Vite
- **Styling**: Styled Components
- **State Management**: TanStack React Query
- **Routing**: React Router v7
- **HTTP Client**: Axios
- **Icons**: Lucide React
- **Testing**: Vitest, React Testing Library
- **Linting**: ESLint, TypeScript ESLint

## Getting Started

### Prerequisites

- Node.js 22.12.0+ (required for Vite)
- npm or yarn package manager
- TMDb API key

### Installation

1. **Clone the repository**

   ```bash
   git clone <repository-url>
   cd movie-search
   ```

2. **Install dependencies**

   ```bash
   npm install
   # or
   yarn install
   ```

3. **Set up environment variables**
   Create a `.env` file in the root directory:

   ```env
   VITE_TMDB_API_KEY=your_tmdb_api_key_here
   ```

   To get a TMDb API key:

   - Visit [The Movie Database](https://www.themoviedb.org/)
   - Create an account
   - Go to Settings → API
   - Request an API key for developer use

4. **Start the development server**

   ```bash
   npm run dev
   # or
   yarn dev
   ```

5. **Open your browser**
   Navigate to `http://localhost:5173`

### Run Tests

```bash
# Run tests in watch mode
npm test

# Run tests once
npm run test:run
```

## Available Scripts

- `npm run dev` - Start development server
- `npm run build` - Build for production
- `npm run preview` - Preview production build
- `npm run lint` - Run ESLint
- `npm test` - Run tests in watch mode
- `npm run test:run` - Run tests once

## Key Features Explained

### Infinite Scroll

The app implements infinite scroll for browsing popular movies, automatically loading more content as the user scrolls down. This should increase overal UX as the database of movies is enormous and potential pagination component might no friendly to use.

### Debounced Search

Search functionality uses a debounced input to optimize API calls and provide a smooth UX.

### Responsive Grid

Movie cards are displayed in a responsive grid that adapts to different screen sizes.

### Movie Details

Clicking on a movie card navigates to a detailed view with comprehensive information including:

- Movie overview
- Release date and runtime
- Genres list
- Movie rating
- Production details

### Future improvements

- Add theme in order to unify styling across the App
- Code cleanup (remove duplicatated components)
- Introduce requests error handling
- UI improvements in DetailPage (layout, styling, nice components)
- virtualization for grid

## Author

**Petar Zayakov**
