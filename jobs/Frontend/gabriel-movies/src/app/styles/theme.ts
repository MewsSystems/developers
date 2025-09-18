export const theme = {
  colors: {
    bgDark: "#0B0B0C",
    textDark: "#E8E9EC",
    textLight: "#B7BBC3",
    link: "#7C7BFF",
    danger: "#EF4444",
    success: "#22C55E",
    warning: "#F59E0B"
  },
} as const;

export type AppTheme = typeof theme;
