export interface MovieGridProps {
  readonly children: React.ReactNode;
}

export const MovieGrid = (props: MovieGridProps) => {
  return (
    <ul className="box-border grid h-full grid-flow-row auto-rows-[300px] grid-cols-[repeat(3,200px)] gap-6 lg:grid-cols-[repeat(5,200px)]">
      {props.children}
    </ul>
  );
};
