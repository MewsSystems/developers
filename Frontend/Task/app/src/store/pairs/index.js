import { ADD_PAIRS } from './actions'

const reducer = (state = [], action) => {
  switch (action.type) {
    case ADD_PAIRS:
      return action.payload
    default:
      return state
  }
}

export default reducer
