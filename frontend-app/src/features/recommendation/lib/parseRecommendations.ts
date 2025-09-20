import { parseBackdropToImgPath, parseWidth } from "@/shared/lib/utils";
import type { Recommendation } from "@/entities/movie/types";
import type { Configuration } from "@/entities/configuration/types";

export function parseRecommendations({
  recommendations,
  configuration,
}: {
  recommendations: Recommendation[];
  configuration: Configuration;
}) {
  const backdropSizePosition = 0;
  const backdropWidth = parseWidth(
    configuration.images.backdrop_sizes[backdropSizePosition]
  );
  return recommendations.map((recommendation) => {
    return {
      id: recommendation.id,
      title: recommendation.title,
      voteAverage: (recommendation.vote_average * 10).toFixed(0) + "%",
      backdrop: {
        width: backdropWidth,
        imgSrc: parseBackdropToImgPath(
          configuration.images,
          recommendation.backdrop_path,
          backdropSizePosition
        ),
      },
    };
  });
}
