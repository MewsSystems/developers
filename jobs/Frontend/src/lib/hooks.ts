/* eslint-disable no-restricted-imports */
import {
  useDispatch as useReduxDispatch,
  useSelector as useReduxSelector,
  useStore as useReduxStore,
} from 'react-redux'
/* eslint-enable no-restricted-imports */

import type { TypedUseSelectorHook } from 'react-redux'
import type { RootState, AppDispatch, AppStore } from './store'

// Use throughout your app instead of plain `useDispatch`, ...
export const useDispatch: () => AppDispatch = useReduxDispatch
export const useSelector: TypedUseSelectorHook<RootState> = useReduxSelector
export const useStore: () => AppStore = useReduxStore
