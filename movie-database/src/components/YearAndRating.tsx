import { StarIcon } from 'lucide-react';

interface Props {
  year: string;
  rating: string;
  ratingCount: number;
  size?: 'sm' | 'md' | 'lg';
}

const YearAndRating = ({ year, rating, ratingCount, size = 'md' }: Props) => {
  return (
    <div className={`flex gap-4 items-center text-${size} text-foreground`}>
      <div>{year}</div>
      <div>|</div>
      <div className="flex gap-1 items-center">
        <StarIcon className='text-primary' size={16} />
        <div>{rating}</div>
        <div>({ratingCount})</div>
      </div>
    </div>
  );
};

export default YearAndRating;

