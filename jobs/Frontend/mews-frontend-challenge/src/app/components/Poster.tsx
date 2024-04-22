import { cn } from "@/design-system/lib/utils";
import { BaseComponentProps } from "@/types";
import { Image } from "lucide-react";
import {
  TMDBResolution,
  getBlurredImageURL,
  getImageURL,
} from "@/app/services/tmdb/";

export type PosterProps = BaseComponentProps & {
  poster?: string;
  resolution?: TMDBResolution;
  alt: string;
};

export function Poster({
  poster,
  className,
  resolution = "w600_and_h900",
  ...props
}: PosterProps) {
  return (
    <div
      className={cn(
        className,
        "relative flex aspect-[2/3] items-center justify-center overflow-hidden bg-slate-300",
      )}
      {...props}
    >
      {!poster && <Image className="h-1/3 w-1/3 text-slate-500" />}
      {poster && (
        <>
          <div
            className="h-full w-full"
            style={{ backgroundImage: getBlurredImageURL(poster) }}
          />
          <img
            className="z-10 h-auto w-full"
            src={getImageURL(resolution, poster)}
          />
        </>
      )}
    </div>
  );
}
