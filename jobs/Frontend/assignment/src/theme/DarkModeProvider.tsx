import { usePrefersColorTheme } from "@/hooks";
import { PropsWithChildren, createContext, useEffect, useState } from "react";

type DarkModeContextType = {
  darkMode: boolean;
  toggleDarkMode: () => void;
  setDarkMode: (value: boolean) => void;
};

export const DarkModeContext = createContext<DarkModeContextType>({
  darkMode: false,
  setDarkMode: () => {},
  toggleDarkMode: () => {},
});

export function DarkModeProvider({ children }: PropsWithChildren<{}>) {
  const userPrefersDark = usePrefersColorTheme() === "dark";
  const [darkMode, setDarkMode] = useState(userPrefersDark);

  useEffect(() => {
    setDarkMode(userPrefersDark);
  }, [userPrefersDark]);

  useEffect(() => {
    document.body.classList.add(".theme-switch");
    document.body.classList.remove(".theme-switch");
  }, [darkMode]);

  const toggleDarkMode = () => setDarkMode(prev => !prev);

  return (
    <DarkModeContext.Provider value={{ darkMode, setDarkMode, toggleDarkMode }}>
      {children}
    </DarkModeContext.Provider>
  );
}
