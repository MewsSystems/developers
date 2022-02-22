import { createSlice, PayloadAction } from '@reduxjs/toolkit'
import type { RootState } from './store'

// Define a type for the slice state
interface CurrentPageState {
    value: number
}

// Define the initial state using that type
const initialState: CurrentPageState = {
    value: 1
}

export const currentPageSlice = createSlice({
    name: 'currentPage',
    initialState,
    reducers: {
        setCurrentPage: (state, action: PayloadAction<number>) => {
            state.value = action.payload
        },
        incrementCurrentPage: (state, action: PayloadAction<number>) => {
            state.value += action.payload
        },
        decrementCurrentPage: (state, action: PayloadAction<number>) => {
            state.value -= action.payload
        }
    }
})

export const { setCurrentPage, incrementCurrentPage, decrementCurrentPage } = currentPageSlice.actions

export const selectCurrentPage = (state: RootState) => state.currentPage.value

export default currentPageSlice.reducer
