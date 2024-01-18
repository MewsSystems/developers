import { ReduxState } from '@/store';

export const currentSearchTerm = (state: ReduxState) => state['search'].query;
