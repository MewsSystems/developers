'use client';

import { useSearchParams } from 'next/navigation';
import { FaAngleLeft } from 'react-icons/fa6';
import Link from 'next/link';

export function BackToSearchLink() {
  const searchParams = useSearchParams();
  const search = searchParams.get('search');
  const page = searchParams.get('page');

  const className =
    'flex items-center w-fit text-white bg-cyan-700 p-1 pl-2 pr-3 rounded-lg hover:underline hover:bg-cyan-900 focus:underline focus:bg-cyan-900';

  let href = '/';
  let text = 'Search for more movies';

  if (search) {
    const query = new URLSearchParams({ search });
    if (page && page !== '1') query.set('page', page);
    href = `/?${query.toString()}`;
    text = 'Go back to search';
  }

  return (
    <Link href={href} className={className}>
      <FaAngleLeft className="mr-1" aria-hidden />
      {text}
    </Link>
  );
}
