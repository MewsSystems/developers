// @flow strict

import * as React from 'react';
import styled from 'styled-components';

import Checkbox from '../components/Checkbox';

type Props = {|
  +checked: boolean,
  +onChange: (e: SyntheticInputEvent<HTMLInputElement>) => void,
  +children: string,
|};

const Container = styled.div`
  padding-bottom: 8px;
`;

const CurrencyPairInput = ({ checked, onChange, children }: Props) => (
  <Container>
    <Checkbox onChange={onChange} label={children} checked={checked} />
  </Container>
);

export default CurrencyPairInput;
