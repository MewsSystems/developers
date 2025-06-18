import type { RenderLoaderParams } from '../../../types';
import { CardSkeleton } from '../CardSkeleton';
import { WrapperListMovies } from '../WrapperListMovies';

export const RenderLoader: React.FC<RenderLoaderParams> = ({
  isLoading,
  isLoadingPopularMovies,
}) => {
  if (isLoading || isLoadingPopularMovies) {
    return (
      <WrapperListMovies>
        {Array.from({ length: 8 }).map((_, i) => (
          <CardSkeleton key={i} />
        ))}
      </WrapperListMovies>
    );
  }
  return null;
};
