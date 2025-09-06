import "@testing-library/jest-dom";
import { vi } from "vitest";

// --- API mocks ---
vi.mock("../api/tmdb", () => ({
  fetchMovies: vi.fn(),
  fetchMovieDetail: vi.fn(),
}));

// --- Component mocks ---
vi.mock("../components/Layout", () => ({
  default: ({ children }) => <div data-testid="layout">{children}</div>,
}));

vi.mock("../components/Loader", () => ({
  default: () => <div data-testid="loader">Loading...</div>,
}));

vi.mock("../components/MovieCard", () => ({
  default: ({ id, title, poster_path }) => (
    <div data-testid={`movie-card-${id}`}>
      <h3>{title}</h3>
      <img src={poster_path} alt={title} />
    </div>
  ),
}));

vi.mock("../components/EmptyState", () => ({
  default: ({ message, icon }) => (
    <div data-testid="empty-state">
      {icon}
      <span>{message}</span>
    </div>
  ),
}));

vi.mock("../components/ErrorState", () => ({
  default: ({ message }) => <div data-testid="error-state">{message}</div>,
}));

vi.mock("../components/PosterImage", () => ({
  default: ({ posterPath, alt, maxWidth }) => (
    <img
      data-testid="poster-image"
      src={posterPath}
      alt={alt}
      style={{ maxWidth }}
    />
  ),
}));

vi.mock("../components/Placeholder", () => ({
  default: ({ width, height, text }) => (
    <div data-testid="placeholder" style={{ width, height }}>
      {text}
    </div>
  ),
}));

// --- Styled components mocks ---
vi.mock("../styles/styles", () => ({
  MovieSearchInput: ({ children, ...props }) => (
    <input {...props}>{children}</input>
  ),
  MovieCardGrid: ({ children }) => (
    <div data-testid="movie-grid">{children}</div>
  ),
  LoadMoreButton: ({ children, ...props }) => (
    <button {...props}>{children}</button>
  ),
  MovieTextContainer: ({ children }) => (
    <div data-testid="movie-text-container">{children}</div>
  ),
}));

// --- Router mocks ---
vi.mock("react-router-dom", async () => {
  const actual = await vi.importActual("react-router-dom");
  return {
    ...actual,
    useParams: vi.fn(),
    Link: ({ children, to, ...props }) => (
      <a href={to} {...props}>
        {children}
      </a>
    ),
  };
});
