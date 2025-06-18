import type { RenderResultNotFoundLayoutParams } from '../../../types';
import { RESULT_NOT_FOUND_SUBTITLE, RESULT_NOT_FOUND_TITLE } from '../../constants';
import { ErrorLayout } from '../ErrorLayout/ErrorLayout';

export const RenderResultNotFoundLayout: React.FC<RenderResultNotFoundLayoutParams> = ({
  isLoading,
  isLoadingPopularMovies,
  listMovies,
  inputSearchDebounced,
}) => {
  if (!isLoading && !isLoadingPopularMovies && !listMovies?.length && inputSearchDebounced) {
    return <ErrorLayout title={RESULT_NOT_FOUND_TITLE} subtitle={RESULT_NOT_FOUND_SUBTITLE} />;
  }

  return null;
};
