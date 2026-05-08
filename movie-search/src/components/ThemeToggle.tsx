import styled from 'styled-components';
import {ThemeType} from "../types/ThemeType.ts";

type ThemeToggleProps = {
    theme: ThemeType,
    toggleTheme: () => void,
}

export const ThemeToggle = ({theme, toggleTheme}: ThemeToggleProps) => {
    return (
        <Container>
            <ToggleButton
                onClick={toggleTheme}
                aria-label={`Switch to ${theme === 'light' ? 'dark' : 'light'} mode`}
                title={`Switch to ${theme === 'light' ? 'dark' : 'light'} mode`}
            >
                {theme === 'light' ? '🌙' : '🌞'}
            </ToggleButton>
        </Container>
    );
};

const Container = styled.div`
    padding: ${({theme}) => theme.spacing.xs};
    margin: auto;
    
    max-width: ${({theme}) => theme.layout.containerWidth};

    text-align: right;
`

const ToggleButton = styled.button`
    padding: ${({theme}) => theme.spacing.sm};

    font-size: ${({theme}) => theme.fontSizes.xl};
    color: ${({theme}) => theme.colors.text};

    background: ${({theme}) => theme.colors.background};
    border: none;
    border-radius: ${({theme}) => theme.radii.full};
    box-shadow: ${({theme}) => theme.shadows.sm};

    cursor: pointer;
    transition: background 0.3s ease, transform 0.2s;

    &:hover {
        transform: scale(1.1);
        background: ${({theme}) => theme.colors.backgroundAlt};
    }

    &:focus-visible {
        outline: 2px solid ${({theme}) => theme.colors.focus};
    }
`;
