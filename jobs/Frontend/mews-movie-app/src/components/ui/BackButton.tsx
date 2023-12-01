import styled from "styled-components";
import { FiArrowLeft } from "react-icons/fi";
import { ComponentProps } from "react";

const Button = styled.button`
    display: inline-flex;
    gap: 0.3rem;
    border: none;
    background: none;
    color: var(--gray-600);
    font-size: 1rem;
    font-weight: 500;
    padding: 0;
    cursor: pointer;

    &:hover,
    &:focus {
        outline: none;
        text-decoration: underline;
        text-underline-offset: 0.25rem;
    }
`;

type Props = ComponentProps<"button">;

const BackButton = ({ children, ...props }: Props) => {
    return (
        <Button {...props}>
            <FiArrowLeft />
            {children}
        </Button>
    );
};

export default BackButton;
