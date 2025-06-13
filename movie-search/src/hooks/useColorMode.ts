import { useEffect, useState } from 'react';
import {ThemeType} from "../types/ThemeType.ts";

const THEME_KEY = 'preferred-theme';

export const  useColorMode = (): [ThemeType, () => void] => {
    const getInitialTheme = (): ThemeType => {
        if (typeof window === 'undefined') return 'light';

        const stored = localStorage.getItem(THEME_KEY) as ThemeType | null;
        if (stored) return stored;

        const prefersDark = window.matchMedia('(prefers-color-scheme: dark)').matches;
        return prefersDark ? 'dark' : 'light';
    };

    const [theme, setTheme] = useState<ThemeType>(getInitialTheme);

    useEffect(() => {
        document.documentElement.setAttribute('data-theme', theme);
        localStorage.setItem(THEME_KEY, theme);
    }, [theme]);

    const toggleTheme = () => {
        setTheme(prev => (prev === 'dark' ? 'light' : 'dark'));
    };

    return [theme, toggleTheme];
}
