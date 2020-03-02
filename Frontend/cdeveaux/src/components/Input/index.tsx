import React from 'react';
import styled from 'styled-components'

import constants from 'cssConstants';

const Wrapper = styled.div`
  position: relative;
  width: 40vw;
  margin: 0 auto;
`;
const Border = styled.div`
  height: 5px;
  width: 100%;
  background: ${constants.ACCENT};
  transition-property: transform, opacity;
  transition-timing-funciton: ease-in-out;
  transition-duration: 100ms;
  opacity: 0.5;
`;
const Input = styled.input`
  box-sizing: border-box;
  background: transparent;
  border: none;
  color: ${constants.ACCENT};
  padding: 1rem 3rem 0.75rem;
  font-weight: bold;
  width: 100%;
  font-size: 2rem;
  text-transform: capitalize;
  transition: opacity ease-in-out 100ms;
  background-image: url(${process.env.PUBLIC_URL}/assets/search.svg);
  background-repeat: no-repeat;
  background-size: 2rem;
  background-position: left center;
  opacity: 0.5;

  &:valid,
  &:focus {
    outline: none;
    opacity: 1;
  }
  &:valid ~ ${Border} {
    opacity: 1;

  }
  &:focus ~ ${Border} {
    opacity: 1;
    transform: scale(1.2, 1);
  }
`;

type Props = {
  type: string,
  onChange: (e: React.ChangeEvent<HTMLInputElement>) => void,
  value: string,
  required: boolean,
  autoFocus: boolean,
  placeholder?: string,
  className?: string,
};
const InputWithBorder = ({ className, ...props}: Props) => (
  <Wrapper className={className}>
    <Input {...props} />
    <Border />
  </Wrapper>
);

export default InputWithBorder;
