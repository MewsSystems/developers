"use client";

import { Search, X } from "lucide-react";
import { Input } from "~/components/ui/input";

interface SearchBarProps {
  value: string;
  onChange: (value: string) => void;
  placeholder?: string;
}

export function SearchBar({
  value,
  onChange,
  placeholder = "Search the archives...",
}: SearchBarProps) {
  return (
    <div className="relative mx-auto mb-12 max-w-md overflow-hidden ">
      <div className="relative p-1">
        <div className="relative">
          <Search
            className="absolute left-3 top-1/2 -translate-y-1/2 transform text-primary"
            aria-hidden="true"
          />
          <Input
            type="text"
            placeholder={placeholder}
            value={value}
            onChange={(e) => onChange(e.target.value)}
            className="h-12 rounded-full border-primary/30 bg-background/50 pl-10 text-white focus:border-primary focus:ring-primary/50"
            aria-label="Search movies"
          />
          {value && (
            <button
              onClick={() => onChange("")}
              className="absolute right-3 top-1/2 -translate-y-1/2 transform text-primary transition-opacity hover:text-primary/70"
              aria-label="Clear search"
            >
              <X size={18} />
            </button>
          )}
        </div>
      </div>
    </div>
  );
}
