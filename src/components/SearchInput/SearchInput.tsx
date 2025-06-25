import { Search, X } from "lucide-react"
import { ClearIcon, IconContainer, Input, InputContainer, SearchIcon } from "./SearchInput.styles"

export interface SearchInputProps {
  value: string
  onChange: (value: string) => void
  placeholder?: string
}

export const SearchInput = ({ value, onChange, placeholder }: SearchInputProps) => {
  const handleClear = () => {
    onChange("")
  }

  return (
    <InputContainer>
      <Input
        type="text"
        value={value}
        onChange={(e) => onChange(e.target.value)}
        placeholder={placeholder}
        aria-label="Search for movies"
        role="searchbox"
      />
      <IconContainer>
        {value && (
          <ClearIcon onClick={handleClear} aria-label="Clear search" type="button">
            <X size={16} />
          </ClearIcon>
        )}
        <SearchIcon aria-hidden="true">
          <Search size={20} />
        </SearchIcon>
      </IconContainer>
    </InputContainer>
  )
}
