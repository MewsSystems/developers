import styled from 'styled-components';
import { ButtonType } from '../types';

const buttonStyles = ({ type = ButtonType.button }: { type?: ButtonType }) => {
  switch (type) {
    case ButtonType.button:
      return `
        background-color: #eee;
        color: #444;
        border: 1px solid #ccc;
      `;
    case ButtonType.link:
      return `
        background-color: white;
        color: black;
        border: 0;
      `;
  }
};


export const Input = styled.button<{ type?: ButtonType }>`
  font-size: 0.8em;
  padding: 0.25em 1em;
  border-radius: 3px;
  ${buttonStyles}
`;

export default Input;