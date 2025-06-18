import { create } from 'zustand';
import { persist } from 'zustand/middleware';
import type { ThemeState } from '../types/storeTypes';
import { DARK_THEME, LIGHT_THEME } from '../constants';

export const useThemeStore = create<ThemeState>()(
  persist(
    set => ({
      theme: LIGHT_THEME,
      toggleTheme: () =>
        set(state => ({ theme: state.theme === LIGHT_THEME ? DARK_THEME : LIGHT_THEME })),
      setTheme: theme => set({ theme }),
    }),
    {
      name: 'theme-storage',
    }
  )
);
