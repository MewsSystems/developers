# Movie Explorer App

A React app for exploring movies using The Movie Database (TMDB) API with clean, minimalistic and responsive design.

## 🚀 Tech Stack

### Core
- **React 18**
- **TypeScript**
- **Vite**
- **React Router**
- **React Query**
- **Axios**
- **Styled Components**
- **React Error Boundary**
- **React.lazy()**

### Testing
- **Jest**
- **React Testing Library**
- **Playwright**

### Development Tools
- **ESLint**
- **Prettier**
- **TypeScript ESLint**

## 📁 Project Structure

```
src/
├── api/                    # API integration
│   └── movieApi/          # Movie API specific code
│       ├── endpoints/     # API endpoint definitions
│       └── utils/        # API utilities and constants
├── app/
│   ├── components/        # Shared components
│   │   ├── ApiErrorScreen/
│   │   ├── ErrorBoundaryFallback/
│   │   └── Loader/
│   └── _tests_/          # Test files
│       ├── e2e/          # End-to-end tests
│       └── utils/        # Test utilities
├── hooks/                 # Custom React hooks
├── pages/                 # Page components
│   ├── common/           # Shared page components
│   │   ├── MovieCover/
│   │   └── PopcornLoader/
│   ├── MovieDetailsPage/
│   └── MoviesListPage/
│       └── components/   # Page-specific components
│           ├── EmptyInitialState/
│           ├── EmptySearchResult/
│           ├── MovieCard/
│           ├── Pagination/
│           └── SearchInput/
├── routes/               # Route definitions
├── styles/               # Global styles
├── types/                # TypeScript type definitions
└── utils/                # Utility functions
```

## 🧪 Testing

### Unit Tests
- Located in `__tests__` directories next to the code they test
- Uses Jest and React Testing Library
- Run with: `yarn test:unit`
- Watch mode: `yarn test:unit:watch`

### End-to-End Tests
- Located in `src/app/_tests_/e2e/`
- Uses Playwright for browser automation
- Tests user flows and API interactions
- Includes custom mock utilities for API responses
- Run with: `yarn test:e2e`
- UI mode: `yarn test:e2e:ui`

## 🎣 Custom Hooks

- **useDebounce**
  - generic hook for delaying value updates
  - perfect for preventing excessive API calls
  - easy customizable
- **usePagination**
  - manages pagination state and navigation
  - provides page controls, boundary checks, and URL synchronization
  - used in <MoviesListPage /> for seamless page navigation
- **useSearchInput**
  - combines input state management with debouncing
  - synchronizes with URL params

## 🚦 Getting Started

1. Clone the repository
2. Install dependencies:
   ```bash
   yarn install
   ```
3. Create a `.env.local` file with your TMDB API key:
   ```
   VITE_TMDB_API_KEY=your_api_key_here
   ```
4. Start the development server:
   ```bash
   yarn start
   ```

## 📝 Available Scripts

- `yarn start` - Start development server
- `yarn build` - Build for production (runs TypeScript build)
- `yarn lint` - Run ESLint
- `yarn preview` - Preview production build
- `yarn test:unit` - Run unit tests (no need to run dev server locally)
- `yarn test:e2e` - Run end-to-end tests (no need to run dev server locally)
- `yarn test:e2e:ui` - Run end-to-end tests with UI (no need to run dev server locally)
- `yarn test:all` - Run all tests (unit + e2e - no need to run dev server locally)

## 🧩 Key Features

- Movie search with debounced input
- Movie details view with rich information
- Error handling with Error Boundaries
- Loading states and error messages
- Responsive design
- Type-safe API integration
- Comprehensive test coverage
- Modern pagination
- **Optimized Performance:**
  - Lazy loading of route components
  - Code splitting by route
  - Suspense boundaries for loading states

## ⚠️ Error Handling

The app includes comprehensive error handling:
- API error states (401, 404, 500)
- User-friendly error messages from centralized constants
- Fallback UI components
- Error boundary for catching React errors
- Typed error messages and status codes

## 📦 State Management

- React Query for server state and caching
- URL state for search terms and pagination
- Local state for UI components
- Proper loading and error states
- Debounced search state

## 🎨 Styling

- Styled Components for component-level styling
- Global styles for consistent theming
- Responsive design patterns
- Loading and error state styling
