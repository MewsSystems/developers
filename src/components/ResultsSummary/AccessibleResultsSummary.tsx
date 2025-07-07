'use client';

import { type HTMLProps, useEffect, useState } from 'react';

export interface AccessibleResultsSummaryProps extends HTMLProps<HTMLDivElement> {
  currentPage: number;
  totalPages: number;
  totalItems: number;
  pageSize: number;
  addSearchGuidance?: boolean;
}

export function AccessibleResultsSummary({
  currentPage,
  totalPages,
  totalItems,
  pageSize,
  addSearchGuidance = false,
  ...rest
}: AccessibleResultsSummaryProps) {
  const isNoResults = totalItems === 0;

  const summary = isNoResults
    ? 'No results match your search'
    : `Page ${currentPage} of ${totalPages}. Results ${(currentPage - 1) * pageSize + 1} to ${Math.min(currentPage * pageSize, totalItems)} of ${totalItems}.`;

  const searchGuidance = addSearchGuidance
    ? `To start a new search navigate to the search input above.`
    : '';

  const [displayedSummary, setDisplayedSummary] = useState(summary);
  const [displayedSearchGuidance, setDisplayedSearchGuidance] = useState(searchGuidance);

  useEffect(() => {
    // Clear, then after timer, show the new summary (to trigger screen reader re-announcement)
    setDisplayedSummary('');
    setDisplayedSearchGuidance('');
    const timeout = setTimeout(() => {
      setDisplayedSummary(summary);
      setDisplayedSearchGuidance(searchGuidance);
    }, 100);
    return () => clearTimeout(timeout);
  }, [summary, searchGuidance]);

  return (
    <div
      role="region"
      aria-label="Search results summary"
      aria-live="polite"
      className="text-cyan-700 text-sm text-center"
      {...rest}
    >
      {displayedSummary}
      {displayedSearchGuidance && <span className="sr-only">{displayedSearchGuidance}</span>}
    </div>
  );
}
