import { Filters } from '../../types'
import { PayloadAction, createSlice } from '@reduxjs/toolkit'

const initialState: Filters = {
  query: '',
  page: '1',
}

const filterSlice = createSlice({
  name: 'filter',
  initialState,
  reducers: {
    setQuery: (state, action: PayloadAction<string>) => {
      state.query = action.payload
    },
    setPage: (state, action: PayloadAction<string>) => {
      state.page = action.payload
    },
  },
})

export const { setQuery, setPage } = filterSlice.actions
export default filterSlice.reducer
