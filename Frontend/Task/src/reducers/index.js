export const configuration = (state = {}, action) => {
  switch (action.type) {
    case 'CONFIGURATION':
      return action.payload;
    default:
      return state;
  }
};

export const data = (state = {}, action) => {
  switch (action.type) {
    case 'DATA':
      return action.payload;
    default:
      return state;
  }
};

export const test = (state = 'empty', action) => {
  switch (action.type) {
    case 'TEST':
      return action.payload;
    default:
      return state;
  }
};
