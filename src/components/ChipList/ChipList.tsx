import React from 'react';

interface ChipListProps {
  title?: string;
  items: string[];
  bgColor?: string;
  textColor?: string;
  direction?: 'row' | 'col';
  className?: string;
}

export function ChipList({
  title,
  items,
  bgColor = 'bg-stone-100',
  textColor = 'text-stone-700',
  direction = 'row',
  className = '',
}: ChipListProps) {
  if (!items.length) return null;

  const containerClasses =
    direction === 'row' ? 'flex flex-wrap gap-2' : 'flex flex-col gap-2 items-start';

  return (
    <div className={className}>
      {title && <h4 className="font-bold mb-2">{title}</h4>}
      <ul className={containerClasses}>
        {items.map((item, index) => (
          <li key={index} className={`text-sm px-3 py-1 rounded-full ${bgColor} ${textColor}`}>
            {item}
          </li>
        ))}
      </ul>
    </div>
  );
}
