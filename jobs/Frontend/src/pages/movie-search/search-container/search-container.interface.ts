export interface SearchContainerProps {
  searchTerm: string | null;
  handleOnSearch: (searchTerm: string) => void;
}
