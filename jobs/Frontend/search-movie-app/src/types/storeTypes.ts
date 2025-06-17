type Theme = 'light' | 'dark';

export type ThemeState = {
  theme: Theme;
  toggleTheme: () => void;
  setTheme: (theme: Theme) => void;
};

export type InputSearchMovieState = {
  inputSearchMovie: string;
  setInputSearchMovie: (value: string) => void;
};
