import { AnyAction } from 'redux'
import { useDispatch } from 'react-redux'
import { ThunkDispatch } from 'redux-thunk'
import { State } from 'state/rootReducer'

export const useThunkDispatch = () =>
  useDispatch<ThunkDispatch<State, void, AnyAction>>()
