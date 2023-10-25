export const TRIGGER_SEARCH = 'TRIGGER_SEARCH';

const initialState = {
  search: '',
};

const counterReducer = (
  state = initialState,
  action: { type: string; payload: { query: string } },
) => {
  switch (action.type) {
    case 'TRIGGER_SEARCH':
      return { ...state, search: action.payload.query };

    default:
      return state;
  }
};

export default counterReducer;
