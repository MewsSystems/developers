// @flow strict

import * as React from 'react';
import styled from 'styled-components';
import { FaDollarSign } from 'react-icons/fa';

import { SIZES, COLORS } from '../utils/constants';

type Props = {|
  +checked: boolean,
  +label: string,
  +size?: $Keys<typeof SIZES>,
  onChange: (e: SyntheticInputEvent<HTMLInputElement>) => void,
  +value?: string,
|};

const IconContainer = styled.div`
  position: relative;
  box-sizing: border-box;
  flex: 0 0 auto;
  display: flex;
  align-items: center;
  justify-content: center;
  background-color: ${COLORS.BAKCGROUND};
  height: 16px;
  width: 16px;
  border-radius: 3px;
  transform: scale(1);
  transition: all 0.2s ease-in-out;

  & > svg {
    visibility: hidden;
    display: flex;
    align-items: center;
    justify-content: center;
    width: 14px;
    height: 14px;
  }
`;

const TextContainer = styled.span`
  color: ${COLORS.DEFAULT};
  font-size: ${({ size }) => SIZES[size]};
  padding-left: 4px;
  vertical-align: middle;
`;

const Label = styled.label`
  display: flex;
  width: 100%;
  flex-direction: row;
  align-items: center;
  position: relative;
  cursor: pointer;

  ${IconContainer} {
    color: ${COLORS.CHECKBOX_ICON};
    border: 1px solid ${COLORS.CHECKBOX_BORDER};
  }

  &:hover ${IconContainer}, &:focus ${IconContainer} {
    border-color: ${COLORS.CHECKBOX_BORDER_HOVER};
  }
`;

const Input = styled.input.attrs({
  type: 'checkbox',
})`
  opacity: 0;
  position: absolute;
  z-index: -1;

  &:checked + ${IconContainer} > svg {
    visibility: visible;
  }

  &:checked ~ ${TextContainer} {
    font-weight: bold;
  }
`;

const Checkbox = ({ checked, label, size = 'normal', onChange, value }: Props) => (
  <Label>
    <Input checked={checked} onChange={onChange} value={value} />
    <IconContainer>
      <FaDollarSign />
    </IconContainer>
    {label && <TextContainer size={size}>{label}</TextContainer>}
  </Label>
);

export default Checkbox;
