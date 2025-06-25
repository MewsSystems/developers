import { keyframes } from "styled-components"

const shimmer = keyframes`
  0% {
    background-position: -468px 0;
  }
  100% {
    background-position: 468px 0;
  }
`

export const theme = {
  colors: {
    primary: "#00d4ff",
    primaryHover: "#00b8e6",
    secondary: "#ff6b35",
    background: "#0f0f23",
    surface: "#1a1a2e",
    surfaceHover: "#16213e",
    text: "#ffffff",
    textSecondary: "#b8bcc8",
    textMuted: "#6c757d",
    border: "#2c2c54",
    success: "#28a745",
    error: "#dc3545",
    warning: "#ffc107",
  },

  fonts: {
    primary:
      '"Roboto", "Oxygen", "Ubuntu", "Cantarell", "Fira Sans", "Droid Sans", "Helvetica Neue", sans-serif',
    mono: '"SFMono-Regular", Consolas, "Liberation Mono", Menlo, Courier, monospace',
  },

  fontSizes: {
    xs: "0.75rem",
    sm: "0.875rem",
    base: "1rem",
    lg: "1.125rem",
    xl: "1.25rem",
    "2xl": "1.5rem",
    "3xl": "1.875rem",
    "4xl": "2.25rem",
  },

  spacing: {
    xs: "0.25rem",
    sm: "0.5rem",
    md: "1rem",
    lg: "1.5rem",
    xl: "2rem",
    "2xl": "3rem",
    "3xl": "4rem",
  },

  breakpoints: {
    sm: "640px",
    md: "768px",
    lg: "1024px",
    xl: "1280px",
  },

  borderRadius: {
    sm: "0.25rem",
    base: "0.5rem",
    lg: "0.75rem",
    xl: "1rem",
    full: "9999px",
  },

  shadows: {
    sm: "0 1px 2px 0 rgba(0, 0, 0, 0.05)",
    base: "0 1px 3px 0 rgba(0, 0, 0, 0.1), 0 1px 2px 0 rgba(0, 0, 0, 0.06)",
    md: "0 4px 6px -1px rgba(0, 0, 0, 0.1), 0 2px 4px -1px rgba(0, 0, 0, 0.06)",
    lg: "0 10px 15px -3px rgba(0, 0, 0, 0.1), 0 4px 6px -2px rgba(0, 0, 0, 0.05)",
  },

  animations: {
    shimmer,
  },
}
