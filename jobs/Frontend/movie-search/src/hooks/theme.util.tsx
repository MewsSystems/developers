"use client";

import { Moon, Sun } from "lucide-react";
import { useState, useEffect } from "react"
import styled from "styled-components";

const ToggleTheme = styled.button`
    background-color: none;
    background: none;
    border: none;
`;

type Themes = 'dark' | 'light';

export default function SetTheme() {
    const [theme, setTheme] = useState<Themes>()

    const toggleTheme = () => {
        if (theme == 'light') {
            setTheme('dark')
            document.documentElement.setAttribute('data-theme', 'dark')
        } else if (theme == 'dark') {
            setTheme('light')
            document.documentElement.setAttribute('data-theme', 'light')
        }
    }

    const defaultTheme = () => {
        const themeLocalStorage = localStorage.getItem('theme') as Themes;
        const themeSystem = window.matchMedia('(prefers-color-scheme: dark)').matches ? 'dark' : 'light';

        return (themeLocalStorage ?? themeSystem)
    }

    useEffect(() => {
        if (!theme) return setTheme(defaultTheme());

        const watchSysTheme = window.matchMedia('(prefers-color-scheme: dark)');

        watchSysTheme.addEventListener('change', event => {
            const newColorScheme = event.matches ? "dark" : "light";

            document.documentElement.setAttribute('data-theme', newColorScheme)
            localStorage.setItem('theme', (newColorScheme))

            setTheme(newColorScheme);
        });

        return () => {
            watchSysTheme.addEventListener('change', event => {
                const newColorScheme = event.matches ? "dark" : "light";

                document.documentElement.setAttribute('data-theme', newColorScheme)
                localStorage.setItem('theme', (newColorScheme))

                setTheme(newColorScheme);
            });
        }
    }, [theme])

    return (
        <ToggleTheme
            key="themeToggle"
            id="themeToggle"
            onClick={toggleTheme}
            data-theme={theme}
            aria-label={`Change theme to ${theme}`}
        >
            {theme === 'dark' ? <Moon /> : <Sun />}
        </ToggleTheme>
    )
}