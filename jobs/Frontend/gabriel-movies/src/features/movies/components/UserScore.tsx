import { Badge } from "@/shared/ui/atoms/Badge";
import { roundVoteAverageToPercent } from "../utils/roundVoteAverageToPercent";

function variantFor(score: number) {
  if (score >= 75) return "success" as const;
  if (score >= 50) return "warning" as const;
  return "danger" as const;
}

type UserScoreProps = {
  voteAverage: number;
};


export function UserScore({ voteAverage }: UserScoreProps) {
  const score = roundVoteAverageToPercent(voteAverage);
  return (
    <Badge $variant={variantFor(score)} aria-label={`User score ${score}%`}>
      {score}%
    </Badge>
  );
}
