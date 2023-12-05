import { createSlice } from "@reduxjs/toolkit"

const initialState = {
  page: 1,
  results: [],
  total_pages: 1,
  total_results: 0,
}

const moviesSlice = createSlice({
  name: "movies",
  initialState,
  reducers: {},
})

export default moviesSlice.reducer
