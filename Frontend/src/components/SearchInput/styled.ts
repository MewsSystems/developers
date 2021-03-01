import styled from 'styled-components';
import Button from '../common/Button';

export const Input = styled.input`
  text-align: center;
  border: none;
  outline: none;
  width: inherit;
  background-color: inherit;
  font-size: ${(props) => props.theme.fontSizes.l};
`;

export interface InputContainerProps {
  maxWidth?: string;
}

export const InputContainer = styled.div<InputContainerProps>`
  max-width: ${(props) => props.maxWidth || 'none'};
  width: 100%;
  position: relative;
  border: ${(props) => props.theme.borders.thin};
  border-color: ${(props) => props.theme.colors.secondaryDark};
  display: flex;
  align-items: center;
  background-color: ${(props) => props.theme.colors.background};
  font-size: ${(props) => props.theme.fontSizes.l};
  color: ${(props) => props.theme.colors.text};

  :focus-within {
    border-color: ${(props) => props.theme.colors.primaryLight};
  }
`;

export const InputIconContainer = styled.div`
  display: flex;
  align-items: center;
  padding: ${(props) => props.theme.space[3]};
`;

export const InputClearButton = styled(Button).attrs({
  variant: 'secondary',
})`
  border: none;
  background-color: inherit !important;
  color: ${(props) => props.theme.colors.secondaryDark};
  padding: ${(props) => props.theme.space[3]};

  &:hover:not(:disabled),
  &:active:not(:disabled),
  &:focus {
    color: inherit;
  }
`;
