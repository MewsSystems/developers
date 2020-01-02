import React from "react";
import "./styles.module.css";

type Props = {
  rate: number;
  trend: "N/A" | "stagnating" | "growing" | "declining";
};

const RateTrends: React.FC<Props> = ({ rate, trend }) => {
  return (
    <div className="trend">
      <span>{rate}</span>
      <span>{trend}</span>
    </div>
  );
};

export default RateTrends;
