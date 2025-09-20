import { slice } from "lodash";
import { parseProfileToImgPath, parseWidth } from "@/shared/lib/utils";
import type { MovieDetailsAppended } from "@/entities/movie/types";
import type { Configuration } from "@/entities/configuration/types";
import type { CastImg } from "@/features/topBilledCast/types";

export function parseCastImgs({
  movie,
  configuration,
}: {
  movie: MovieDetailsAppended;
  configuration: Configuration;
}): CastImg[] {
  const MAX_CAST = 9;
  const cast = slice(movie.credits.cast, 0, MAX_CAST);
  const profileSizePosition = 1;
  const profileWidth = parseWidth(
    configuration.images.profile_sizes[profileSizePosition]
  );

  return cast.map((c) => {
    return {
      id: c.id,
      width: profileWidth,
      img: parseProfileToImgPath(
        configuration.images,
        c.profile_path + "",
        profileSizePosition
      ),
      character: c.character,
      name: c.name,
    };
  });
}
