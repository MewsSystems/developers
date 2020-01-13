import styled from 'styled-components';

import { getSizeBtn, } from './utils';


const ButtonIcon = styled.button`
  margin: 0;

  display: inline-block;
  text-align: center;
  transition: color 0.15s ease-in-out, background-color 0.15s ease-in-out, border-color 0.15s ease-in-out, box-shadow 0.15s ease-in-out;
  outline: 0;
  outline-offset: 0;
  user-select: none;
  cursor: pointer;

  text-transform: none;
  line-height: 1;
  font-weight: 400;
  line-height: 1.5;
  padding: 0.5em 0.9em;

  border-radius: ${(p) => p.theme.input.borderRadius}; 
  border-width: 1px;
  border-style: solid;
  border-color: transparent;

  color: ${(p) => p.theme.white};
  background: ${(p) => p.theme.grey.t700};
  border-color: ${(p) => p.theme.grey.t700};

  ${getSizeBtn}

  &:focus:not([disabled]) {
  }

  &:hover:not([disabled]) , &:active:not([disabled]) {
    background: ${(p) => p.theme.grey.t800};
  }
  
  &:disabled {
    cursor: default;
    color: ${(p) => p.theme.white};
    background: ${(p) => p.theme.grey.t400};
    border-color: transparent;
  }
`;


export default ButtonIcon;
