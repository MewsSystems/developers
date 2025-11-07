import classNames from 'classnames';
import TextInput from '@/components/TextInput';

interface SearchInputProps {
  value?: string;
  onChange?: (value: string) => void;
  placeholder?: string;
  className?: string;
}

const SearchInput = ({
  placeholder = 'Search',
  value,
  onChange = () => undefined,
  className,
}: SearchInputProps) => {
  return (
    <div className={classNames('inline-block relative', className)}>
      <div className="absolute top-3 left-3 text-gray-400">
        <svg
          xmlns="http://www.w3.org/2000/svg"
          className="h-5 w-5"
          fill="none"
          viewBox="0 0 24 24"
          stroke="currentColor"
        >
          <path
            strokeLinecap="round"
            strokeLinejoin="round"
            strokeWidth="2"
            d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z"
          />
        </svg>
      </div>
      <TextInput
        value={value}
        placeholder={placeholder}
        className="w-full pl-10"
        onChange={(e) => onChange(e.target.value)}
      />
    </div>
  );
};

export default SearchInput;
