import { Link, LinkProps } from 'react-router-dom';
import { SearchMovieItem } from '../../services/tmdbApi';
import { splitDate } from '../../utils';
import MoviePoster from '../MoviePoster';
import { Card, CardBody, CardText, CardTitle, CardProps } from './styled';

interface PlainMovieCardProps {
  isLink?: false;
}

interface LinkMovieCardProps {
  isLink: true;
  to: LinkProps['to'];
}

const MovieCard = (
  props: SearchMovieItem &
    CardProps &
    (PlainMovieCardProps | LinkMovieCardProps)
) => {
  const cardProps = props.isLink ? { $as: Link, to: props.to } : {};
  const [releaseYear] = splitDate(props.release_date);
  const details = [
    releaseYear || 'N/A',
    props.original_language.toUpperCase(),
    `${props.vote_average}/10`,
  ];

  return (
    <Card {...cardProps} height={props.height} width={props.width}>
      <MoviePoster
        posterPath={props.poster_path}
        alt={props.title}
        width="92px"
        height={'inherit'}
      />
      <CardBody>
        <CardTitle>
          {props.title}{' '}
          {props.title !== props.original_title && (
            <small>({props.original_title})</small>
          )}
        </CardTitle>
        <div>{details.join(' \u25CF ')}</div>
        <CardText>{props.overview}</CardText>
      </CardBody>
    </Card>
  );
};

export default MovieCard;
