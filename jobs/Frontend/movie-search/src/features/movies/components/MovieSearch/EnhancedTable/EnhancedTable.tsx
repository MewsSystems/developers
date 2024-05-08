import { Paper, Table, TableBody, TableCell, TableContainer, TableRow } from '@mui/material';
import {} from 'i18next';
import { useMemo, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useNavigate } from 'react-router-dom';
import AppRoutes from '../../../../../configs/appRoutes';
import { ENDPOINT_URL_IMAGES_w92 } from '../../../../../configs/config';
import { getComparator } from '../../../../common/helpers/tableSorting';
import { SortingOrder } from '../../../../common/models/SortingOrder';
import { Movie } from '../../../models/Movie';
import EnhancedTableHead from './EnhancedTableHead';

interface EnhancedTableProps {
  rows: Movie[];
}

export default function EnhancedTable({ rows }: EnhancedTableProps) {
  const { t } = useTranslation();
  const navigate = useNavigate();

  const [order] = useState<SortingOrder>('desc');
  const [orderBy] = useState<keyof Movie>('popularity');

  const handleClick = (id: number) => navigate(`${AppRoutes.Movie}/${id}`);

  const sortedRows = useMemo(() => rows.sort(getComparator(order, orderBy)), [order, orderBy, rows]);

  return (
    <Paper sx={{ m: 2, borderRadius: 2, overflowY: 'auto' }} elevation={3}>
      <TableContainer data-testid="movie-enhanced-table" sx={{ height: 'calc(100svh - 28rem)' }}>
        <Table stickyHeader aria-labelledby="movie-search-table" size="small">
          <EnhancedTableHead order={order} orderBy={orderBy} rowCount={rows.length} />
          <TableBody>
            {sortedRows.map(row => {
              return (
                <TableRow
                  hover
                  onClick={() => handleClick(row.id)}
                  role="checkbox"
                  tabIndex={-1}
                  key={row.id}
                  sx={{ cursor: 'pointer', height: 120 }}>
                  <TableCell width={100} scope="row">
                    <img
                      style={{ boxShadow: '2px 2px 3px rgba(0, 0, 0, 0.3)' }}
                      height={100}
                      width={68}
                      src={ENDPOINT_URL_IMAGES_w92 + row.poster_path}
                      alt={`${row.title} ${t('movieDetails.poster')}`}
                    />
                  </TableCell>
                  <TableCell sx={{ color: 'primary.main' }} width={240} align="left">
                    {row.title}
                  </TableCell>
                  <TableCell width={155} align="left">
                    {row.release_date.substring(0, 4)}
                  </TableCell>
                  <TableCell width={140} align="right">
                    {row.popularity}
                  </TableCell>
                  <TableCell width={155} align="right">
                    {`${Math.round(row.vote_average)}/10`}
                  </TableCell>
                  <TableCell width={140} align="right">
                    {row.vote_count}
                  </TableCell>
                  <TableCell sx={{ minWidth: 185 }} align="center">
                    {row.original_language.toUpperCase()}
                  </TableCell>
                </TableRow>
              );
            })}
          </TableBody>
        </Table>
      </TableContainer>
    </Paper>
  );
}
