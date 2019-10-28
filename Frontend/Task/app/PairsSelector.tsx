import * as React from 'react';
import { useCallback } from 'react';

type CheckboxProps = {
  id: string;
  checked: boolean;
  togglePair: (id: string) => void;
};

const Checkbox = ({ id, checked, togglePair }: CheckboxProps) => {
  const click = useCallback(() => {
    togglePair(id);
  }, [id, togglePair]);

  return <input type="checkbox" checked={checked} onChange={click} />;
};

type Currency = {
  code: string;
  name: string;
};

type Props = {
  pairs: ReadonlyArray<{
    id: string;
    currencies: [Currency, Currency];
    selected: boolean;
  }>;
  togglePair: (id: string) => void;
};

export const PairsSelector = ({ pairs, togglePair }: Props) => (
  <ul>
    {pairs.map(pair => (
      <li key={pair.id}>
        <label>
          <Checkbox
            id={pair.id}
            checked={pair.selected}
            togglePair={togglePair}
          />
          <span title={pair.currencies[0].name}>{pair.currencies[0].code}</span>
          <span title={pair.currencies[1].name}>{pair.currencies[1].code}</span>
        </label>
      </li>
    ))}
  </ul>
);
