import React from "react";
import styled, { css } from "styled-components";

interface IButtonProps {
  onClick: () => void;
  content: string;
  variant: "primary" | "secondary" | "active";
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
  border: ${({ theme }) => `1px solid ${theme.color.blue}`}
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
  &:hover {
    cursor: ${(props) => (props.isDisabled ? "not-allowed" : "pointer")};
    color: ${(props) => props.theme.color.blue};
    background: rgba(25, 118, 210, 0.04);
  }
  ${({ variant }) => {
    if (variant === "active") {
      return activeButtonStyle;
    }
    if (variant === "secondary") {
      return secondaryButtonStyle;
    }
  }}
`;

const activeButtonStyle = css`
  background: ${(props) => props.theme.color.darkBlue};
  color: ${(props) => props.theme.color.grey};
  width: 32px;
  height: 32px;
  &:hover {
    color: ${(props) => props.theme.color.blue};
    background: rgba(25, 118, 210, 0.04);
    border: ${({ theme }) => `1px solid ${theme.color.darkBlue}`};
  }
`;
const secondaryButtonStyle = css`
  width: 32px;
  height: 32px;
  margin: 0 3px;
  border: ${({ theme }) => `1px solid ${theme.color.blue}`};
  background: ${(props) => props.theme.color.white};
  color: ${(props) => props.theme.color.blue};
  &:hover {
    color: ${(props) => props.theme.color.darkBlue};
    border: ${({ theme }) => `1px solid ${theme.color.darkBlue}`};
  }
`;
