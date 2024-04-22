import { SearchIcon } from "lucide-react";
import { useState } from "react";
import { useDebounce } from "react-use";

import { Input } from "@/design-system/components/ui/input";
import { useSearchFilters } from "../Search.data";
import { useHistory } from "react-router-dom";
import { getSearchRoute } from "@/app/AppRouter.utils";

export function SearchInput() {
  const history = useHistory();
  const searchParams = useSearchFilters();
  const initialQuery = searchParams.q;

  const [searchQuery, setSearchQuery] = useState(initialQuery);

  useDebounce(
    () => {
      if (initialQuery === searchQuery) return;
      history.push(getSearchRoute({ ...searchParams, q: searchQuery }));
    },
    300,
    [searchQuery],
  );

  return (
    <fieldset className="relative">
      <Input
        type="text"
        placeholder="Search for a movie..."
        aria-label="Search movie input"
        value={searchQuery}
        onChange={(e) => setSearchQuery(e.target.value)}
      />
      <SearchIcon className="absolute right-3 top-0 h-full text-secondary" />
    </fieldset>
  );
}
