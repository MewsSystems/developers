"use client";
import styled from "styled-components";

// Container for the no-movies message
const NoMoviesContainer = styled.div`
  background-color: #ecf0f1; // Light grey background for the message box
  color: #2c3e50; // Dark text for contrast
  padding: 30px 20px;
  border-radius: 8px;
  text-align: center;
  margin: 20px 0; // Spacing from other elements
`;

// Heading for the title/message
const MessageTitle = styled.h2`
  font-size: 24px;
  margin-bottom: 10px; // Space below the title
`;

// Text for additional information or guidance
const MessageText = styled.p`
  font-size: 16px;
  line-height: 1.5; // For better readability
`;

interface NoMoviesComponentProps {
  message: string;
  additionalText: string;
}

export default function NoMoviesComponent({
  message,
  additionalText,
}: NoMoviesComponentProps) {
  return (
    <NoMoviesContainer>
      <MessageTitle>{message}</MessageTitle>
      <MessageText>{additionalText}</MessageText>
    </NoMoviesContainer>
  );
}
