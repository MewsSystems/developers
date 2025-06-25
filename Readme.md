# Movie Search Application

This application provides an intuitive interface to search for movies and view detailed information using The Movie Database (TMDb) API.

The app is deployed on Netlify: https://tmdb-mews.netlify.app

## 🚀 Quick Start

### Prerequisites

- Node.js 18+ 
- pnpm (recommended) or npm/yarn

### Installation & Setup

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd developers
   ```

2. **Install dependencies**
   ```bash
   pnpm install
   ```

3. **Environment Configuration**
   
   Create a `.env` file in the root directory with your TMDb API credentials:
   ```env
   VITE_TMDB_BASE_URL=https://api.themoviedb.org/3
   VITE_TMDB_API_KEY=your_api_key_here
   VITE_ENABLE_MSW=false # Set to true to enable MSW mocking
   ```
   
   > **Note:** Get your free API key from [The Movie Database (TMDb)](https://www.themoviedb.org/settings/api)

4. **Start the development server**
   ```bash
   pnpm dev
   ```

5. **Open your browser**
   
   Navigate to `http://localhost:5173` to view the application.

## 🎯 Features

### Core Functionality
- **Movie Search**: Real-time search with debounced input for optimal performance
- **Movie Details**: Comprehensive movie information including ratings, genres, and descriptions
- **Responsive Design**: Fully responsive interface that works on desktop, tablet, and mobile
- **Pagination**: Efficient navigation through search results
- **Loading States**: Skeleton screens and loading indicators for better UX
- **Error Handling**: Comprehensive error handling with user-friendly messages

### Technical Features
- **TypeScript**: Full type safety throughout the application
- **State Management**: React Query for efficient server state management
- **Styled Components**: Component-scoped CSS with theming support
- **Performance Optimization**: Code splitting, memoization, and efficient re-renders
- **Testing**: Comprehensive unit and integration tests
- **E2E Testing**: Cypress for end-to-end testing
- **Code Quality**: BiomeJS for linting and formatting

## 🛠️ Technology Stack

### Frontend Framework
- **React 19** - Latest React with modern hooks and concurrent features
- **TypeScript** - Type-safe development
- **Vite** - Fast build tool and development server

### State Management & Data Fetching
- **TanStack React Query** - Server state management and caching
- **Axios** - HTTP client with interceptors

### Styling & UI
- **Styled Components** - CSS-in-JS with theming
- **Lucide React** - Modern icon library
- **Responsive Design** - Mobile-first approach

### Testing & Quality
- **Vitest** - Fast unit testing framework
- **Testing Library** - Component testing utilities
- **Cypress** - End-to-end testing
- **MSW (Mock Service Worker)** - API mocking for tests
- **BiomeJS** - Linting and formatting

### Development Tools
- **Husky** - Git hooks for code quality
- **Lint-staged** - Pre-commit linting

## 📁 Project Structure

```
src/
├── components/             # Reusable UI components
│   ├── MovieCard/         # Example: Individual movie card component
│   │   ├── MovieCard.tsx
│   │   ├── MovieCard.styles.ts
│   │   ├── MovieCard.test.tsx
│   │   └── index.ts
│   ├── SearchInput/       # Debounced search input
│   ├── Pagination/        # Pagination component
│   ├── MovieGrid/         # Movie grid layout
│   └── ...               # Other UI components
├── constants/             # Application constants
│   ├── errors.ts         # Error messages and codes
│   └── routes.ts         # Route definitions
├── hooks/                # Custom React hooks
│   ├── __tests__/        # Hook tests
│   ├── useDebounce.ts    # Input debounce hook
│   ├── useMovies.ts      # Movie management hook
│   ├── usePagination.ts  # Pagination state hook
│   └── useSearchState.ts # Search state hook
├── lib/                  # External library configurations
│   ├── __tests__/       # External library tests
│   └── api.ts           # Axios configuration
├── pages/               # Page components
│   ├── SearchPage/      # Example: Main search page
│   │   ├── SearchPage.tsx
│   │   ├── SearchPage.styles.ts
│   │   ├── SearchPage.test.tsx
│   │   └── index.ts
│   ├── MovieDetailPage/ # Movie detail page
│   ├── NotFoundPage/    # 404 page
│   └── ...             # Other pages
├── services/            # API service layer
│   ├── __tests__/       # API service layer tests
│   └── movieService.ts  # TMDb API integration
├── styles/              # Global styles and theming
│   ├── GlobalStyles.ts  # Global application styles
│   ├── theme.ts         # Theme configuration
│   └── styled.d.ts      # Styled-components type definitions
├── test/                # Test configuration and mocks
│   ├── mocks/           # MSW handlers and fixtures
│   └── setup.ts         # Test setup configuration
├── types/               # TypeScript type definitions
│   ├── movie.ts         # Movie-related types
│   └── env.d.ts         # Environment variable types
├── utils/               # Utility functions
│   ├── __tests__/       # Utility functions tests
│   └── movieUtils.ts    # Movie-related utilities
├── main.tsx             # Application entry point
└── vite-env.d.ts        # Vite types
```

