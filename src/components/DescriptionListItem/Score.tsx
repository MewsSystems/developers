import { DescriptionListItem } from '@/components/DescriptionListItem';
import { formatVote } from '@/lib/format';

interface ScoreProps {
  score: number;
  count: number;
  isSmall?: boolean;
}

export function Score({ score, count, isSmall = false }: ScoreProps) {
  const detailClassName = isSmall ? 'text-sm text-stone-800 leading-none' : '';
  const termClassName = isSmall ? 'text-sm font-normal leading-none' : 'before:mt-2';

  return (
    <DescriptionListItem
      term="Score: "
      detail={formatVote(score, count)}
      termClassName={termClassName}
      detailClassName={`inline ${detailClassName}`}
    />
  );
}
