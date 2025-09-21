import { slice } from "lodash";
import type { MovieDetailsAppended } from "@/entities/movie/types";

export function parseReviews({ movie }: { movie: MovieDetailsAppended }) {
  const MAX_TEXT = 100;
  const reviews = movie.reviews.results;
  const reviewsToShow = slice(reviews, 0, 1);
  return reviewsToShow.map((review) => {
    return {
      id: review.id,
      sliced_content: review.content.slice(0, MAX_TEXT),
      fullContent: review.content,
    };
  });
}
