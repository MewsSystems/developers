import styled, { css } from 'styled-components';

const subColor = 'grey';
const mainColor = 'black';

const shrinkLabelStyles = css`
  top: 1rem;
  font-size: 12px;
  color: ${mainColor};
`;

type FormInputLabelProps = {
  shrink?: boolean;
};

export const FormInputLabel = styled.label<FormInputLabelProps>`
  color: ${subColor};
  font-size: 16px;
  font-weight: normal;
  position: absolute;
  pointer-events: none;
  left: 0;
  top: 2rem;
  transition: 300ms ease all;
  ${({ shrink }) => shrink && shrinkLabelStyles};
`;

export const Input = styled.input`
  background: none;
  background-color: white;
  color: ${subColor};
  font-size: 18px;
  padding: 10px 10px 10px 5px;
  display: block;
  width: 100%;
  border: none;
  border-radius: 0;
  border-bottom: 1px solid ${subColor};
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
