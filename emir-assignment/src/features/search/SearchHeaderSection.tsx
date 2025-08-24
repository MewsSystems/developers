import SearchHeroDynamic from "../../components/SearchHeroDynamic";
import SearchBar from "../../components/SearchBar";

export default function SearchHeaderSection({
    value,
    onChange,
}: {
    value: string;
    onChange: (v: string) => void;
}) {
    return (
        <SearchHeroDynamic
            title="Discover movies you’ll love"
            subtitle="Search the TMDB catalog"
        >
            <SearchBar
                value={value}
                onChange={onChange}
                autoFocus
                className="w-full"
                placeholder="Search for a movie (e.g., Parasite)..."
            />
        </SearchHeroDynamic>
    );
}
