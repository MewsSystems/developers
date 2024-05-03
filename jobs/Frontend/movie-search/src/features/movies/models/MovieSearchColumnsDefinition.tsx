import { GridColDef } from '@mui/x-data-grid';
import { t } from 'i18next';
import { ENDPOINT_URL_IMAGES_w92 } from '../../../configs/config';
import { Movie } from './Movie';

export const movieSearchColumnsDefinition: GridColDef<Movie>[] = [
  {
    field: 'poster_path',
    headerName: t('movieDetails.poster'),
    sortable: false,
    width: 100,
    renderCell: params => (
      <img height={100} src={ENDPOINT_URL_IMAGES_w92 + params.value} alt={t('movieDetails.poster')} />
    )
  },
  {
    field: 'title',
    headerName: t('movieDetails.title'),
    width: 240
  },
  {
    field: 'release_date',
    headerName: t('movieDetails.releaseDate'),
    width: 155,
    valueFormatter: (value: Movie['release_date']) => value.substring(0, 4)
  },
  {
    field: 'popularity',
    headerName: t('movieDetails.popularity'),
    type: 'number',
    width: 140
  },
  {
    field: 'vote_average',
    headerName: t('movieDetails.voteAverage'),
    type: 'number',
    width: 155,
    valueFormatter: (value: Movie['vote_average']) => `${Math.round(value)}/10`
  },
  {
    field: 'vote_count',
    headerName: t('movieDetails.voteCount'),
    type: 'number',
    width: 140
  },
  {
    field: 'original_language',
    headerName: t('movieDetails.originalLanguage'),
    minWidth: 185,
    align: 'center',
    valueFormatter: (value: Movie['original_language']) => value.toUpperCase()
  }
];
