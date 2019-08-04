// @flow strict

import * as React from 'react';
import styled from 'styled-components';

import Checkbox from '../components/Checkbox';

type Props = {|
  +checked: boolean,
  +onChange: (e: SyntheticInputEvent<HTMLInputElement>) => void,
  +label: string,
|};

const Container = styled.div`
  padding-bottom: 8px;
`;

const CurrencyPairInput = ({ checked, onChange, label }: Props) => (
  <Container>
    <Checkbox onChange={onChange} label={label} checked={checked} />
  </Container>
);

export default React.memo<Props>(CurrencyPairInput);
