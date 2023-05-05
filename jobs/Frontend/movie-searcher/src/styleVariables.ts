export const basicStyle = {
  colors: {
    green: {
      10: "#3cb371",
    },
  },
  fontSizes: {
    14: "14px",
    16: "16px",
    28: "28px",
  },
};

export const customStyle = {
  colors: {
    primary: basicStyle.colors.green[10],
  },
  fontSize: {
    h1: basicStyle.fontSizes[28],
  },
  fontWeight: {
    normal: 400,
    bold: 500,
  },
};
