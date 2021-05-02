import React from "react";
import styled from "styled-components";

interface IButtonProps {
  onClick: () => void;
  content: string;
  variant: "primary" | "secondary";
  isDisabled: boolean;
}

export const Button: React.FC<IButtonProps> = (props) => {
  const { isDisabled, onClick, content, variant } = props;
  return (
    <ButtonWrapper onClick={onClick} isDisabled={isDisabled} variant={variant}>
      {content}
    </ButtonWrapper>
  );
};

const ButtonWrapper = styled.button<{ variant: string; isDisabled: boolean }>`
  &:hover {
    cursor: ${(props) => (props.isDisabled ? "not-allowed" : "pointer")};
    color: ${(props) => props.theme.color.blue};
    background: rgba(25, 118, 210, 0.04);
  }
  width: max-content;
  padding: 6px 8px;
  text-transform: uppercase;
  text-align: center;
  color: ${(props) =>
    props.isDisabled ? "rgba(255,255,255,0.5)" : props.theme.color.blue};
  display: flex;
  justify-content: center;
  white-space: nowrap;
  align-items: center;
  line-height: 1.75;
  background: ${(props) =>
    props.isDisabled ? "grey" : props.theme.color.white};
  border-radius: 4px;
`;
