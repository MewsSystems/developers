import React from "react";
import { ButtonProps } from "./types";
import { StyledButton } from "./style";

export const Button: React.FC<ButtonProps> = ({ label, handleLoadMore}) => {

    return (
        <StyledButton 
            className="button"
            onClick={handleLoadMore}
        >
            {label}
        </StyledButton>
    )
}