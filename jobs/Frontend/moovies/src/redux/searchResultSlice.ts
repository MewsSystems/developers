import { createSlice, PayloadAction } from '@reduxjs/toolkit'
import type { RootState } from './store'
import { SearchResult } from '../components/SearchResultsList'

// Define a type for the slice state
interface SearchResultState {
    value: SearchResult
}

// Define the initial state using that type
const initialState: SearchResultState = {
    value: { data: [], loading: true, error: false }
}

export const searchResultSlice = createSlice({
    name: 'searchResult',
    initialState,
    reducers: {
        setSearchResult: (state, action: PayloadAction<SearchResult>) => {
            state.value = action.payload
        }
    }
})

export const { setSearchResult } = searchResultSlice.actions

export const selectSearchResult = (state: RootState) => state.searchResult.value

export default searchResultSlice.reducer
