import { Input, Label } from "@/app/components/ui"
type MovieCardSearchProps = {
  search: string
  setSearch: (value: string) => void
}
export const MovieCardSearch = ({
  search,
  setSearch,
}: MovieCardSearchProps) => {
  return (
    <div className="space-y-2">
      <Label htmlFor="search" className="text-lg">
        Search
      </Label>
      <Input
        id="search"
        type="search"
        placeholder="Search for movies"
        value={search}
        className="h-14 text-lg"
        onChange={(e) => setSearch(e.target.value)}
      />
    </div>
  )
}
