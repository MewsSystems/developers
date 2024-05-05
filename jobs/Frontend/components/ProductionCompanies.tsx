"use client";

import styled from "styled-components";
import { ProductionCompany } from "@/interfaces/movie";

const SmallHeading = styled.h4`
  margin-bottom: 0;
`;

interface ProductionCompaniesProps {
  productionCompanies: ProductionCompany[];
}

const ProductionCompanies = ({
  productionCompanies,
}: ProductionCompaniesProps) => {
  return (
    <>
      <SmallHeading>Production Companies:</SmallHeading>
      <ul>
        {productionCompanies?.map((productionCompany, index) => (
          <li key={index}>
            {productionCompany.name}
            {productionCompany.originCountry && (
              <> - {productionCompany.originCountry}</>
            )}
          </li>
        ))}
      </ul>
    </>
  );
};

export default ProductionCompanies;
