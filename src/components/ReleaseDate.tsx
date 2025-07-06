import { formatDate } from '@/lib/format';
import { DescriptionListItem } from '@/components/TermDetail';

interface ReleaseDateProps {
  date: string;
  isSmall?: boolean;
}

export function ReleaseDate({ date, isSmall = false }: ReleaseDateProps) {
  const detailClassName = isSmall ? 'text-sm text-stone-800 leading-none' : '';
  const termClassName = isSmall ? 'text-sm font-normal leading-none' : 'before:mt-2';

  return (
    <DescriptionListItem
      term="Released: "
      detail={date ? <time dateTime={date}>{formatDate(date)}</time> : 'unknown'}
      termClassName={termClassName}
      detailClassName={`inline ${detailClassName}`}
    />
  );
}
