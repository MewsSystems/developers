import { useEffect, useState } from "react";

export type ColorTheme = "light" | "dark";

export function usePrefersColorTheme() {
  const [colorTheme, setColorTheme] = useState<ColorTheme>("light");

  useEffect(() => {
    const darkThemeMq = window.matchMedia("(prefers-color-scheme: dark)");

    const handleChange = (e: MediaQueryListEvent) => {
      setColorTheme(e.matches ? "dark" : "light");
    };
    darkThemeMq.addEventListener("change", handleChange);

    setColorTheme(darkThemeMq.matches ? "dark" : "light");

    return () => darkThemeMq.removeEventListener("change", handleChange);
  }, []);

  return colorTheme;
}
