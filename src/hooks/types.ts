import type {URLSearchParams} from 'url';

export type SetURLSearchParams = (
  nextInit?: URLSearchParams | string | Record<string, string> | [string, string][] | null,
  navigateOpts?: {replace?: boolean; state?: unknown},
) => void;

export type UsePaginationWithUrlSyncParams = {
  totalPages: number;
  searchQuery: string;
};
