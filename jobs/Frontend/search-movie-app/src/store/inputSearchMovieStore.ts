import { create } from 'zustand';
import type { InputSearchMovieState } from '../types/storeTypes';

const useInputSearchMovie = create<InputSearchMovieState>(set => ({
  inputSearchMovie: '',
  setInputSearchMovie: inputSearchMovie => set({ inputSearchMovie }),
}));

export { useInputSearchMovie };
