// ESLint rule disabled here because this is the designated place
// for importing and re-exporting the typed versions of hooks.
/* eslint-disable @typescript-eslint/no-restricted-imports */
import { useDispatch, useSelector } from "react-redux"
import type { AppDispatch, RootState } from "./store"

export const useAppDispatch = useDispatch.withTypes<AppDispatch>()
export const useAppSelector = useSelector.withTypes<RootState>()
