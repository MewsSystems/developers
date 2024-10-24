import styled, { css } from 'styled-components';
import { ColorsTheme } from '../../assets/colors/theme-colors/colors.ts';

const mainColor = ColorsTheme.primary;

const shrinkLabelStyles = css`
  top: 0.5rem;
  font-size: 12px;
  color: ${mainColor};
`;

type FormInputLabelProps = {
  shrink: string;
};

const StyledLabel = styled.label<FormInputLabelProps>`
  color: ${mainColor};
  font-size: 16px;
  font-weight: normal;
  position: absolute;
  pointer-events: none;
  left: 0.1rem;
  top: 2rem;
  transition: 300ms ease all;
  ${({ shrink }) => !!shrink && shrinkLabelStyles};
`;

const StyledInput = styled.input`
  background: none;
  background-color: ${ColorsTheme.secondary};
  color: ${mainColor};
  font-size: 18px;
  padding: 10px 10px 10px 5px;
  display: block;
  width: 10rem;
  border: none;
  border-radius: 0;
  border-bottom: 1px solid ${mainColor};
  margin: 1rem 0;

  &:focus {
    outline: none;
  }

  &:focus ~ ${StyledLabel} {
    ${shrinkLabelStyles};
  }
`;

const Group = styled.div`
  position: relative;
`;

export { StyledInput, StyledLabel, Group };
