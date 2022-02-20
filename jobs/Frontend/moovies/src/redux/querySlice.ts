import { createSlice, PayloadAction } from '@reduxjs/toolkit'
import type { RootState } from './store'

// Define a type for the slice state
interface QueryState {
    value: string
}

// Define the initial state using that type
const initialState: QueryState = {
    value: ''
}

export const querySlice = createSlice({
    name: 'query',
    initialState,
    reducers: {
        setQuery: (state, action: PayloadAction<string>) => {
            state.value = action.payload
        }
    }
})

export const { setQuery } = querySlice.actions

export const selectQuery = (state: RootState) => state.query.value

export default querySlice.reducer
