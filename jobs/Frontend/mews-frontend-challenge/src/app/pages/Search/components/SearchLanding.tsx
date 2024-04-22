import { BaseComponentProps } from "@/types";

export function SearchLanding(props: BaseComponentProps) {
  return (
    <div {...props}>
      <h1 className="text-3xl font-bold">Welcome</h1>
      <h2 className="text-xl">
        Millions of movies, TV shows and people to discover. Explore now
      </h2>
    </div>
  );
}
