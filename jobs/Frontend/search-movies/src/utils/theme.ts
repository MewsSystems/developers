export const colors = {
  primary: "#4b4bf7",
  primaryDark: "#bcbcbc",
  primaryText: "#fff",
  secondaryText: "#000",
  primaryLight: "#2b2b2b",
  secondary: "#2e22d2",
  secondaryLight: "#2e22d2",
  white: "#fff",
  black: "#000",
  appBackground:
    "linear-gradient(121deg, rgba(11,9,20,1) 0%, rgba(13,17,57,1) 74%)",
};

const size = {
  mobileS: "320px",
  mobileM: "375px",
  mobileL: "425px",
  tablet: "768px",
  laptop: "1024px",
  laptopL: "1440px",
  desktop: "2560px",
};

export const device = {
  mobileS: `(min-width: ${size.mobileS})`,
  mobileM: `(min-width: ${size.mobileM})`,
  mobileL: `(min-width: ${size.mobileL})`,
  tablet: `(min-width: ${size.tablet})`,
  laptop: `(min-width: ${size.laptop})`,
  laptopL: `(min-width: ${size.laptopL})`,
  desktop: `(min-width: ${size.desktop})`,
  desktopL: `(min-width: ${size.desktop})`,
};
