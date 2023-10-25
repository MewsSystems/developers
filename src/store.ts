import { configureStore } from '@reduxjs/toolkit';

import searchReducer from './reducers/searchReducer';
import thunkMiddleware from './middlewares/thunkMiddleware';

const store = configureStore({
  reducer: {
    search: searchReducer,
  },
  middleware: (getDefaultMiddleware) => getDefaultMiddleware().concat(thunkMiddleware),
});

export default store;
