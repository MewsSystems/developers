import React from "react";
import { useSelector, useDispatch } from "react-redux";
import { Button, Input } from "reactstrap";
import {
  SelectorsWrapper,
  SelectorsContent,
  SelectorPairs
} from "./StyledSelectorsComponents";
import { updateFilters, setFilters } from "../store/actions";

export const Selectors = () => {
  const { currencyPairs, filteredPairs } = useSelector(state => state);
  const dispatch = useDispatch();
  const handleCheckboxChange = id => {
    dispatch(updateFilters(id));
  };
  const handleSelectAllClick = () => {
    dispatch(setFilters(Object.keys(currencyPairs)));
  };
  const handleUnselectAllClick = () => {
    dispatch(setFilters([]));
  };
  return (
    <SelectorsWrapper>
      <Button outline color="success" onClick={handleSelectAllClick}>
        Select all
      </Button>{" "}
      <Button outline color="warning" onClick={handleUnselectAllClick}>
        Unselect all
      </Button>
      <SelectorsContent>
        {Object.entries(currencyPairs).map(([id, pair]) => {
          return (
            <SelectorPairs key={id}>
              <Input
                type="checkbox"
                checked={filteredPairs.includes(id)}
                onChange={() => {
                  handleCheckboxChange(id);
                }}
              />
              {pair.shortcut}
            </SelectorPairs>
          );
        })}
      </SelectorsContent>
    </SelectorsWrapper>
  );
};