## 🎬 Application Flow

### Search Page (`/`)
- **Search Input**: Debounced search with real-time results
- **Popular Movies**: Default view showing trending movies
- **Movie Grid**: Responsive grid layout with movie cards
- **Pagination**: Navigate through multiple pages of results
- **Loading States**: Skeleton cards during data fetching

### Movie Detail Page (`/movie/:id`)
- **Detailed Information**: Complete movie details including runtime, genres, and production info
- **High-Quality Images**: Backdrop and poster images from TMDb
- **Rating System**: User ratings and vote counts
- **Navigation**: Back to search functionality

## 🔧 Available Scripts

### Development
```bash
pnpm dev          # Start development server
pnpm build        # Build for production
pnpm preview      # Preview production build
```

### Testing
```bash
pnpm test         # Run unit tests in watch mode
pnpm test:run     # Run unit tests once
pnpm test:ui      # Open Vitest UI
pnpm test:coverage # Generate test coverage report
```

### End-to-End Testing
```bash
pnpm cypress:open # Open Cypress test runner
pnpm cypress:run  # Run Cypress tests headlessly
```

### Code Quality
```bash
pnpm lint         # Check code with BiomeJS
pnpm lint:fix     # Fix linting issues
pnpm format       # Format code
pnpm check        # Run all quality checks
```

## 🔌 API Integration

This application integrates with [The Movie Database (TMDb) API](https://developer.themoviedb.org/docs/getting-started) to fetch movie data:

### Endpoints Used
- `GET /movie/popular` - Fetch popular movies
- `GET /search/movie` - Search movies by query
- `GET /movie/{id}` - Get detailed movie information

## 🧪 Testing Strategy

### Unit Tests
- **Components**: Testing component behavior and rendering
- **Hooks**: Testing custom hook logic and state management
- **Services**: Testing API integration and error handling
- **Utilities**: Testing helper functions

### Integration Tests
- **Page Components**: Testing complete page functionality
- **API Integration**: Testing service layer with mocked responses

### E2E Tests
- **User Flows**: Complete user journey testing with Cypress
- **Cross-browser**: Ensuring compatibility across different browsers

## 🔄 State Management

### React Query Implementation
- **Caching**: Intelligent caching of API responses
- **Error Boundaries**: Graceful error handling
- **Loading States**: Built-in loading state management

### Custom Hooks
- **useMovies**: Movie search and fetching logic
- **useDebounce**: Input debouncing for search optimization
- **usePagination**: Pagination state management
- **useSearchState**: Search input and results state

## 🔄 CI/CD

The project includes a comprehensive CI/CD pipeline that automatically runs both unit tests and end-to-end tests.

### GitHub Actions Workflow

The CI pipeline is defined in `.github/workflows/ci.yml` and includes:

- **Unit Tests Job (`unit-tests`)**: 
  - Runs linting and type checks with BiomeJS
  - Executes unit tests with Vitest
  - Builds the project to ensure compilation

- **E2E Tests Job (`cypress-tests`)**:
  - Runs after unit tests pass
  - Starts development server with MSW enabled
  - Executes Cypress end-to-end tests
  - Caches Cypress binary for faster subsequent runs

### Triggered On
- Push to `master` or `main` branches
- Pull requests targeting `master` or `main` branches

### Environment Configuration
- Uses **Node.js 22.x** for consistent testing environment
- Uses **pnpm** for package management with caching
- Enables **MSW (Mock Service Worker)** for reliable API mocking in tests

## 🎨 Design System

### Theme Configuration
- **Colors**: Consistent color palette throughout the application
- **Typography**: Responsive font sizes and weights
- **Spacing**: Consistent spacing scale
- **Breakpoints**: Mobile-first responsive design

### Styling Architecture
- **Co-located Styles**: Styled components are kept in separate files alongside their React components to maintain better component cohesion, improve developer experience by keeping related code together, and facilitate maintenance. This approach works well for this application's scope and complexity.

### Bundle Analysis
```bash
pnpm build
pnpm preview
```

## 🔒 Environment Variables

| Variable | Description | Required |
|----------|-------------|----------|
| `VITE_TMDB_BASE_URL` | TMDb API base URL | Yes |
| `VITE_TMDB_API_KEY` | Your TMDb API key | Yes |
| `VITE_ENABLE_MSW` | Enable MSW mocking | No |

## 📝 License

This project is part of a technical assessment and is for demonstration purposes.

## 🔗 Useful Links

- [The Movie Database API](https://www.themoviedb.org/documentation/api)
- [React Documentation](https://react.dev/)
- [TypeScript Handbook](https://www.typescriptlang.org/docs/)
- [TanStack Query](https://tanstack.com/query/latest)
- [Styled Components](https://styled-components.com/)
- [Vite Documentation](https://vitejs.dev/)

---
