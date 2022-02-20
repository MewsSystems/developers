import { TypedUseSelectorHook, useDispatch, useSelector } from 'react-redux'
import type { RootState, AppDispatch } from '../redux/store'

export const useAppDispatch = () => useDispatch<AppDispatch>()
export const useAppSelector: TypedUseSelectorHook<RootState> = useSelector

// TODO remove
// https://redux.js.org/usage/usage-with-typescript#use-typed-hooks-in-components
// takhle to pouzivej
