import { AnyAction } from 'redux'
import createMockStore from 'redux-mock-store'
import thunk, { ThunkDispatch } from 'redux-thunk'
import { State } from 'state/rootReducer'

export const mockStore = createMockStore<
  State,
  ThunkDispatch<State, void, AnyAction>
>([thunk])
