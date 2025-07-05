import { formatDate } from '@/lib/format';

interface ReleaseDateProps {
  date: string;
}

export function ReleaseDate({ date }: ReleaseDateProps) {
  return (
    <p className="text-stone-800 text-sm mt-1">
      <span className="text-cyan-700">Released:</span>{' '}
      {date ? <time dateTime={date}>{formatDate(date)}</time> : 'unknown'}
    </p>
  );
}
