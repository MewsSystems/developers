import { useId } from "react";

export const DataPoint = ({
  title,
  value,
}: {
  title: string;
  value: string | number | undefined;
}) => {
  const id = useId();
  return (
    <div className="flex">
      <dt id={id} className="font-bold">
        {title}:
      </dt>{" "}
      <dd aria-labelledby={id}>{value}</dd>
    </div>
  );
};
