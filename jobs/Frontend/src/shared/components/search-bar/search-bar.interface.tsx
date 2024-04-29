export interface SearchBarProps {
  searchTerm: string | null;
  onSearch: (value: string) => void;
}
