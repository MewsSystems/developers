import styled, { css } from 'styled-components';
import { ColorsTheme } from '../../assets/colors/theme-colors/colors.ts';

const mainColor = ColorsTheme.primary;

const shrinkLabelStyles = css`
  top: 1rem;
  font-size: 12px;
  color: ${mainColor};
`;

type FormInputLabelProps = {
  searchquery?: string;
};

export const FormInputLabel = styled.label<FormInputLabelProps>`
  color: ${mainColor};
  font-size: 16px;
  font-weight: normal;
  position: absolute;
  pointer-events: none;
  left: 0;
  top: 2rem;
  transition: 300ms ease all;
  ${({ searchquery }) => !!searchquery && shrinkLabelStyles};
`;

export const Input = styled.input`
  background: none;
  background-color: ${ColorsTheme.secondary};
  color: ${mainColor};
  font-size: 18px;
  padding: 10px 10px 10px 5px;
  display: block;
  width: 100%;
  border: none;
  border-radius: 0;
  border-bottom: 1px solid ${mainColor};
  margin: 25px 0;

  &:focus {
    outline: none;
  }

  &:focus ~ ${FormInputLabel} {
    ${shrinkLabelStyles};
  }
`;

export const Group = styled.div`
  position: relative;
`;
