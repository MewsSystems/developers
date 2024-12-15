import React from "react";

interface SearchBarProps {
  value: string;
  onChange: (query: string) => void;
}

export const SearchBar: React.FC<SearchBarProps> = ({ value, onChange }) => {
  const handleInputChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    onChange(event.target.value);
  };

  return (
    <div className="flex items-center bg-darkSoft rounded-lg px-6 py-4 shadow-lg max-w-3xl mx-auto">
      <input
        type="text"
        value={value}
        onChange={handleInputChange}
        placeholder="Search for a movie..."
        className="w-full bg-transparent text-gray-200 placeholder-gray-400 focus:outline-none text-lg"
      />
      {value && (
        <button
          type="button"
          onClick={() => onChange("")}
          className="text-gray-400 hover:text-gray-200"
          aria-label="Clear search"
        >
          ✕
        </button>
      )}
    </div>
  );
};
