import React, { Component } from "react";
import styled from "styled-components";

import CheckSvg from "./check.svg";

const Form = styled.form`
  display: flex;
  margin-left: 90px;
  padding: 16px;
  border-radius: 5px;
  background-color: #fff;
`;

const InputContainer = styled.div`
  position: relative;
  appearance: none;
  display: flex;
  align-items: center;
  margin-right: 28px;
`;

const InputCheckbox = styled.input`
  appearance: none;
  width: 18px;
  height: 18px;
  background: #ffffff;
  border: 1px solid #cfd9ed;
  box-sizing: border-box;
  border-radius: 5px;
  transition: 200ms;

  &:hover {
    border-color: #383838;
  }

  &:checked {
    background: #cfd9ed;
  }
`;

const InputLabel = styled.label`
  font-size: 12px;
  color: #727272;
`;

class RatesFilter extends Component {
  constructor(props) {
    super(props);

    this.filterForm = React.createRef();
  }

  handleFormSubmit = e => {
    const activeCurrencyPairs = [...e.target.elements]
      .filter(input => (input.checked ? input.value : null))
      .map(element => element.value);

    this.props.updateActiveCurrencyPairs(activeCurrencyPairs);
    e.preventDefault();
  };

  render() {
    return (
      <Form ref={this.filterForm} onSubmit={this.handleFormSubmit}>
        {Object.keys(this.props.currencyPairs).map(key => {
          const label = `${this.props.currencyPairs[key][0].code}/${
            this.props.currencyPairs[key][1].code
          }`;
          return (
            <InputContainer key={label}>
              <CheckSvg
                style={{
                  position: "absolute",
                  left: "9px",
                  pointerEvents: "none"
                }}
              />
              <InputCheckbox
                type="checkbox"
                id={label}
                name="currencies[]"
                value={key}
                checked={this.props.activeCurrencyPairs.indexOf(key) !== -1}
                onChange={
                  e =>
                    this.filterForm.current.dispatchEvent(new Event("submit")) //submit form
                }
              />
              <InputLabel htmlFor={label}>{label}</InputLabel>
            </InputContainer>
          );
        })}
      </Form>
    );
  }
}

export default RatesFilter;
