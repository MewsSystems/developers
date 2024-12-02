"use client";
import {
  MessageText,
  MessageTitle,
  NoMoviesContainer,
} from "./NoMoviesStyledComponents";

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
