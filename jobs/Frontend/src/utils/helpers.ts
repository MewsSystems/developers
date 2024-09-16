import { SyntheticEvent } from "react";

export const fallbackImageHandler = (event: SyntheticEvent<HTMLImageElement>) => {
  (event.target as HTMLImageElement).id = `image-fallback`;
  (event.target as HTMLImageElement).srcset =
    "https://placehold.co/92x138";
  return event;
}