import { formatList, parseBackdropToImgPath } from "@/shared/lib/utils";
import type { Collection } from "@/entities/movie/types";
import type { Configuration } from "@/entities/configuration/types";
import type { CollectionDetail } from "@/features/collection/types";

export function parseCollection({
  collection,
  configuration,
  language,
}: {
  collection: Collection;
  configuration: Configuration;
  language: string;
}): CollectionDetail {
  const backdropSizePosition = 6;

  const backdrop_img_path = collection.backdrop_path
    ? parseBackdropToImgPath(
        configuration.images,
        collection.backdrop_path,
        backdropSizePosition
      )
    : "";
  const bgImage = `url(${backdrop_img_path})`;
  const parts = formatList(
    collection.parts.map(({ title }) => title),
    language
  );

  return {
    bgImage,
    parts,
    name: collection.name,
  };
}
