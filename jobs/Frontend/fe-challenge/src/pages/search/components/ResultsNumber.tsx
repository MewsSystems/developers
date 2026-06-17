interface ResultsNumberProps {
  query: string;
  count: number;
}

const ResultsNumber = ({ count, query }: ResultsNumberProps) => {
  return (
    <>
      <h2 className="mb-2">
        <span className="font-semibold">{count}</span> matches for{' '}
        <span className="font-semibold italic">{query}</span>
      </h2>
      <hr className="mb-3" />
    </>
  );
};

export default ResultsNumber;
