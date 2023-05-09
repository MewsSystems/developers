// Import the React library and the styled component ErrorTextWrapper from ErrorText.styled file
import React from "react";
import { ErrorTextWrapper } from "./ErrorText.styled";

// Define the props that the component can receive
interface ErrorTextProps {
    message: string;
}

// Define a functional component that receives the props as an object destructured argument
const ErrorText: React.FC<ErrorTextProps> = ({ message }) => {
    // Render the component with the ErrorTextWrapper component and the message from the props
    return (
        <ErrorTextWrapper>
            {message}
        </ErrorTextWrapper>
    );
};

// Export the component as the default export
export default ErrorText;
