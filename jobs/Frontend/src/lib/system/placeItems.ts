import { system } from '@tradersclub/styled-system';

export interface PlaceItemsProps {
  placeItems?: string
}

export const placeItems = system({
  placeItems: {
    property: 'placeItems',
  },
});
