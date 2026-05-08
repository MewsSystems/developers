import { create } from 'zustand';
import type { InputSearchMovieState } from '../types';

export const useInputSearchMovie = create<InputSearchMovieState>(set => ({
  inputSearchMovie: '',
  setInputSearchMovie: inputSearchMovie => set({ inputSearchMovie }),
}));
