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

export const searchText = (state = '', action) => {
  switch (action.type) {
    case 'SEARCH_TEXT':
      return action.payload;
    default:
      return state;
  }
};
