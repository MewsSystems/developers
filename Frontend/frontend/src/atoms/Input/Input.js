import styled from 'styled-components';

import { getSizeInput, } from './utils';


const Input = styled.input`
  display: block;
  box-sizing: border-box;
  -webkit-box-sizing: border-box;
  padding: 0.25em 0.5em;
  width: 100%;
  
  background: ${(p) => p.theme.white};
  line-height: 1.5;
  background-clip: padding-box;
  transition: all 0.3s ease;

  border-radius: ${(p) => p.theme.input.borderRadius};
  border-color: ${(p) => p.theme.grey.t500};
  border-style: ${(p) => p.theme.input.borderStyle};

  border-top-width: ${(p) => p.theme.input.borderWidthTopBottom};
  border-bottom-width: ${(p) => p.theme.input.borderWidthTopBottom};
  border-right-width: ${(p) => p.theme.input.borderWidthLeftRight};
  border-left-width: ${(p) => p.theme.input.borderWidthLeftRight};

  ${getSizeInput}

  &::placeholder {
    color: ${(p) => p.theme.grey.t400};
    opacity: 1; /* Firefox */
  }
  &:-ms-input-placeholder { /* Internet Explorer 10-11 */
    color: ${(p) => p.theme.grey.t400};
  }
  &:-ms-input-placeholder { /* Microsoft Edge */
    color: ${(p) => p.theme.grey.t400};;
  }

  &:focus {
    outline: unset;
  }
  &:disabled {
    background: ${(p) => p.theme.grey.t100};
  }
`;

export default Input;
