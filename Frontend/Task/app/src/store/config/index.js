import { LOAD_CONFIG } from './actions'

const reducer = (state = {}, action) => {
  switch (action.type) {
    case LOAD_CONFIG:
      return action.payload
    default:
      return state
  }
}

export default reducer
