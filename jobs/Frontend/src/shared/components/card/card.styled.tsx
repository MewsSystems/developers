import Grid from '@mui/material/Grid';
import styled from 'styled-components';

export const StyledGrid = styled(Grid)({
  cursor: 'pointer',
  transition: 'box-shadow 0.3s',
  '&:hover': {
    boxShadow: '0 4px 8px rgba(0, 0, 0, 0.1)',
  },
});

export const StyledImage = styled.img`
  margin-right: 16px;
  width: 140px;
  height: 190px;
  object-fit: cover;
`;
