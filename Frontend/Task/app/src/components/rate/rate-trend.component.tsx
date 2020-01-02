import React from "react";
import "./styles.module.css";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { RateProps } from "../../redux/rates/rates.model";

const RateTrends: React.FC<RateProps> = ({ rate, trend }) => {
  let icon;
  if (trend === "N/A" || trend === "stagnating") {
    icon = <FontAwesomeIcon icon="arrow-right" />;
  }
  if (trend === "growing") {
    icon = <FontAwesomeIcon icon="arrow-up" />;
  }
  if (trend === "declining") {
    icon = <FontAwesomeIcon icon="arrow-up" />;
  }
  return (
    <div className="trend">
      <span>{rate}</span>
      <span>{trend}</span>
      <span>{icon}</span>
    </div>
  );
};

export default RateTrends;
