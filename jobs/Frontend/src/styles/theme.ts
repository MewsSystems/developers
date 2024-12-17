const theme = {
  colors: {
    backgroundGradient: "linear-gradient(0.25turn,  #0250c5, #d43f8d)",
    backgroundAttachment: "fixed",
    textPrimary: "#FFFFFF",
    textSecondary: "rgba(255,255,255,0.7)",
    cardBackground: "#2C2A3E",
    accent: "#E50914",
  },
  fonts: {
    primary: "'Inter', sans-serif",
  },
  spacing: (factor: number) => `${8 * factor}px`,
};

export default theme;
