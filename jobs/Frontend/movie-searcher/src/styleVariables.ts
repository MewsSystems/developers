export const basicStyle = {
  colors: {
    green: {
      10: "#3cb371",
    },
    grey: {
      50: "#808080",
    },
    yellow: {
      90: "#ff7e00",
    },
  },
  fontSize: {
    14: "14px",
    16: "16px",
    32: "32px",
    40: "40px",
  },
};

export const customStyle = {
  colors: {
    primary: basicStyle.colors.green[10],
  },
  fontSize: {
    h1: basicStyle.fontSize[40],
    h2: basicStyle.fontSize[32],
    h4: basicStyle.fontSize[16],
  },
  fontWeight: {
    normal: 400,
    bold: 500,
    black: 900,
  },
};
