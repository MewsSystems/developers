import "./button.style.css";
import React from "react";

type ButtonProps = {
    label: string;
    handleLoadMore?: () => void; // optional function with no arguments and no return
};

export const Button: React.FC<ButtonProps> = ({ label, handleLoadMore}) => {

    return (
        <button 
            className="button"
            onClick={handleLoadMore}
        >
            {label}
        </button>
    )
}