import { BaseComponentProps } from "@/types";

export type AudienceScoreProps = BaseComponentProps & {
  voteAverage: number;
  voteCount: number;
};

export function AudienceScore({
  voteCount,
  voteAverage,
  ...props
}: AudienceScoreProps) {
  return (
    <div className="flex items-end gap-2" {...props}>
      <span className="text-4xl font-bold">
        {getAudienceScore(voteAverage)}
      </span>
      {voteCount && <span className="text-secondary">{voteCount} reviews</span>}
    </div>
  );
}

function getAudienceScore(voteAverage: number = 0) {
  return `${Math.round(voteAverage * 10)}%`;
}
