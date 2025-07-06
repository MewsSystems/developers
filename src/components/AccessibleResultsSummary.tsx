import { Ref, useEffect, useRef } from 'react';
import { mergeRefs } from '@/lib/mergRefs';

export interface AccessibleResultsSummaryProps extends React.HTMLProps<HTMLDivElement> {
  currentPage: number;
  totalPages: number;
  totalItems: number;
  pageSize: number;
  ref?: Ref<HTMLDivElement>;
}

export function AccessibleResultsSummary({
  currentPage,
  totalPages,
  totalItems,
  pageSize,
  ref,
  ...rest
}: AccessibleResultsSummaryProps) {
  const summaryRef = useRef<HTMLDivElement>(null);

  let summary: string;
  const isNoResults = totalItems === 0;
  if (isNoResults) {
    summary = 'No results match your search';
  } else {
    const firstItemIndex = (currentPage - 1) * pageSize + 1;
    const lastItemIndex = Math.min(currentPage * pageSize, totalItems);
    summary = `Page ${currentPage} of ${totalPages}. Results ${firstItemIndex} to ${lastItemIndex} of ${totalItems}.`;
  }

  useEffect(() => {
    // Force screen readers to announce updates:
    // Clear and reset text to reliably trigger aria-live announcement.
    if (summaryRef.current) {
      summaryRef.current.textContent = '';
      setTimeout(() => {
        if (summaryRef.current) {
          summaryRef.current.textContent = summary;
        }
      }, 100);
    }
  }, [summary]);

  return (
    <div
      role="region"
      aria-label="Search results summary"
      aria-live="polite"
      ref={mergeRefs(ref, summaryRef)}
      tabIndex={isNoResults ? -1 : undefined}
      className="text-cyan-700 text-sm text-center"
      {...rest}
    >
      {summary}
    </div>
  );
}
