import styled, { css } from 'styled-components';
import { ButtonProps } from './Button';

const StyledButton = styled.button<ButtonProps>`
    background-color: transparent;
    outline: none;
    border: transparent;
    border-radius: 4px;
    cursor: pointer;
    padding: 8px 20px;
    font-family: Inter, system-ui, Avenir, Helvetica, Arial, sans-serif;
    font-size: 12px;
    font-weight: bold;
    text-transform: uppercase;
    transition: all 200ms;

    &:disabled {
        opacity: 30%;
        pointer-events: none;
    }

    ${(props) =>
        props.variant === 'primary' &&
        css`
            background-color: ${props.theme.colors.primary};
            border: 1.5px solid transparent;
            color: ${props.theme.colors.btnTextLight};

            &:hover {
                background-color: transparent;
                border: 1.5px solid ${props.theme.colors.primary};
                color: ${props.theme.colors.primary};
            }
        `}

    ${(props) =>
        props.variant === 'secondary' &&
        css`
            background-color: transparent;
            border: 1.5px solid ${props.theme.colors.secondary};
            color: ${props.theme.colors.secondary};

            &:hover {
                background-color: ${props.theme.colors.secondary};
                border: 1.5px solid ${props.theme.colors.secondary};
                color: ${props.theme.colors.btnTextLight};
            }
        `}

    ${(props) =>
        props.variant === 'icon' &&
        css`
            background-color: transparent;
            stroke: ${props.theme.colors.primary};
            transition: all 200ms;

            svg {
                width: 16px;
                height: 16px;
            }

            &:hover {
                transform: scale(1.2);
            }
        `}

    ${(props) =>
        props.size === 'small' &&
        css`
            padding: 6px 18px;
            font-size: 11px;
        `}
`;

StyledButton.defaultProps = {
    variant: 'primary',
    size: 'normal',
};

export default StyledButton;
