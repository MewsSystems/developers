import styled from 'styled-components';
import { FontSize } from '../types';

export const Input = styled.input<{ size?: FontSize }>`
  font-size: ${props => props.size || FontSize.small};
  border-radius: 15px;
  margin: auto;
  display:block;
  width: calc(80%);
  padding:5px;
  padding-left:15px;
  border: 1px solid #ccc;
  color: #888;

  &:focus{
    outline: none;
    box-shadow: 0 0 10px #fd9;
  }
`;

export default Input;