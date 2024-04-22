import { BaseComponentProps } from "@/types";

export type MovieTitleProps = BaseComponentProps & {
  title: string;
  releaseDate?: string;
};

export function MovieTitle({ title, releaseDate, ...props }: MovieTitleProps) {
  return (
    <h2 className="text-3xl font-bold" {...props}>
      {title}{" "}
      {releaseDate && (
        <span className="font-light text-secondary">
          ({getYearOfRelease(releaseDate)})
        </span>
      )}
    </h2>
  );
}

function getYearOfRelease(releaseDate: string) {
  return new Date(releaseDate).getFullYear();
}
