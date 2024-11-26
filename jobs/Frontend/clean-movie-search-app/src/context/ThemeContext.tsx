import React, {
  createContext,
  useContext,
  useState,
  useCallback,
  useMemo,
} from 'react';
import { ThemeProvider as StyledThemeProvider } from 'styled-components';
import { Theme } from '../theme/types';
import { lightTheme, darkTheme } from '../theme/themes';

interface ThemeContextType {
  theme: Theme;
  toggleTheme: () => void;
}

// Context creation for the global theme
const ThemeContext = createContext<ThemeContextType | undefined>(undefined);

export const ThemeProvider: React.FC<{ children: React.ReactNode }> = ({
  children,
}) => {
  // theme state and toggle function - TODO: could be named selectedTheme and setSelelectedTheme
  const [theme, setTheme] = useState<Theme>(darkTheme);

  const toggleTheme = useCallback(() => {
    setTheme((current) => (current.name === 'light' ? darkTheme : lightTheme));
  }, []);

  // Only recreate the theme on theme toggle and not on every render
  const value = useMemo(() => ({ theme, toggleTheme }), [theme, toggleTheme]);

  return (
    <ThemeContext.Provider value={value}>
      <StyledThemeProvider theme={theme}>{children}</StyledThemeProvider>
    </ThemeContext.Provider>
  );
};

export const useTheme = () => {
  const context = useContext(ThemeContext);
  if (context === undefined) {
    throw new Error('useTheme must be used within the ThemeProvider');
  }
  return context;
};
