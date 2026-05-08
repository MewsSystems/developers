type Theme = 'light' | 'dark';

type ThemeState = {
  theme: Theme;
  toggleTheme: () => void;
  setTheme: (theme: Theme) => void;
};

type InputSearchMovieState = {
  inputSearchMovie: string;
  setInputSearchMovie: (value: string) => void;
};

export type { ThemeState, InputSearchMovieState };
