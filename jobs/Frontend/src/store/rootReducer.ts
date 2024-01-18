/* Instruments */
import { TMDBApiSlice, SearchSlice } from '@/store/slices';

export const reducer = {
  [TMDBApiSlice.reducerPath]: TMDBApiSlice.reducer,
  [SearchSlice.reducerPath]: SearchSlice.reducer
};
