'use client';

import { useSearchParams } from 'next/navigation';
import Link from 'next/link';

export function BackToSearchLink() {
  const searchParams = useSearchParams();
  const search = searchParams.get('search');
  const page = searchParams.get('page');

  const className = 'text-purple-800 hover:underline focus:underline block mb-4 w-fit';

  if (search) {
    const query = new URLSearchParams({ search });
    if (page && page !== '1') query.set('page', page);

    return (
      <Link href={`/?${query.toString()}`} className={className}>
        Go back to search
      </Link>
    );
  }

  return (
    <Link href="/" className={className}>
      Search for more movies
    </Link>
  );
}
