import { type HTMLProps, useEffect, useMemo, useState } from 'react';

export interface AccessibleResultsSummaryProps extends HTMLProps<HTMLDivElement> {
  currentPage: number;
  totalPages: number;
  totalItems: number;
  pageSize: number;
  addSearchGuidance?: boolean;
  isHidden?: boolean;
}

export function AccessibleResultsSummary({
  currentPage,
  totalPages,
  totalItems,
  pageSize,
  addSearchGuidance = false,
  isHidden = false,
  ...rest
}: AccessibleResultsSummaryProps) {
  const isNoResults = totalItems === 0;

  const summary = isNoResults
    ? 'No results match your search'
    : `Page ${currentPage} of ${totalPages}. Results ${(currentPage - 1) * pageSize + 1} to ${Math.min(
        currentPage * pageSize,
        totalItems
      )} of ${totalItems}.`;

  const guidance = addSearchGuidance
    ? 'To start a new search navigate to the search input above.'
    : '';

  const announcement = useMemo(() => {
    if (isHidden) return '';
    return `${summary}${guidance ? ' ' + guidance : ''}`;
  }, [isHidden, summary, guidance]);

  const [ariaMessage, setAriaMessage] = useState(announcement);

  useEffect(() => {
    setAriaMessage('');
    const timeout = setTimeout(() => {
      setAriaMessage(announcement);
    }, 100);

    return () => clearTimeout(timeout);
  }, [announcement]);

  return (
    <div
      role="region"
      aria-label="Search results summary"
      className="text-cyan-700 text-sm text-center"
      {...rest}
    >
      {/* Screen reader only announcement (always present for live updates) */}
      <span className="sr-only" aria-live="polite" aria-atomic="true">
        {ariaMessage}
      </span>

      {/* Visible summary, only shown when not hidden */}
      {!isHidden && <span aria-hidden="true">{summary}</span>}
    </div>
  );
}
