'use client';

import { useSearchParams, usePathname, useRouter } from 'next/navigation';
import { useDebouncedCallback } from 'use-debounce';

export default function ({ placeholder }: { placeholder: string }) {
    const searchParams = useSearchParams();
    const pathname = usePathname();
    const { replace } = useRouter();

    const handleSearch = useDebouncedCallback((term: string) => {
        let params;
        if (searchParams) params = new URLSearchParams(searchParams);

        if (term && params) {
            params.set('query', term);
        } else if (params) {
            params.delete('query');
        }

        replace(`${pathname}?${params?.toString()}`);

    }, 500);

    return (
        <div className="relative flex flex-1 flex-shrink-0">
            <div className="flex flex-col space-y-4 sm:flex-row sm:space-x-4 sm:space-y-0">
                <input className='w-full rounded-md border border-gray-300 bg-white py-2 pl-3 pr-3 leading-4 placeholder-gray-500 text-black text-xs'
                    placeholder={placeholder}
                    onChange={(e) => {
                        handleSearch(e.target.value);
                    }}
                    defaultValue={searchParams?.get('query')?.toString()}
                />
            </div>
        </div>
    );
}
