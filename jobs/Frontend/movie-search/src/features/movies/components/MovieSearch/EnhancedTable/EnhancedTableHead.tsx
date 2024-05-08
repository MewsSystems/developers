import { Box, SxProps, TableCell, TableHead, TableRow, TableSortLabel, Theme } from '@mui/material';
import { useTranslation } from 'react-i18next';
import { SortingOrder } from '../../../../common/models/SortingOrder';
import { Movie } from '../../../models/Movie';

export interface EnhancedTableProps {
  order: SortingOrder;
  orderBy: string;
  rowCount: number;
}

const visuallyHidden: SxProps<Theme> = {
  border: 0,
  clip: 'rect(0 0 0 0)',
  height: '1px',
  margin: '-1px',
  overflow: 'hidden',
  padding: 0,
  position: 'absolute',
  whiteSpace: 'nowrap',
  width: '1px'
};

interface HeadCell {
  id: keyof Movie;
  label: string;
  numeric: boolean;
}

export default function EnhancedTableHead({ order, orderBy }: EnhancedTableProps) {
  const { t } = useTranslation();

  const headCells: readonly HeadCell[] = [
    {
      id: 'poster_path',
      label: t('movieDetails.poster'),
      numeric: false
    },
    {
      id: 'title',
      label: t('movieDetails.title'),
      numeric: false
    },
    {
      id: 'release_date',
      label: t('movieDetails.releaseDate'),
      numeric: false
    },
    {
      id: 'popularity',
      label: t('movieDetails.popularity'),
      numeric: true
    },
    {
      id: 'vote_average',
      label: t('movieDetails.voteAverage'),
      numeric: true
    },
    {
      id: 'vote_count',
      label: t('movieDetails.voteCount'),
      numeric: true
    },
    {
      id: 'original_language',
      label: t('movieDetails.originalLanguage'),
      numeric: false
    }
  ];

  return (
    <TableHead data-testid="movie-enhanced-table-head">
      <TableRow>
        {headCells.map(headCell => (
          <TableCell
            sx={{ p: 1 }}
            key={headCell.id}
            align={headCell.numeric ? 'right' : 'left'}
            sortDirection={orderBy === headCell.id ? order : false}>
            <TableSortLabel active={orderBy === headCell.id} direction={orderBy === headCell.id ? order : 'asc'}>
              {headCell.label}
              {orderBy === headCell.id ? (
                <Box component="span" sx={visuallyHidden}>
                  {order === 'desc' ? 'sorted descending' : 'sorted ascending'}
                </Box>
              ) : null}
            </TableSortLabel>
          </TableCell>
        ))}
      </TableRow>
    </TableHead>
  );
}
