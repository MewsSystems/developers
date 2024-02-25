import React from 'react';
import Stack from '@mui/material/Stack';
import Chip from '@mui/material/Chip';

interface Props {
  collection: {
    id: string | number;
    name: string;
  }[];
}

export default function Button({ collection }: Props) {
  return (
    <Stack direction="row" spacing={1}>
      {collection.map((item) => {
        return (
          <Chip
            color="warning"
            label={item.name}
            key={item.id}
            size="small"
            variant="outlined"
            style={{
              fontWeight: 'bold',
            }}
          />
        );
      })}
    </Stack>
  );
}
