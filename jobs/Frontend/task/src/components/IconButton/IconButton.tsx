import { FC } from "react";
import { IconButtonProps } from "src/components/IconButton/IconButtonProps";
import styled from "styled-components";

export const IconButton: FC<IconButtonProps> = (props) => {
  return (
    <Button onClick={props.onClick} disabled={props.disabled}>
      {props.name}
    </Button>
  );
};

const Button = styled.button`
  background: none;
  border: none;
  padding: 4px 12px;
  margin: 0;
  cursor: ${({ disabled }) => (disabled ? "not-allowed" : "pointer")};
  opacity: ${({ disabled }) => (disabled ? "0.3" : "1")};
`;
