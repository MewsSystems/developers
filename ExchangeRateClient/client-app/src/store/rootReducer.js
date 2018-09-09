// @flow

const initalState = {
  data: [],
};

export default (state = { ...initalState }, action) => {
  switch (action.type) {
    case "SIMPLE_ACTION":
      return {
        result: action.payload,
      };
    default:
      return state;
  }
};
