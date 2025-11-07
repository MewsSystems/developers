import { ReactNode } from 'react';
import PopcornIcon from '@/components/PopcornIcon';

interface Props {
  children: ReactNode;
}

const EmptySearch = ({ children }: Props) => {
  return (
    <div className="flex flex-col mt-12 justify-center items-center">
      <PopcornIcon className="w-14 h-14" />
      {children}
    </div>
  );
};

EmptySearch.Title = ({ children }: Props) => (
  <h3 className="text-2xl font-bold m-2">{children}</h3>
);

EmptySearch.Description = ({ children }: Props) => (
  <p className="text-lg">{children}</p>
);

export default EmptySearch;
