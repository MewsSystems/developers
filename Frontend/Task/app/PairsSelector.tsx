import * as React from 'react';
import { useCallback, useRef, useState } from 'react';
import { Currency } from './Currency';
import styled from 'styled-components';
import useOnClickOutside from 'use-onclickoutside';

const CheckboxInput = styled.input`
  margin-right: 10px;
`;

type CheckboxProps = {
  id: string;
  checked: boolean;
  togglePair: (id: string) => void;
};

const Checkbox = ({ id, checked, togglePair }: CheckboxProps) => {
  const click = useCallback(() => {
    togglePair(id);
  }, [id, togglePair]);

  return <CheckboxInput type="checkbox" checked={checked} onChange={click} />;
};

const Container = styled.div`
  position: relative;
`;

const Dropdown = styled.div`
  position: absolute;
  background: #fff;
  border: 1px solid #000;
  border-radius: 10px;
  border-top-left-radius: 0;
  overflow: hidden;
`;

const DropdownToggle = styled.button`
  background: #fff;
  padding: 10px;
  border: 1px solid #000;
  font-size: 14px;
`;

const Pair = styled.label`
  display: flex;
  align-items: center;
  padding: 10px;
  ${({ checked }: { checked: boolean }) => (checked ? 'background: #eee;' : '')}
  &:hover {
    background: #ddd;
  }
`;

const Codes = styled.div`
  font-weight: bold;
`;
const CurrencyName = styled.div`
  font-size: 10px;
`;

type Props = {
  pairs: ReadonlyArray<{
    id: string;
    currencies: [Currency, Currency];
    selected: boolean;
  }>;
  togglePair: (id: string) => void;
};

export const PairsSelector = ({ pairs, togglePair }: Props) => {
  const [dropdownOpen, setDropdownOpen] = useState(false);
  const toggleDropdown = useCallback(() => {
    setDropdownOpen(dropdownOpen => !dropdownOpen);
  }, []);
  const closeDropdown = useCallback(() => {
    setDropdownOpen(false);
  }, []);
  const containerRef = useRef(null);
  useOnClickOutside(containerRef, closeDropdown);

  return (
    <Container ref={containerRef}>
      <DropdownToggle onClick={toggleDropdown}>Select pairs...</DropdownToggle>
      {dropdownOpen && (
        <Dropdown>
          {pairs.map(pair => (
            <Pair key={pair.id} checked={pair.selected}>
              <Checkbox
                id={pair.id}
                checked={pair.selected}
                togglePair={togglePair}
              />
              <div>
                <Codes>
                  {pair.currencies[0].code} &ndash; {pair.currencies[1].code}
                </Codes>
                <CurrencyName>{pair.currencies[0].name}</CurrencyName>
                <CurrencyName>{pair.currencies[1].name}</CurrencyName>
              </div>
            </Pair>
          ))}
        </Dropdown>
      )}
    </Container>
  );
};
